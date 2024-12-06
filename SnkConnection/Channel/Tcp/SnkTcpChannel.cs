using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using SnkConnection.Codec;
using SnkConnection.IO;

namespace SnkConnection
{
    namespace Channel
    {
        namespace Tcp
        {
            public class SnkTcpChannel<TMessage> : SnkChannel<TMessage>, ISnkTcpChannel<TMessage> where TMessage : ISnkMessage
            {
                private Socket _socket;
                private readonly SemaphoreSlim _connectSemaphore = new SemaphoreSlim(1, 1);

                public bool Connected { get; private set; } = false;

                public SnkTcpChannel(ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian, bool noDelay, int receiveBufferSize, int sendBufferSize) : base(codecFactory, bigEndian, noDelay, receiveBufferSize, sendBufferSize)
                {
                }

                public override async Task OpenAsync(string host, int port, int timeoutMilliseconds, CancellationToken cancellationToken)
                {
                    if (await _connectSemaphore.WaitAsync(timeoutMilliseconds, cancellationToken).ConfigureAwait(false) == false)
                        throw new Exception($"[Net-SnkTcpChannel]连接超时或者被取消。");

                    if (_socket != null)
                        throw new Exception("[Net-SnkTcpChannel]Socket已存在。");

                    try
                    {
                        _socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
                        {
                            NoDelay = noDelay,
                            ReceiveBufferSize = receiveBufferSize,
                            SendBufferSize = sendBufferSize
                        };

                        var dnsException = default(Exception);
                        foreach (var address in await Dns.GetHostAddressesAsync(host).ConfigureAwait(false))
                        {
                            try
                            {
                                await _socket.ConnectAsync(address, port).ConfigureAwait(false);

                                this.writer = new SnkNetworkWriter(this._socket, true);
                                this.reader = new SnkNetworkReader(this._socket, true);

                                dnsException = null;
                                Connected = true;
                                break;
                            }
                            catch(Exception exception)
                            {
                                dnsException = exception;
                            }
                        }

                        if (dnsException != null)
                            throw dnsException;

                        if (Connected == false)
                            throw new Exception($"[Net-SnkTcpChannel]连接失败。");

                    }
                    catch(Exception exception)
                    {
                        throw new Exception($"[Net-SnkTcpChannel]连接异常。", exception);
                    }
                    finally
                    {
                        _connectSemaphore.Release();
                    }
                }

                public override async Task CloseAsync()
                {
                    await _connectSemaphore.WaitAsync().ConfigureAwait(false);
                    try
                    {
                        if (_socket != null)
                        {
                            _socket.Close();
                            _socket.Dispose();
                            Connected = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"[Net-SnkTcpChannel]关闭异常。", exception);
                    }
                    finally
                    {
                        _socket = null;
                        _connectSemaphore.Release();
                    }
                }

                protected override void OnDispose()
                {
                    try
                    {
                        _ = CloseAsync();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"[Net-SnkTcpChannel]释放异常。", exception);
                    }
                }
            }
        }
    }
}
