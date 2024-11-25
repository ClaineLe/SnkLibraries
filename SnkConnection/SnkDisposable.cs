using SnkConnection.IO;
using System;

namespace SnkConnection
{
    public abstract class SnkDisposable : IDisposable 
    {

        private bool _disposed = false;

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
