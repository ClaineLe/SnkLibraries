using System.Text;
using SnkLogging;

namespace SnkConnection
{
    namespace Codec
    {
        public abstract class SnkMessageCodec : SnkDisposable, ISnkMessageCodec
        {
            private StringBuilder _stringBuilder = new StringBuilder();

            protected void PrintMessage(ISnkMessage message, byte[] buffer, int position)
            {
                _stringBuilder.Clear();
                _stringBuilder.AppendLine($"[Net-{this.GetType().Name}]{message}");
                _stringBuilder.Append("[Bytes]");
                for (int i = 0; i < position; i++)
                    _stringBuilder.AppendFormat("{0:X2} ", buffer[i]);
                SnkLogHost.Default?.Info(_stringBuilder.ToString().Trim());
            }

            protected override void OnDispose()
            {
                if (this._stringBuilder != null)
                {
                    this._stringBuilder.Clear();
                    this._stringBuilder = null;
                }
                base.OnDispose();
            }
        }
    }
}
