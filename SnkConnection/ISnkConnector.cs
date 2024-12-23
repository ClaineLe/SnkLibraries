using System;
using System.Threading.Tasks;

namespace SnkConnection
{
    public interface ISnkConnector : IDisposable
    {
        SnkConnectorName ConnectorName { get; }

        StateHandle StateHandler { get; set; }

        ReceiveHandle ReceiveHandler { get; set; }

        ExceptionHandle ExceptionHandler { get; set; }

        Task ConnectAsync(string hostname, int port, int timeoutMilliseconds);

        Task Disconnect();

        Task<ISnkResponse> Request(ISnkRequest request);

        void Notify(ISnkNotification response);
    }
}
