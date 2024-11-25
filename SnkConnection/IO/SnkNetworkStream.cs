using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public abstract class SnkNetworkStream : SnkDisposable, ISnkNetworkStream
        {
            private NetworkStream _networkStream;
            protected bool isBigEndian;
            public bool IsBigEndian => isBigEndian;

            public abstract int BufferSize { get; }

            public SnkNetworkStream(Socket socket, bool isBigEndian)
            {
                this._networkStream = new NetworkStream(socket);// networkStream ?? throw new ArgumentNullException(nameof(networkStream));
                this.isBigEndian = isBigEndian;
            }

            public virtual void Flush()
            {
                ValidateDisposed();
                _networkStream.Flush();
            }

            public virtual Task FlushAsync()
            {
                ValidateDisposed();
                return _networkStream.FlushAsync();
            }

            protected async Task WriteAsync(byte[] buffer, int offset, int count)
            {
                ValidateDisposed();
                await _networkStream.WriteAsync(buffer, offset, count).ConfigureAwait(false);
            }

            protected async Task<int> ReadAsync(byte[] buffer, int offset, int count)
            {
                ValidateDisposed();
                var n = 0;
                while (n < count)
                {
                    int len = await _networkStream.ReadAsync(buffer, offset + n, count - n).ConfigureAwait(false);
                    if (len <= 0)
                        throw new IOException("Stream is closed.");

                    n += len;
                }
                return n;
            }
        }
    }
}
