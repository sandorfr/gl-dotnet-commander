using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander.Internal
{
    public class ExclusiveOperationWrapper<TOperation> : OperationBase, IOperation, IObserver<OperationState>
        where TOperation : IOperation
    {
        private TOperation innerOperation;
        private IOperation otherOperation;
        private bool otherIsRunning;

        public ExclusiveOperationWrapper(TOperation innerOperation, IOperation otherOperation)
        {
            this.innerOperation = innerOperation;
            this.otherOperation = otherOperation;
            this.innerOperation.Subscribe(this);
            this.otherOperation.Subscribe(this);
        }

        public bool CanRun => this.IsEnabled && !this.IsRunning && !this.IsOtherRunning;

        public bool IsEnabled => true;

        public bool IsOtherRunning => this.otherOperation.IsRunning;

        public bool IsRunning => this.innerOperation.IsRunning;

        protected TOperation InnerOperation => this.innerOperation;

        public Task InvokeAsync()
        {
           return this.InnerOperation.InvokeAsync();
        }

        void IObserver<OperationState>.OnCompleted()
        {
            
        }

        void IObserver<OperationState>.OnError(Exception error)
        {

        }

        void IObserver<OperationState>.OnNext(OperationState value)
        {
            var state = new OperationState(this);
            this.OnStateChanged(state);
        }
    }

    public class CancellableExclusiveOperationWrapper : ExclusiveOperationWrapper<ICancellableOperation>, ICancellableOperation
    {
        public CancellableExclusiveOperationWrapper(ICancellableOperation innerOperation, IOperation otherOperation)
            : base(innerOperation, otherOperation)
        {

        }

        public void Cancel()
        {
            this.InnerOperation.Cancel();
        }
    }
}
