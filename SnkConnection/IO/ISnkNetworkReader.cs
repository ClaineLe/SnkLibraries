using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public interface ISnkNetworkReader : ISnkNetworkStream
        {
            Task<ushort> ReadUInt16();
            Task<short> ReadInt16();
            Task<uint> ReadUInt32();
            Task<int> ReadInt32();
            Task<ulong> ReadUInt64();
            Task<long> ReadInt64();
            Task<byte> ReadByte();
            Task<int> ReadBytes(byte[] buffer, int offset, int count);
        }
    }
}
