using SnkConnection.Channel;
using SnkConnection.Codec;
using SnkConnection.EventArguments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    public class SnkConnector<TRequest, TResponse, TNotification> : SnkDisposable, ISnkConnector<TRequest, TResponse, TNotification>
        where TRequest : class, ISnkRequest
        where TResponse : class, ISnkResponse
        where TNotification : class, ISnkNotification
    {
        protected readonly SemaphoreSlim connectLock = new SemaphoreSlim(1, 1);

        private string _host;
        private int _port;
        private int _connectTimeout;
        private int _reconnectIntervalTime = -1;
        private int _reconnectCurrentTimes = -1;

        private ISnkChannel<ISnkMessage> _channel;
        private SnkEventPublisher _eventPublisher;

        private EventHandler<TNotification> _notificationHandler;
        private EventHandler<ISnkConnectionEventArgs> _connectionEventHandler;

        public EventHandler<TNotification> NotificationHandler
        {
            get => _notificationHandler;
            set => _notificationHandler = value;
        }
        public EventHandler<ISnkConnectionEventArgs> ConnectionEventHandler
        {
            get => _connectionEventHandler;
            set => _connectionEventHandler = value;
        }

        public SnkConnector(string connectorName, ISnkChannelFactory<ISnkMessage> channelFactory, ISnkMessageCodecFactory<ISnkMessage> codecFactory)
        {
            this._channel = channelFactory.CreateTcpChannel(connectorName, codecFactory);
            this._eventPublisher = new SnkEventPublisher(this, _connectionEventHandler);
        }

        public async Task ConnectAsync(string host, int port, int timeoutMilliseconds, CancellationToken cancellationToken)
        {
            ValidateDisposed();

            if (timeoutMilliseconds <= 0)
                timeoutMilliseconds = SnkConnectionDefines.CONNECT_TIMEOUT_MILLISECONDS;

            if (!await connectLock.WaitAsync(timeoutMilliseconds, cancellationToken).ConfigureAwait(false)) 
                throw new Exception($"连接超时或者取消。");

            var connectionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            try
            {
                this._eventPublisher.PublishConnect(false, true);
                this._host = host;
                this._port = port;
                this._connectTimeout = timeoutMilliseconds;

                await this._channel.OpenAsync(host, port, timeoutMilliseconds, connectionTokenSource.Token).ConfigureAwait(false);
                this._eventPublisher.PublishConnect(true, false);
            }
            catch(Exception exception)
            {
                this._eventPublisher.PublishConnect(false, false);
                this._eventPublisher.PublishException("连接异常", exception);
            }
            finally 
            {
                connectionTokenSource?.Dispose();
                connectLock.Release();
            }
        }

        public async Task Disconnect()
        {
            try 
            {
                if (_channel != null)
                {
                    await _channel.CloseAsync().ConfigureAwait(false);
                    _channel = null;
                }
            }
            catch (Exception exception)
            {
                this._eventPublisher.PublishException("断开异常", exception);
            }
        }

        private void ResetReconnectInfo() 
        {
            _reconnectIntervalTime = -1;
            _reconnectCurrentTimes = -1;
        }

        private int ReconnectIntervalTimeBackoff() 
        {
            if (_reconnectIntervalTime < 0)
                _reconnectIntervalTime = SnkConnectionDefines.RECONNECT_INTERVAL_TIME;
            else
                _reconnectIntervalTime += SnkConnectionDefines.RECONNECT_INTERVAL_TIME_BACKOFF;
            return _reconnectIntervalTime;
        }

        public async Task Reconnect(CancellationToken cancellationToken)
        {
            try 
            {
                await Disconnect().ConfigureAwait(false);

                if (Interlocked.Increment(ref _reconnectCurrentTimes) > SnkConnectionDefines.RECONNECT_MAX_TIMES)
                    return;

                var reconnectDelayTime = ReconnectIntervalTimeBackoff();
                await Task.Delay(reconnectDelayTime).ConfigureAwait(false);
                await ConnectAsync(this._host, this._port, this._connectTimeout, cancellationToken).ConfigureAwait(false);
                ResetReconnectInfo();
            }
            catch (Exception exception)
            {
                this._eventPublisher.PublishException("断线重连", exception);
            }
        }

        public async Task<TResponse> Request(TRequest request)
        {
            try
            {
                ValidateDisposed();
                await this._channel.WriteAsync(request).ConfigureAwait(false);
                return null;
            }
            catch (Exception exception)
            {
                this._eventPublisher.PublishException("请求异常", exception);
            }
            return null;
        }

        public async Task Notify(TResponse response)
        {
            try
            {
                ValidateDisposed();
                await this._channel.WriteAsync(response).ConfigureAwait(false);
            }
            catch (Exception exception) 
            {
                this._eventPublisher.PublishException("推送异常", exception);
            }
        }

        protected override void OnDispose()
        {
            if (_notificationHandler != null)
                _notificationHandler = null;

            try
            {
                Disconnect().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                this._eventPublisher.PublishException("释放异常", exception);
            }
        }
    }
}
