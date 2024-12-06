using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Channel
    {
        public interface ISnkChannel<TMessage> : IDisposable where TMessage : ISnkMessage 
        {
            Task OpenAsync(string host, int port, int timeoutMilliseconds, CancellationToken cancellationToken);
            Task WriteAsync(TMessage message);
            Task<TMessage> ReadAsync();
            Task CloseAsync();
        }
    }
}
