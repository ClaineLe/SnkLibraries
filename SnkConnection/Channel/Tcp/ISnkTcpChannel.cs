namespace SnkConnection
{
    namespace Channel
    {
        namespace Tcp
        {
            public interface ISnkTcpChannel<TMessage> : ISnkChannel<TMessage> where TMessage : ISnkMessage
            {
            }
        }
    }
}
