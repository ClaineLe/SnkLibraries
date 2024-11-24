using System;

namespace SnkConnection
{
    namespace EventArguments
    {
        public class SnkEventPublisher
        {
            private object _owner;
            private EventHandler<ISnkConnectionEventArgs> _eventHandler;
            public SnkEventPublisher(object owner, EventHandler<ISnkConnectionEventArgs> eventHandler)
            {
                this._owner = owner;
                this._eventHandler = eventHandler;
            }

            public void Publish<TConnectionEventArgs>(TConnectionEventArgs eventArgument) where TConnectionEventArgs : ISnkConnectionEventArgs
            {
                _eventHandler?.Invoke(this._owner, eventArgument);
            }
        }
    }
}
