using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public class SnkNetworkWriter : SnkNetworkStream, ISnkNetworkWriter
        {
            public override int BufferSize { get; }

            protected byte[] buffer;

            public SnkNetworkWriter(Socket socket, bool isBigEndian) : base(socket, isBigEndian)
            {
                BufferSize = socket.SendBufferSize * 2;
                buffer = new byte[BufferSize];
            }

            public async Task WriteUInt16(UInt16 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[1] = (byte)value;
                    buffer[0] = (byte)(value >> 8);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                }
                await WriteAsync(buffer, 0, 2).ConfigureAwait(false);
            }
            public async Task WriteUInt32(UInt32 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[3] = (byte)value;
                    buffer[2] = (byte)(value >> 8);
                    buffer[1] = (byte)(value >> 16);
                    buffer[0] = (byte)(value >> 24);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                }
                await WriteAsync(buffer, 0, 4).ConfigureAwait(false);
            }
            public async Task WriteUInt64(UInt64 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[7] = (byte)value;
                    buffer[6] = (byte)(value >> 8);
                    buffer[5] = (byte)(value >> 16);
                    buffer[4] = (byte)(value >> 24);
                    buffer[3] = (byte)(value >> 32);
                    buffer[2] = (byte)(value >> 40);
                    buffer[1] = (byte)(value >> 48);
                    buffer[0] = (byte)(value >> 56);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                    buffer[4] = (byte)(value >> 32);
                    buffer[5] = (byte)(value >> 40);
                    buffer[6] = (byte)(value >> 48);
                    buffer[7] = (byte)(value >> 56);
                }
                await WriteAsync(buffer, 0, 8).ConfigureAwait(false);
            }
            public async Task WriteInt16(Int16 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[1] = (byte)value;
                    buffer[0] = (byte)(value >> 8);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                }
                await WriteAsync(buffer, 0, 2).ConfigureAwait(false);
            }
            public async Task WriteInt32(Int32 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[3] = (byte)value;
                    buffer[2] = (byte)(value >> 8);
                    buffer[1] = (byte)(value >> 16);
                    buffer[0] = (byte)(value >> 24);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                }
                await WriteAsync(buffer, 0, 4).ConfigureAwait(false);
            }
            public async Task WriteInt64(Int64 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    buffer[7] = (byte)value;
                    buffer[6] = (byte)(value >> 8);
                    buffer[5] = (byte)(value >> 16);
                    buffer[4] = (byte)(value >> 24);
                    buffer[3] = (byte)(value >> 32);
                    buffer[2] = (byte)(value >> 40);
                    buffer[1] = (byte)(value >> 48);
                    buffer[0] = (byte)(value >> 56);
                }
                else
                {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                    buffer[4] = (byte)(value >> 32);
                    buffer[5] = (byte)(value >> 40);
                    buffer[6] = (byte)(value >> 48);
                    buffer[7] = (byte)(value >> 56);
                }
                await WriteAsync(buffer, 0, 8).ConfigureAwait(false);
            }
            public async Task WriteByte(byte value)
            {
                ValidateDisposed();
                buffer[0] = value;
                await WriteAsync(buffer, 0, 1).ConfigureAwait(false);
            }
            public async Task WriteBytes(byte[] buffer, int offset, int count)
            {
                ValidateDisposed();
                await WriteAsync(buffer, offset, count).ConfigureAwait(false);
            }
        }
    }
}
