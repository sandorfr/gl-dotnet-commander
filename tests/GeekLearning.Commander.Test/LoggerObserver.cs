using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander.Test
{
    public class LoggerObserver : IObserver<OperationState>
    {
        List<OperationState> states = new List<OperationState>();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(OperationState value)
        {
            this.states.Add(value);
        }

        public IList<OperationState> States => this.states;
    }
}
