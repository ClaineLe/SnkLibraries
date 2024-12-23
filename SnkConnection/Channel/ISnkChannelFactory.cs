using SnkConnection.Channel.Kcp;
using SnkConnection.Channel.Tcp;
using SnkConnection.Channel.Udp;
using SnkConnection.Codec;

namespace SnkConnection
{
    namespace Channel
    {
        public interface ISnkChannelFactory<TMessage> where TMessage : ISnkMessage
        {
            ISnkTcpChannel<TMessage> CreateTcpChannel(SnkConnectorName connectorName, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 1024 * 64, int sendBufferSize = 1024 * 64);
            ISnkUdpChannel<TMessage> CreateUdpChannel(SnkConnectorName connectorName, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 1024 * 64, int sendBufferSize = 1024 * 64);
            ISnkKcpChannel<TMessage> CreateKcpChannel(SnkConnectorName connectorName, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 1024 * 64, int sendBufferSize = 1024 * 64);
        }
    }
}
