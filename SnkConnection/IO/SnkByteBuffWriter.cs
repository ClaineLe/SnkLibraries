using System;

namespace SnkConnection
{
    namespace IO
    {
        public class SnkByteBuffWriter : SnkByteBuffer
        {
            public SnkByteBuffWriter(int capacity, bool isBigEndian) : base(capacity, isBigEndian)
            {
            }

            public void WriteInt16(Int16 value) 
            {
                ValidateOutOfRange(2);
                if (isBigEndian)
                {
                    buffer[position + 1] = (byte)value;
                    buffer[position + 0] = (byte)(value >> 8);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                }
                position += 2;
            }
            public void WriteInt32(Int32 value)
            {
                ValidateOutOfRange(4);
                if (isBigEndian)
                {
                    buffer[position + 3] = (byte)value;
                    buffer[position + 2] = (byte)(value >> 8);
                    buffer[position + 1] = (byte)(value >> 16);
                    buffer[position + 0] = (byte)(value >> 24);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                    buffer[position + 2] = (byte)(value >> 16);
                    buffer[position + 3] = (byte)(value >> 24);
                }
                position += 4;
            }
            public void WriteInt64(Int64 value)
            {
                ValidateOutOfRange(8);
                if (isBigEndian)
                {
                    buffer[position + 7] = (byte)value;
                    buffer[position + 6] = (byte)(value >> 8);
                    buffer[position + 5] = (byte)(value >> 16);
                    buffer[position + 4] = (byte)(value >> 24);
                    buffer[position + 3] = (byte)(value >> 32);
                    buffer[position + 2] = (byte)(value >> 40);
                    buffer[position + 1] = (byte)(value >> 48);
                    buffer[position + 0] = (byte)(value >> 56);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                    buffer[position + 2] = (byte)(value >> 16);
                    buffer[position + 3] = (byte)(value >> 24);
                    buffer[position + 4] = (byte)(value >> 32);
                    buffer[position + 5] = (byte)(value >> 40);
                    buffer[position + 6] = (byte)(value >> 48);
                    buffer[position + 7] = (byte)(value >> 56);
                }
                position += 8;
            }
            public void WriteUInt16(UInt16 value)
            {
                ValidateOutOfRange(2);
                if (isBigEndian)
                {
                    buffer[position + 1] = (byte)value;
                    buffer[position + 0] = (byte)(value >> 8);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                }
                position += 2;
            }
            public void WriteUInt32(UInt32 value)
            {
                ValidateOutOfRange(4);
                if (isBigEndian)
                {
                    buffer[position + 3] = (byte)value;
                    buffer[position + 2] = (byte)(value >> 8);
                    buffer[position + 1] = (byte)(value >> 16);
                    buffer[position + 0] = (byte)(value >> 24);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                    buffer[position + 2] = (byte)(value >> 16);
                    buffer[position + 3] = (byte)(value >> 24);
                }
                position += 4;
            }
            public void WriteUInt64(UInt64 value)
            {
                ValidateOutOfRange(8);
                if (isBigEndian)
                {
                    buffer[position + 7] = (byte)value;
                    buffer[position + 6] = (byte)(value >> 8);
                    buffer[position + 5] = (byte)(value >> 16);
                    buffer[position + 4] = (byte)(value >> 24);
                    buffer[position + 3] = (byte)(value >> 32);
                    buffer[position + 2] = (byte)(value >> 40);
                    buffer[position + 1] = (byte)(value >> 48);
                    buffer[position + 0] = (byte)(value >> 56);
                }
                else
                {
                    buffer[position + 0] = (byte)value;
                    buffer[position + 1] = (byte)(value >> 8);
                    buffer[position + 2] = (byte)(value >> 16);
                    buffer[position + 3] = (byte)(value >> 24);
                    buffer[position + 4] = (byte)(value >> 32);
                    buffer[position + 5] = (byte)(value >> 40);
                    buffer[position + 6] = (byte)(value >> 48);
                    buffer[position + 7] = (byte)(value >> 56);
                }
                position += 8;
            }
            public void WriteByte(byte value)
            {
                ValidateOutOfRange(1);
                buffer[position++] = value;
            }
            public void WriteBytes(byte[] values, int offset, int count) 
            {
                ValidateOutOfRange(count);
                Array.Copy(values, offset, buffer, position, count);
                position += count;
            }
        }
    }
}
