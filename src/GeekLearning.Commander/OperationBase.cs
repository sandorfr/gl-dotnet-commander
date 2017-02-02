using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace GeekLearning.Commander
{
    public abstract class OperationBase : IObservable<OperationState>
    {
        private ImmutableList<ObserverWeakReference> observers = ImmutableList<ObserverWeakReference>.Empty;

        protected void OnStateChanged(OperationState state)
        {
            foreach (var observer in observers)
            {
                observer.Invoke(state);
            }
        }

        private void DeleteObserverReference(ObserverWeakReference observerRef)
        {
            observers = observers.Remove(observerRef);
        }

        public IDisposable Subscribe(IObserver<OperationState> observer)
        {
            var observerRef = new ObserverWeakReference(observer, this.DeleteObserverReference);
            this.observers = this.observers.Add(observerRef);
            return observerRef;
        }

        private class ObserverWeakReference : IDisposable
        {
            WeakReference observerRef;
            WeakReference onDisposeRef;

            public ObserverWeakReference(IObserver<OperationState> observer, Action<ObserverWeakReference> onDispose)
            {
                observerRef = new WeakReference(observer);
                onDisposeRef = new WeakReference(onDispose);
            }


            public void Invoke(OperationState state)
            {
                var observer = observerRef.Target as IObserver<OperationState>;
                if (observer != null)
                {
                    observer.OnNext(state);
                }
                else
                {
                    this.Dispose();
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool isDisposing)
            {
                (onDisposeRef.Target as Action<ObserverWeakReference>)?.Invoke(this);
                if (isDisposing)
                {
                    GC.SuppressFinalize(this);
                }
            }

            ~ObserverWeakReference()
            {
                Dispose(false);
            }
        }
    }
}
