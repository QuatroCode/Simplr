using Microsoft.Azure.WebJobs.Host;
using Ninject;

namespace Simplr.Core.WebJobs
{
    public class NinjectJobActivator : IJobActivator
    {
        public IKernel Kernel { get; private set; }

        public NinjectJobActivator(IKernel kernel)
        {
            Kernel = kernel;
        }

        public T CreateInstance<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
