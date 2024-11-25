using SnkConnection.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public abstract class SnkMessageEncode<TMessage> : SnkMessageCodec, ISnkMessageEncode<TMessage> where TMessage : ISnkMessage
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
            protected SnkByteBuffWriter buffWriter;
            public async Task Encode(TMessage message, ISnkNetworkWriter writer)
            {
                buffWriter = buffWriter ?? new SnkByteBuffWriter(writer.BufferSize, writer.IsBigEndian);
                await _semaphore.WaitAsync().ConfigureAwait(false);
                try 
                {
                    await EncodeProcess(message, writer).ConfigureAwait(false);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            protected abstract Task EncodeProcess(TMessage message, ISnkNetworkWriter writer);
        }
    }
}
