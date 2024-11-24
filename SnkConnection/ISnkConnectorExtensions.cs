using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    public static class ISnkConnectorExtensions
    {
        public static Task ConnectAsync(this ISnkConnector target, string hostname, int port, int timeoutMilliseconds)
            => target.ConnectAsync(hostname, port, timeoutMilliseconds, CancellationToken.None);

        public static Task Reconnect(this ISnkConnector target)
            => target.Reconnect(CancellationToken.None);
    }
}
