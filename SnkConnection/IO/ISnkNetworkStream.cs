using System;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace IO
    {
        public interface ISnkNetworkStream : IDisposable
        {
            int BufferSize { get; }
            bool IsBigEndian { get; }

            void Flush();

            Task FlushAsync();
        }
    }
}
