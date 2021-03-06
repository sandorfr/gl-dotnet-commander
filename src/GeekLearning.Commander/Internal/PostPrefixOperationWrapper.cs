﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander.Internal
{
    public class PostPrefixOperationWrapper : OperationBase, IOperation, IObserver<OperationState>
    {
        private IOperation innerOperation;

        public PostPrefixOperationWrapper(IOperation innerOperation, Func<Task> prefix, Func<Task> suffix)
        {
            this.innerOperation = innerOperation;
            this.innerOperation.Subscribe(this);
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public bool CanRun => this.IsEnabled && !this.innerOperation.IsRunning && !this.IsRunning;

        public bool IsEnabled => this.innerOperation.IsEnabled;

        private volatile bool isRunning;
        private Func<Task> prefix;
        private Func<Task> suffix;

        public bool IsRunning
        {
            get { return isRunning; }
            private set { isRunning = value; }
        }


        public async Task InvokeAsync()
        {
            if (IsEnabled && !IsRunning)
            {
                this.IsRunning = true;
                base.OnStateChanged(new OperationState(this));
                try
                {
                    if (this.prefix != null) 
                    {
                        await this.prefix.Invoke();
                    }
                    
                    await this.innerOperation.InvokeAsync();
                    
                    if (this.suffix != null)
                    {
                        await this.suffix.Invoke();
                    }
                }
                finally
                {
                    this.IsRunning = false;
                    base.OnStateChanged(new OperationState(this));
                }
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
            base.OnStateChanged(new OperationState(this));
        }
    }
}
