using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander
{
    public interface IOperation: IObservable<OperationState>
    {
        bool IsRunning { get; }

        bool IsEnabled { get; }

        bool CanRun { get; }

        Task InvokeAsync();
    }
}
