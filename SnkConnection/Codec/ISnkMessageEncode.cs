using SnkConnection.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Codec
    {
        public interface ISnkMessageEncode<TMessage> where TMessage : ISnkMessage 
        {
            Task Encode(TMessage message, SnkBinaryWriter writer);
        }

        public abstract class SnkMessageEncode<TMessage> : ISnkMessageEncode<TMessage> where TMessage : ISnkMessage
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

            public async Task Encode(TMessage message, SnkBinaryWriter writer)
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                try 
                {
                    await EncodeProcess(message, writer).ConfigureAwait(false);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            protected abstract Task EncodeProcess(TMessage message, SnkBinaryWriter writer);
        }

        public class Message : ISnkRequest, ISnkNotification, ISnkResponse 
        {
            public ushort cmd;
            public ushort seq;
            public byte opt;
            public byte block;
            public byte max_block;
        }

        public class MessageEncode : SnkMessageEncode<Message>
        {
            protected override async Task EncodeProcess(Message message, SnkBinaryWriter writer)
            {
                await writer.WriteUInt16(message.cmd).ConfigureAwait(false);
                await writer.WriteUInt16(message.seq).ConfigureAwait(false);
                await writer.WriteByte(message.opt).ConfigureAwait(false);
                await writer.WriteByte(message.block).ConfigureAwait(false);
                await writer.WriteByte(message.max_block).ConfigureAwait(false);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        public class MessageDecode : SnkMessageDecode<Message>
        {
            protected override Task<Message> DecodeProcess(SnkBinaryReader reader)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
