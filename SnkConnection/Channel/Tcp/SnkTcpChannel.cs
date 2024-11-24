using SnkConnection.Codec;
using SnkConnection.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
                        throw new Exception($"连接超时或者被取消。");

                    if (_socket != null)
                        throw new Exception("Socket已存在");

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

                                this.writer = new SnkBinaryWriter(this._socket, true);
                                this.reader = new SnkBinaryReader(this._socket, true);

                                Connected = true;
                                break;
                            }
                            catch(Exception exception)
                            {
                                Console.WriteLine($"连接失败，地址: {address}, 异常: {exception.Message}");
                                dnsException = exception;
                            }
                        }

                        if (dnsException != null)
                            throw dnsException;

                        if (Connected == false)
                            throw new Exception($"连接失败，未知的异常，或许所有DNS均不可用。 host：{host}");

                    }
                    catch(Exception exception)
                    {
                        throw new Exception($"连接异常。主机: {host}, 端口: {port}\n原始异常: {exception.Message}", exception);
                    }
                    finally
                    {
                        _connectSemaphore.Release();
                    }
                }

                public override async Task CloseAsync()
                {
                    try
                    {
                        await _connectSemaphore.WaitAsync().ConfigureAwait(false);
                        if (_socket != null) 
                        {
                            _socket.Shutdown(SocketShutdown.Both);
                            _socket.Close();
                            Connected = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"断开异常。\n原始异常: {exception.Message}", exception);
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Dispose 异常: {ex.Message}");
                    }
                }
            }
        }
    }
}
