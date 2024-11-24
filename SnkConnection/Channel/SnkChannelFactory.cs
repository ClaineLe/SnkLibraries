using SnkConnection.Channel.Kcp;
using SnkConnection.Channel.Tcp;
using SnkConnection.Channel.Udp;
using SnkConnection.Codec;
using System;

namespace SnkConnection
{


    namespace Channel
    {
        public class SnkChannelFactory<TMessage> : ISnkChannelFactory<TMessage> where TMessage : ISnkMessage
        {
            public ISnkKcpChannel<TMessage> CreateKcpChannel(string tag, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 65536, int sendBufferSize = 65536)
                => throw new NotImplementedException();

            public ISnkTcpChannel<TMessage> CreateTcpChannel(string tag, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 65536, int sendBufferSize = 65536)
                => new SnkTcpChannel<TMessage>(codecFactory, bigEndian, noDelay, receiveBufferSize, sendBufferSize);

            public ISnkUdpChannel<TMessage> CreateUdpChannel(string tag, ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian = true, bool noDelay = true, int receiveBufferSize = 65536, int sendBufferSize = 65536)
                => throw new NotImplementedException();
        }
    }
}
