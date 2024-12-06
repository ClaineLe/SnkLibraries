using SnkConnection.IO;
using SnkLogging;
using System;

namespace SnkConnection
{
    public abstract class SnkDisposable : IDisposable 
    {
        private ISnkLogger _logger;
        protected ISnkLogger logger 
        {
            get 
            {
                if (_logger == null)
                    _logger = SnkLogHost.GetLog(this.GetType().Name);
                return _logger;
            }
        }

        private bool _disposed = false;
        protected bool isDisposed => _disposed;

        protected void ValidateDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SnkNetworkStream));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    OnDispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void OnDispose() { }
        public void Dispose() => Dispose(true);
        ~SnkDisposable() => Dispose(false);
    }
}
