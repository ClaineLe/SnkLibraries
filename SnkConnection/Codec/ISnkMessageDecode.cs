using SnkConnection.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public interface ISnkMessageDecode<TMessage> where TMessage : ISnkMessage 
        {
            Task<TMessage> Decode(SnkBinaryReader reader);
        }

        public abstract class SnkMessageDecode<TMessage> : ISnkMessageDecode<TMessage> where TMessage : ISnkMessage
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

            public async Task<TMessage> Decode(SnkBinaryReader reader)
            {
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

            protected abstract Task<TMessage> DecodeProcess(SnkBinaryReader reader);
        }
    }
}
