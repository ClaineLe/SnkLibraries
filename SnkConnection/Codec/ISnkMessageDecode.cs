using SnkConnection.IO;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public interface ISnkMessageDecode<TMessage> : ISnkMessageCodec where TMessage : ISnkMessage 
        {
            Task<TMessage> Decode(ISnkNetworkReader reader);
        }
    }
}
