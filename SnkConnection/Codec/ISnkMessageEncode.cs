using SnkConnection.IO;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public interface ISnkMessageEncode<TMessage> : ISnkMessageCodec where TMessage : ISnkMessage 
        {
            Task Encode(TMessage message, ISnkNetworkWriter writer);
        }
    }
}
