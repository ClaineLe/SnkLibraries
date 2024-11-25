using SnkConnection.Codec;
using SnkConnection.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    namespace Channel
    {
        public abstract class SnkChannel<TMessage> : SnkDisposable, ISnkChannel<TMessage> where TMessage : ISnkMessage
        {
            protected bool bigEndian;
            protected bool noDelay;
            protected int receiveBufferSize;
            protected int sendBufferSize;

            protected ISnkNetworkReader reader;
            protected ISnkNetworkWriter writer;

            protected ISnkMessageEncode<TMessage> encoder;
            protected ISnkMessageDecode<TMessage> decoder;

            public abstract Task OpenAsync(string host, int port, int timeoutMilliseconds, CancellationToken cancellationToken);

            public abstract Task CloseAsync();

            protected SnkChannel(ISnkMessageCodecFactory<TMessage> codecFactory, bool bigEndian, bool noDelay, int receiveBufferSize, int sendBufferSize)
            {
                if (codecFactory == null)
                    throw new ArgumentNullException(nameof(codecFactory));

                this.encoder = codecFactory.CreateEncoder();
                this.decoder = codecFactory.CreateDecoder();

                this.bigEndian = bigEndian;
                this.noDelay = noDelay;
                this.receiveBufferSize = receiveBufferSize;
                this.sendBufferSize = sendBufferSize;
            }

            public virtual async Task<TMessage> ReadAsync()
            {
                ValidateReader();
                return await decoder.Decode(reader).ConfigureAwait(false);
            }

            public virtual async Task WriteAsync(TMessage message)
            {
                ValidateWriter();
                await encoder.Encode(message, writer).ConfigureAwait(false);
            }

            protected void ValidateReader()
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
            }

            protected void ValidateWriter()
            {
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
            }
        }
    }
}
