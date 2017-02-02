using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekLearning.Commander.Internal
{
    public sealed class CancellableTokenSourceOperationWrapper: OperationWrapperBase, ICancellableOperation
    {
        private CancellationTokenSource source;
        private bool swallowTaskCancelledException;

        public CancellableTokenSourceOperationWrapper(IOperation innerOperation, CancellationTokenSource source, bool swallowTaskCancelledException)
            : base(innerOperation)
        {
            this.source = source;
            this.swallowTaskCancelledException = swallowTaskCancelledException;
        }

        public void Cancel()
        {
            this.source.Cancel();
        }

        public override async Task InvokeAsync()
        {
            try
            {
               await base.InvokeAsync();
            }
            catch(TaskCanceledException)
            {
                if (!swallowTaskCancelledException)
                {
                    throw;
                }
            }
        }
    }
}
