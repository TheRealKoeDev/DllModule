using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public interface ISubServiceGenerator<TService>
    {
        ISubServiceGenerator<TService> AddSubService<TSubService>(Expression<Func<TService, TSubService>> subServiceSelector)
            where TSubService: class;
    }
}
