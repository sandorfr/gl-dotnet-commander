using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.Commander
{
    public interface ICancellableOperation: IOperation
    {
        void Cancel();
    }
}
