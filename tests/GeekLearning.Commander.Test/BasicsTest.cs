namespace GeekLearning.Commander.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using GeekLearning.Commander;

    public class BasicsTest
    {
        [Fact]
        public async Task OperationState()
        {
            var operation = Operation.For(() => Task.Delay(30));

            Assert.True(operation.CanRun);
            Assert.True(operation.IsEnabled);
            Assert.False(operation.IsRunning);


            var runningOperation = operation.InvokeAsync();

            Assert.False(operation.CanRun);
            Assert.True(operation.IsEnabled);
            Assert.True(operation.IsRunning);

            await runningOperation;

            Assert.True(operation.CanRun);
            Assert.True(operation.IsEnabled);
            Assert.False(operation.IsRunning);
        }

        [Fact]
        public async Task OperationStateObserver()
        {
            var observer = new LoggerObserver();
            var operation = Operation.For(() => Task.Delay(30));
            operation.Subscribe(observer);
            await operation.InvokeAsync();

            Assert.Equal(2, observer.States.Count);

            Assert.Equal(true, observer.States[0].IsRunning);
            Assert.Equal(true, observer.States[0].IsEnabled);
            Assert.Equal(false, observer.States[0].CanRun);


            Assert.Equal(false, observer.States[1].IsRunning);
            Assert.Equal(true, observer.States[1].IsEnabled);
            Assert.Equal(true, observer.States[1].CanRun);
        }

        [Fact]
        public async Task OperationStateObserverMultiRun()
        {
            var observer = new LoggerObserver();
            var operation = Operation.For(() => Task.Delay(30));
            operation.Subscribe(observer);

            int runs = 10;

            for (int i = 0; i < runs; i++)
            {
                await operation.InvokeAsync();
            }

            Assert.Equal(runs * 2, observer.States.Count);


            for (int i = 0; i < runs; i++)
            {
                Assert.Equal(true, observer.States[i * 2].IsRunning);
                Assert.Equal(true, observer.States[i * 2].IsEnabled);
                Assert.Equal(false, observer.States[i * 2].CanRun);

                Assert.Equal(false, observer.States[i * 2 + 1].IsRunning);
                Assert.Equal(true, observer.States[i * 2 + 1].IsEnabled);
                Assert.Equal(true, observer.States[i * 2 + 1].CanRun);
            }
        }
    }
}
