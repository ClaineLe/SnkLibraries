using System;

namespace SnkConnection
{
    namespace EventArguments
    {
        public class SnkExceptionEventArgs : SnkConnectionEventArgs
        {
            public string Message { get; }
            public Exception Exception { get; }
            public SnkExceptionEventArgs(string message, Exception exception)
            {
                this.Message = message;
                this.Exception = exception;
            }
        }
    }
}
