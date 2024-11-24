using SnkConnection.EventArguments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnkConnection
{
    public interface ISnkConnector : IDisposable
    {
        Task ConnectAsync(string hostname, int port, int timeoutMilliseconds, CancellationToken cancellationToken);

        Task Disconnect();

        Task Reconnect(CancellationToken cancellationToken);
    }

    public interface ISnkConnector<TRequest, TResponse, TNotification> : ISnkConnector
        where TRequest : class, ISnkRequest
        where TResponse : class, ISnkResponse
        where TNotification : class, ISnkNotification
    {
        EventHandler<TNotification> NotificationHandler { get; set; }

        EventHandler<ISnkConnectionEventArgs> ConnectionEventHandler { get; set; }

        Task<TResponse> Request(TRequest request);

        Task Notify(TResponse response);
    }
}
