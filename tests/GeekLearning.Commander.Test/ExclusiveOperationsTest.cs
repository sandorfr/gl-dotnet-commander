using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeekLearning.Commander.Test
{
    public class ExclusiveOperationsTest
    {
        [Fact]
        public async Task ExclusiveOperations()
        {
            var operation1 = Operation.For(() => Task.Delay(500));

            var operation2 = Operation
                .For(() => Task.Delay(500))
                .Exlusive(operation1);

            var observer = new LoggerObserver();
            operation2.Subscribe(observer);

            var execution = operation1.InvokeAsync();

            Assert.True(operation2.IsEnabled);
            Assert.False(operation2.IsRunning);
            Assert.False(operation2.CanRun);

            await execution;

            await operation2.InvokeAsync();

            Assert.Equal(false, observer.States[0].IsRunning);
            Assert.Equal(true, observer.States[0].IsEnabled);
            Assert.Equal(false, observer.States[0].CanRun);


            Assert.Equal(false, observer.States[1].IsRunning);
            Assert.Equal(true, observer.States[1].IsEnabled);
            Assert.Equal(true, observer.States[1].CanRun);

            Assert.Equal(true, observer.States[2].IsRunning);
            Assert.Equal(true, observer.States[2].IsEnabled);
            Assert.Equal(false, observer.States[2].CanRun);


            Assert.Equal(false, observer.States[3].IsRunning);
            Assert.Equal(true, observer.States[3].IsEnabled);
            Assert.Equal(true, observer.States[3].CanRun);
        }
    }
}
