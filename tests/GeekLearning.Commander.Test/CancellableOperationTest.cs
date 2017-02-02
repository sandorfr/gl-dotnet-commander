using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeekLearning.Commander.Test
{
    public class CancellableOperationTest
    {
        [Fact]
        public async Task CancelOperation()
        {
            var cancellableOperation = Operation.CancellableFor((token) => Task.Delay(1000, token));

            var runningOperation = cancellableOperation.InvokeAsync();

            await Task.Delay(100);
            cancellableOperation.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => runningOperation);
        }

        public async Task CancelOperationWithSwallowedTaskCancelledException()
        {
            var cancellableOperation = Operation.CancellableFor(
                (token) => Task.Delay(1000, token), 
                swallowTaskCancelledException: true
                );

            var runningOperation = cancellableOperation.InvokeAsync();

            await Task.Delay(100);
            cancellableOperation.Cancel();

            await runningOperation;
        }

        [Fact]
        public async Task CancelOperationState()
        {
            var observer = new LoggerObserver();
            var cancellableOperation = Operation.CancellableFor((token) => Task.Delay(1000, token));
            cancellableOperation.Subscribe(observer);

            var runningOperation = cancellableOperation.InvokeAsync();

            await Task.Delay(100);
            cancellableOperation.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => runningOperation);

            Assert.Equal(2, observer.States.Count);

            Assert.Equal(true, observer.States[0].IsRunning);
            Assert.Equal(true, observer.States[0].IsEnabled);
            Assert.Equal(false, observer.States[0].CanRun);


            Assert.Equal(false, observer.States[1].IsRunning);
            Assert.Equal(true, observer.States[1].IsEnabled);
            Assert.Equal(true, observer.States[1].CanRun);

        }
    }
}
