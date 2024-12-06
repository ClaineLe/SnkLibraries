using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using SnkConnection.Channel;
using SnkConnection.Codec;

namespace SnkConnection
{
    public enum SnkExceptionCode
    {
        Connect,
        ProcessReceive,
        ProcessSend,
        Notify,
        Disconnect,
        Dispose,
    }

    public delegate void StateHandle(SnkConnectorState oldState, SnkConnectorState newState);
    public delegate void ReceiveHandle(ISnkMessage message);
    public delegate void ExceptionHandle(SnkExceptionCode exceptionCode, Exception exception);

    public class SnkConnector<TRequest, TResponse, TNotification> : SnkDisposable, ISnkConnector
        where TRequest : class, ISnkRequest
        where TResponse : class, ISnkResponse
        where TNotification : class, ISnkNotification
    {
        protected SemaphoreSlim connectSemaphore = new SemaphoreSlim(1, 1);

        public SnkConnectorName ConnectorName { get; }

        private SnkConnectorState _state = SnkConnectorState.None;
        private ISnkChannel<ISnkMessage> _channel;

        private ISnkChannelFactory<ISnkMessage> _channelFactory;
        private ISnkMessageCodecFactory<ISnkMessage> _codecFactory;
        private ConcurrentQueue<ISnkMessage> _willSendQueue = new ConcurrentQueue<ISnkMessage>();

        private StateHandle _stateHandler;
        private ReceiveHandle _receiveHandler;
        private ExceptionHandle _exceptionHandler;

        public SnkConnectorState State
        {
            get => _state;
            protected set
            {
                if (_state == value)
                    return;
                var oldState = _state;
                _state = value;
                _stateHandler?.Invoke(oldState, _state);
            }
        }

        public StateHandle StateHandler 
        {
            get => _stateHandler;
            set => _stateHandler = value;
        }

        public ReceiveHandle ReceiveHandler
        {
            get => _receiveHandler;
            set => _receiveHandler = value;
        }

        public ExceptionHandle ExceptionHandler
        {
            get => _exceptionHandler;
            set => _exceptionHandler = value;
        }

        public SnkConnector(SnkConnectorName connectorName, ISnkChannelFactory<ISnkMessage> channelFactory, ISnkMessageCodecFactory<ISnkMessage> codecFactory)
        {
            ConnectorName = connectorName;
            _channelFactory = channelFactory;
            _codecFactory = codecFactory;
        }

        public async Task ConnectAsync(string host, int port, int timeoutMilliseconds)
        {
            if (await connectSemaphore.WaitAsync(timeoutMilliseconds).ConfigureAwait(false) == false)
                throw new TimeoutException($"连接超时:{State}");

            ValidateDisposed();

            if (timeoutMilliseconds <= 0)
                timeoutMilliseconds = SnkConnectionDefines.CONNECT_TIMEOUT_MILLISECONDS;

            try
            {
                if (State != SnkConnectorState.None)
                    throw new Exception($"连接状态异常:{State}");

                if (this._channel == null)
                    this._channel = _channelFactory.CreateTcpChannel(ConnectorName, this._codecFactory);

                State = SnkConnectorState.Connecting;
                await this._channel.OpenAsync(host, port, timeoutMilliseconds, CancellationToken.None).ConfigureAwait(false);
                State = SnkConnectorState.Connected;

                //收发两线程，看需要可改单线程
                _ = Task.Factory.StartNew(ProcessReceive, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                _ = Task.Factory.StartNew(ProcessSend, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (Exception exception)
            {
                ProcessException(SnkExceptionCode.Connect, exception);
            }
            finally 
            {
                connectSemaphore.Release();
            }
        }

        public Task<ISnkResponse> Request(ISnkRequest request) => throw new NotImplementedException();

        public void Notify(ISnkNotification notification)
        {
            try 
            {
                if (this.State == SnkConnectorState.Connected)
                {
                    ValidateDisposed();
                    this._willSendQueue.Enqueue(notification);
                }
            }
            catch (Exception exception)
            {
                ProcessException(SnkExceptionCode.Notify, exception);
            }
        }

        protected void ProcessException(SnkExceptionCode exceptionCode, Exception exception)
        {
            this.State = SnkConnectorState.Exception;
            this.ExceptionHandler?.Invoke(exceptionCode, exception);
        }

        protected async Task ProcessReceive()
        {
            try
            {
                while (!this.isDisposed && this.State == SnkConnectorState.Connected && this._channel != null)
                {
                    var message = await this._channel.ReadAsync().ConfigureAwait(false);
                    if (message != null)
                        this.ReceiveHandler?.Invoke(message);
                }
            }
            catch (Exception exception)
            {
                ProcessException(SnkExceptionCode.ProcessReceive, exception);
            }
        }

        protected void ProcessSend() 
        {
            try
            {
                while (!this.isDisposed && this.State == SnkConnectorState.Connected && this._channel != null)
                {
                    while (_willSendQueue.TryDequeue(out var message))
                        _ = this._channel?.WriteAsync(message).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                ProcessException(SnkExceptionCode.ProcessSend, exception);
            }
        }

        public async Task Disconnect()
        {
            await connectSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_channel != null)
                {
                    await _channel.CloseAsync().ConfigureAwait(false);
                    _channel = null;
                }

                if (_willSendQueue != null)
                {
                    while (!_willSendQueue.IsEmpty)
                        _willSendQueue.TryDequeue(out _);
                }
            }
            catch (System.Exception exception)
            {
                ProcessException(SnkExceptionCode.Disconnect, exception);
            }
            finally 
            {
                this.State = SnkConnectorState.None;
                connectSemaphore.Release();
            }
        }

        protected override void OnDispose()
        {
            if (this._stateHandler != null)
                this._stateHandler = null;

            if (this._receiveHandler != null)
                this._receiveHandler = null;

            if (this._exceptionHandler != null)
                this._exceptionHandler = null;

            try
            {
                _ = Disconnect();
            }
            catch(System.Exception exception)
            {
                ProcessException(SnkExceptionCode.Dispose, exception);
            }
        }
    }
}
