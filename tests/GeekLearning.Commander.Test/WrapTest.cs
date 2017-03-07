using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeekLearning.Commander.Test
{
    public class WrapTest
    {
        [Fact]
        public async Task TestWrapBasic()
        {
            var observer = new LoggerObserver();
            bool prefixRun = false;
            bool suffixRun = false;
            var operation = Operation.For(() => Task.Delay(30)).Wrap(async ()=> prefixRun = true, async ()=> suffixRun = true);
            operation.Subscribe(observer);
            await operation.InvokeAsync();

            Assert.True(prefixRun);
            Assert.True(suffixRun);
        }
    }
}
