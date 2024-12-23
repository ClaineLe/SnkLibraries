namespace SnkConnection
{
    public interface ISnkMessage{ }
    public interface ISnkRequest : ISnkMessage { }
    public interface ISnkResponse : ISnkMessage { }
    public interface ISnkNotification : ISnkMessage { }
}
