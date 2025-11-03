using System.Collections.Generic;

namespace IOCContainer
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        IServiceProvider BuildServiceProvider();
    }
}