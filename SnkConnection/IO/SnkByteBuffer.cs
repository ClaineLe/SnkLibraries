using System;

namespace SnkConnection
{
    namespace IO
    {
        public abstract class SnkByteBuffer
        {
            protected bool isBigEndian;

            protected byte[] buffer;
            protected int position;

            public int Position => this.position;
            public byte[] Buffer => this.buffer;

            protected SnkByteBuffer(int capacity, bool isBigEndian)
            {
                this.buffer = new byte[capacity];
                this.isBigEndian = isBigEndian;
            }

            protected void ValidateOutOfRange(int count)
            {
                if (position + count > buffer.Length)
                    throw new IndexOutOfRangeException($"Attempted to read beyond the buffer size. position:{position}, count:{count}, buffer_size:{buffer.Length}");
            }

            public void Reset()
            {
                position = 0;
            }
        }
    }
}
