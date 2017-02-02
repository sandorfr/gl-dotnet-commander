using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeekLearning.Commander.Internal
{
    public class CommandOperationWrapper: OperationWrapperBase, ICommand, IObserver<OperationState>
    {
        private Action<Exception> onSwallowedException;

        public CommandOperationWrapper(IOperation innerOperation, Action<Exception> onSwallowedException) 
            : base(innerOperation)
        {
            this.onSwallowedException = onSwallowedException;
            base.Subscribe(this);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.InnerOperation.CanRun;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await this.InvokeAsync();
            }
            catch(Exception ex)
            {
                this.onSwallowedException?.Invoke(ex);
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(OperationState value)
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
