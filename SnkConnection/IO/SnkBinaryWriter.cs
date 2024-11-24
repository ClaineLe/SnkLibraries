using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public sealed class SnkBinaryWriter : SnkBinaryStream
        {
            private byte[] _buffer;

            public SnkBinaryWriter(Socket socket, bool isBigEndian) : base(socket, isBigEndian)
            {
                _buffer = new byte[socket.SendBufferSize * 2];
            }

            public async Task WriteUInt16(UInt16 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[1] = (byte)value;
                    _buffer[0] = (byte)(value >> 8);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                }
                await networkStream.WriteAsync(_buffer, 0, 2).ConfigureAwait(false);
            }
            public async Task WriteUInt32(UInt32 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[3] = (byte)value;
                    _buffer[2] = (byte)(value >> 8);
                    _buffer[1] = (byte)(value >> 16);
                    _buffer[0] = (byte)(value >> 24);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                    _buffer[2] = (byte)(value >> 16);
                    _buffer[3] = (byte)(value >> 24);
                }
                await networkStream.WriteAsync(_buffer, 0, 4).ConfigureAwait(false);
            }
            public async Task WriteUInt64(UInt64 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[7] = (byte)value;
                    _buffer[6] = (byte)(value >> 8);
                    _buffer[5] = (byte)(value >> 16);
                    _buffer[4] = (byte)(value >> 24);
                    _buffer[3] = (byte)(value >> 32);
                    _buffer[2] = (byte)(value >> 40);
                    _buffer[1] = (byte)(value >> 48);
                    _buffer[0] = (byte)(value >> 56);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                    _buffer[2] = (byte)(value >> 16);
                    _buffer[3] = (byte)(value >> 24);
                    _buffer[4] = (byte)(value >> 32);
                    _buffer[5] = (byte)(value >> 40);
                    _buffer[6] = (byte)(value >> 48);
                    _buffer[7] = (byte)(value >> 56);
                }
                await networkStream.WriteAsync(_buffer, 0, 8).ConfigureAwait(false);
            }
            public async Task WriteInt16(Int16 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[1] = (byte)value;
                    _buffer[0] = (byte)(value >> 8);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                }
                await networkStream.WriteAsync(_buffer, 0, 2).ConfigureAwait(false);
            }
            public async Task WriteInt32(Int32 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[3] = (byte)value;
                    _buffer[2] = (byte)(value >> 8);
                    _buffer[1] = (byte)(value >> 16);
                    _buffer[0] = (byte)(value >> 24);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                    _buffer[2] = (byte)(value >> 16);
                    _buffer[3] = (byte)(value >> 24);
                }
                await networkStream.WriteAsync(_buffer, 0, 4).ConfigureAwait(false);
            }
            public async Task WriteInt64(Int64 value)
            {
                ValidateDisposed();
                if (isBigEndian)
                {
                    _buffer[7] = (byte)value;
                    _buffer[6] = (byte)(value >> 8);
                    _buffer[5] = (byte)(value >> 16);
                    _buffer[4] = (byte)(value >> 24);
                    _buffer[3] = (byte)(value >> 32);
                    _buffer[2] = (byte)(value >> 40);
                    _buffer[1] = (byte)(value >> 48);
                    _buffer[0] = (byte)(value >> 56);
                }
                else
                {
                    _buffer[0] = (byte)value;
                    _buffer[1] = (byte)(value >> 8);
                    _buffer[2] = (byte)(value >> 16);
                    _buffer[3] = (byte)(value >> 24);
                    _buffer[4] = (byte)(value >> 32);
                    _buffer[5] = (byte)(value >> 40);
                    _buffer[6] = (byte)(value >> 48);
                    _buffer[7] = (byte)(value >> 56);
                }
                await networkStream.WriteAsync(_buffer, 0, 8).ConfigureAwait(false);
            }

            public async Task WriteByte(byte value)
            {
                ValidateDisposed();
                _buffer[0] = value;
                await networkStream.WriteAsync(_buffer, 0, 1).ConfigureAwait(false);
            }

            public async Task WriteBytes(byte[] buffer, int offset, int count)
            {
                ValidateDisposed();
                await networkStream.WriteAsync(buffer, offset, count).ConfigureAwait(false);
            }
        }
    }
}
