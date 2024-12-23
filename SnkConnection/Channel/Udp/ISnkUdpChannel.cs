namespace SnkConnection
{
    namespace Channel
    {
        namespace Udp
        {
            public interface ISnkUdpChannel<TMessage> : ISnkChannel<TMessage> where TMessage : ISnkMessage
            {
            }
        }
    }
}
