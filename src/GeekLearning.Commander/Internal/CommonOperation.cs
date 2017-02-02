using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander.Internal
{
    public sealed class CommonOperation : OperationBase, IOperation
    {
        public CommonOperation(Func<Task> action)
        {
            this.operation = action;
            this.IsRunning = false;
        }

        private Func<Task> operation;

        public bool CanRun => this.IsEnabled && !this.IsRunning;

        public bool IsEnabled => true;

        private volatile bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            private set { isRunning = value; }
        }


        public async Task InvokeAsync()
        {
            if (IsEnabled)
            {
                this.IsRunning = true;
                base.OnStateChanged(new OperationState(this));
                try
                {
                    await operation?.Invoke();
                }
                finally
                {
                    this.IsRunning = false;
                    base.OnStateChanged(new OperationState(this));
                }
            }
        }
    }
}
