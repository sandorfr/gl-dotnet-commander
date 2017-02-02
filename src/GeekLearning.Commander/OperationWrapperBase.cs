using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander
{
    public abstract class OperationWrapperBase : IOperation
    {
        private IOperation innerOperation;

        public OperationWrapperBase(IOperation innerOperation)
        {
            this.innerOperation = innerOperation;
        }

        protected IOperation InnerOperation => innerOperation;

        public virtual bool CanRun
        {
            get
            {
                return this.innerOperation.CanRun;
            }
        }

        public virtual bool IsEnabled
        {
            get
            {
                return this.innerOperation.IsEnabled;
            }
        }

        public virtual bool IsRunning
        {
            get
            {
                return this.innerOperation.IsRunning;
            }
        }

        public virtual Task InvokeAsync()
        {
            return this.innerOperation.InvokeAsync();
        }

        public virtual IDisposable Subscribe(IObserver<OperationState> observer)
        {
            return this.innerOperation.Subscribe(observer);
        }
    }
}
