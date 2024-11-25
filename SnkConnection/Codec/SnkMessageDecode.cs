using SnkConnection.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public abstract class SnkMessageDecode<TMessage> : SnkMessageCodec, ISnkMessageDecode<TMessage> where TMessage : ISnkMessage
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
            protected SnkByteBuffReader buffReader;

            public async Task<TMessage> Decode(ISnkNetworkReader reader)
            {
                buffReader = buffReader ?? new SnkByteBuffReader(reader.BufferSize, reader.IsBigEndian);
                await _semaphore.WaitAsync().ConfigureAwait(false);
                try
                {
                    return await DecodeProcess(reader).ConfigureAwait(false);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            protected abstract Task<TMessage> DecodeProcess(ISnkNetworkReader reader);
        }
    }
}
