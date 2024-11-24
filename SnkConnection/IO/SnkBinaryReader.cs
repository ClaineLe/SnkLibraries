using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public sealed class SnkBinaryReader : SnkBinaryStream
        {
            private byte[] buffer;

            public SnkBinaryReader(Socket socket, bool isBigEndian) : base(socket, isBigEndian)
            {
                buffer = new byte[socket.ReceiveBufferSize * 2];
            }

            public async Task<ushort> ReadUInt16()
            {
                await ReadBytes(buffer, 0, 2).ConfigureAwait(false);
                if (isBigEndian)
                    return (ushort)(buffer[1] | buffer[0] << 8);
                else
                    return (ushort)(buffer[0] | buffer[1] << 8);
            }

            public async Task<short> ReadInt16()
            {
                await ReadBytes(buffer, 0, 2).ConfigureAwait(false);
                if (isBigEndian)
                    return (short)(buffer[1] | buffer[0] << 8);
                else
                    return (short)(buffer[0] | buffer[1] << 8);
            }

            public async Task<uint> ReadUInt32()
            {
                await ReadBytes(buffer, 0, 4).ConfigureAwait(false);
                if (isBigEndian)
                    return (uint)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                else
                    return (uint)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
            }

            public async Task<int> ReadInt32()
            {
                await ReadBytes(buffer, 0, 4).ConfigureAwait(false);
                if (isBigEndian)
                    return (int)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                else
                    return (int)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
            }

            public async Task<ulong> ReadUInt64()
            {
                await ReadBytes(buffer, 0, 8).ConfigureAwait(false);
                if (isBigEndian)
                {
                    uint lo = (uint)(buffer[7] | buffer[6] << 8 | buffer[5] << 16 | buffer[4] << 24);
                    uint hi = (uint)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                    return ((ulong)hi) << 32 | lo;
                }
                else
                {
                    uint lo = (uint)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
                    uint hi = (uint)(buffer[4] | buffer[5] << 8 | buffer[6] << 16 | buffer[7] << 24);
                    return ((ulong)hi) << 32 | lo;
                }
            }

            public async Task<long> ReadInt64()
            {
                await ReadBytes(buffer, 0, 8).ConfigureAwait(false);
                if (isBigEndian)
                {
                    uint lo = (uint)(buffer[7] | buffer[6] << 8 | buffer[5] << 16 | buffer[4] << 24);
                    uint hi = (uint)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                    return ((long)hi) << 32 | lo;
                }
                else
                {
                    uint lo = (uint)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
                    uint hi = (uint)(buffer[4] | buffer[5] << 8 | buffer[6] << 16 | buffer[7] << 24);
                    return ((long)hi) << 32 | lo;
                }
            }

            public async Task<byte> ReadByte()
            {
                await ReadBytes(buffer, 0, 1).ConfigureAwait(false);
                return buffer[0];
            }

            private async Task<int> ReadBytes(byte[] buffer, int offset, int count)
            {
                ValidateDisposed();
                var n = 0;
                while (n < count)
                {
                    int len = await this.networkStream.ReadAsync(buffer, offset + n, count - n).ConfigureAwait(false);
                    if (len <= 0)
                        throw new IOException("Stream is closed.");

                    n += len;
                }
                return n;
            }
        }
    }
}
