using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeekLearning.Commander
{
    public static class Operation
    {
        public static IOperation For(Func<Task> action)
        {
            return new Internal.CommonOperation(action);
        }

        public static ICancellableOperation AsCancellable(this IOperation operation, CancellationTokenSource source, bool swallowTaskCancelledException = false)
        {
            return new Internal.CancellableTokenSourceOperationWrapper(operation, source, swallowTaskCancelledException);
        }

        public static ICancellableOperation CancellableFor(Func<CancellationToken, Task> action, bool swallowTaskCancelledException = false)
        {
            var cts = new CancellationTokenSource();
            return new Internal.CommonOperation(() => action(cts.Token))
                .AsCancellable(cts, swallowTaskCancelledException: swallowTaskCancelledException);
        }

        public static IOperation Exlusive(this IOperation operation, IOperation otherOperation)
        {
            return new Internal.ExclusiveOperationWrapper<IOperation>(operation, otherOperation);
        }

        public static ICancellableOperation Exlusive(this ICancellableOperation operation, IOperation otherOperation)
        {
            return new Internal.CancellableExclusiveOperationWrapper(operation, otherOperation);
        }

        public static ICommand AsCommand(this IOperation operation, Action<Exception> onSwallowedException = null)
        {
            return new Internal.CommandOperationWrapper(operation, onSwallowedException);
        }

    }
}
