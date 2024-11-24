namespace SnkConnection
{
    namespace Channel
    {
        namespace Kcp
        {
            public interface ISnkKcpChannel<TMessage> : ISnkChannel<TMessage> where TMessage : ISnkMessage
            {
            }
        }
    }
}
