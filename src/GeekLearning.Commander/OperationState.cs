using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander
{
    public struct OperationState
    {
        public OperationState(bool isRunning, bool isEnabled)
        {
            IsRunning = isRunning;
            IsEnabled = isEnabled;
            CanRun = IsEnabled && !IsRunning;
        }

        public OperationState(IOperation operation)
        {
            IsRunning = operation.IsRunning;
            IsEnabled = operation.IsEnabled;
            CanRun = operation.CanRun;
        }

        public bool IsRunning { get; }
        public bool CanRun { get; }
        public bool IsEnabled { get; }
    }
}
