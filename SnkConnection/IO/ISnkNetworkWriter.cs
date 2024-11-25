using System.Threading.Tasks;
using System;

namespace SnkConnection
{
    namespace IO
    {
        public interface ISnkNetworkWriter : ISnkNetworkStream
        {
            Task WriteUInt16(UInt16 value);
            Task WriteUInt32(UInt32 value);
            Task WriteUInt64(UInt64 value);
            Task WriteInt16(Int16 value);
            Task WriteInt32(Int32 value);
            Task WriteInt64(Int64 value);
            Task WriteByte(byte value);
            Task WriteBytes(byte[] buffer, int offset, int count);
        }
    }
}
