namespace EtAlii.Generators
{
    using System;
    using System.Collections.Generic;

    public sealed class CompositeDisposable : IDisposable
    {
        private readonly IEnumerable<IDisposable> _disposables;
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            _disposables = disposables;
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var disposable in _disposables)
                {
                    disposable.Dispose();
                }
            }
        }
        public void Dispose()
        {
            // Also dispose managed resources
            Dispose(true);
            // Don't call the finalizer (~Compo...)
            GC.SuppressFinalize(this);
        }

        ~CompositeDisposable()
        {
            // Only dispose unmanaged resources
            Dispose(false);
        }
    }
}
