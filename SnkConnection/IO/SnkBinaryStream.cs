using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public abstract class SnkBinaryStream : SnkDisposable, IDisposable
        {
            protected NetworkStream networkStream;
            protected bool isBigEndian;

            public SnkBinaryStream(Socket socket, bool isBigEndian)
            {
                this.networkStream = new NetworkStream(socket);// networkStream ?? throw new ArgumentNullException(nameof(networkStream));
                this.isBigEndian = isBigEndian;
            }

            public virtual void Flush()
            {
                ValidateDisposed();
                networkStream.Flush();
            }

            public virtual Task FlushAsync()
            {
                ValidateDisposed();
                return networkStream.FlushAsync();
            }
        }
    }
}
