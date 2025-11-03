using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);

        TService GetService<TService>();
    }
}