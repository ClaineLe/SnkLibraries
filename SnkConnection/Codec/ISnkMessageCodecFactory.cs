namespace SnkConnection
{

    namespace Codec
    {
        public interface ISnkMessageCodecFactory<TMessage> where TMessage : ISnkMessage
        {
            ISnkMessageEncode<TMessage> CreateEncoder();

            ISnkMessageDecode<TMessage> CreateDecoder();
        }
    }
}
