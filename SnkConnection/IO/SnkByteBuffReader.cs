using System;

namespace SnkConnection
{
    namespace IO
    {
        public class SnkByteBuffReader : SnkByteBuffer
        {
            public SnkByteBuffReader(int capacity, bool isBigEndian) : base(capacity, isBigEndian)
            {
            }

            public Int16 ReadInt16()
            {
                ValidateOutOfRange(2);
                Int16 result;
                if (isBigEndian)
                    result = (Int16)(buffer[position + 1] | buffer[position] << 8);
                else
                    result = (Int16)(buffer[position] | buffer[position + 1] << 8);
                position += 2;
                return result;
            }
            public Int32 ReadInt32()
            {
                ValidateOutOfRange(4);
                Int32 result;
                if (isBigEndian)
                    result = (Int32)(buffer[position + 3] | buffer[position + 2] << 8 | buffer[position + 1] << 16 | buffer[position] << 24);
                else
                    result = (Int32)(buffer[position] | buffer[position + 1] << 8 | buffer[position + 2] << 16 | buffer[position + 3] << 24);
                position += 4;
                return result;
            }
            public Int64 ReadInt64()
            {
                ValidateOutOfRange(8);
                Int64 result;
                if (isBigEndian)
                    result = (Int64)(buffer[position + 7] | buffer[position + 6] << 8 | buffer[position + 5] << 16 | buffer[position + 4] << 24 | buffer[position + 3] << 32 | buffer[position + 2] << 40 | buffer[position + 1] << 48 | buffer[position] << 56);
                else
                    result = (Int64)(buffer[position] | buffer[position + 1] << 8 | buffer[position + 2] << 16 | buffer[position + 3] << 24 | buffer[position + 4] << 32 | buffer[position + 5] << 40 | buffer[position + 6] << 48 | buffer[position + 7] << 56);

                position += 8;
                return result;
            }
            public UInt16 ReadUInt16()
            {
                ValidateOutOfRange(2);
                var result = UInt16.MinValue;
                if (isBigEndian)
                    result = (UInt16)(buffer[position + 1] | buffer[position] << 8);
                else
                    result = (UInt16)(buffer[position] | buffer[position + 1] << 8);
                position += 2;
                return result;
            }
            public UInt32 ReadUInt32()
            {
                ValidateOutOfRange(4);
                UInt32 result;
                if (isBigEndian)
                    result = (UInt32)(buffer[position + 3] | buffer[position + 2] << 8 | buffer[position + 1] << 16 | buffer[position] << 24);
                else
                    result = (UInt32)(buffer[position] | buffer[position + 1] << 8 | buffer[position + 2] << 16 | buffer[position + 3] << 24);
                position += 4;
                return result;
            }
            public UInt64 ReadUInt64()
            {
                ValidateOutOfRange(8);
                UInt64 result;
                if (isBigEndian)
                    result = (UInt64)(buffer[position + 7] | buffer[position + 6] << 8 | buffer[position + 5] << 16 | buffer[position + 4] << 24 | buffer[position + 3] << 32 | buffer[position + 2] << 40 | buffer[position + 1] << 48 | buffer[position] << 56);
                else
                    result = (UInt64)(buffer[position] | buffer[position + 1] << 8 | buffer[position + 2] << 16 | buffer[position + 3] << 24 | buffer[position + 4] << 32 | buffer[position + 5] << 40 | buffer[position + 6] << 48 | buffer[position + 7] << 56);

                position += 8;
                return result;
            }
            public byte ReadByte()
            {
                ValidateOutOfRange(1);
                byte result = buffer[position];
                position += 1;
                return result;
            }
            public byte[] ReadBytes(int count)
            {
                ValidateOutOfRange(count);
                var result = new byte[count];
                Array.Copy(buffer, position, result, 0, count);
                position += count;
                return result;
            }
        }
    }
}
