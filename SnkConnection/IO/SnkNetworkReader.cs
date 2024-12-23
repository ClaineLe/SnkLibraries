using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public class SnkNetworkReader : SnkNetworkStream, ISnkNetworkReader
        {
            public override int BufferSize { get; }

            protected byte[] buffer;

            public SnkNetworkReader(Socket socket, bool isBigEndian) : base(socket, isBigEndian)
            {
                BufferSize = socket.ReceiveBufferSize * 2;
                buffer = new byte[BufferSize];
            }

            public async Task<ushort> ReadUInt16()
            {
                await ReadAsync(buffer, 0, 2).ConfigureAwait(false);
                if (isBigEndian)
                    return (ushort)(buffer[1] | buffer[0] << 8);
                else
                    return (ushort)(buffer[0] | buffer[1] << 8);
            }

            public async Task<short> ReadInt16()
            {
                await ReadAsync(buffer, 0, 2).ConfigureAwait(false);
                if (isBigEndian)
                    return (short)(buffer[1] | buffer[0] << 8);
                else
                    return (short)(buffer[0] | buffer[1] << 8);
            }

            public async Task<uint> ReadUInt32()
            {
                await ReadAsync(buffer, 0, 4).ConfigureAwait(false);
                if (isBigEndian)
                    return (uint)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                else
                    return (uint)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
            }

            public async Task<int> ReadInt32()
            {
                await ReadAsync(buffer, 0, 4).ConfigureAwait(false);
                if (isBigEndian)
                    return (int)(buffer[3] | buffer[2] << 8 | buffer[1] << 16 | buffer[0] << 24);
                else
                    return (int)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
            }

            public async Task<ulong> ReadUInt64()
            {
                await ReadAsync(buffer, 0, 8).ConfigureAwait(false);
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
                await ReadAsync(buffer, 0, 8).ConfigureAwait(false);
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
                await ReadAsync(buffer, 0, 1).ConfigureAwait(false);
                return buffer[0];
            }

            public async Task<int> ReadBytes(byte[] buffer, int offset, int count)
            {
                await ReadAsync(buffer, offset, count);
                return count;
            }
        }
    }
}
