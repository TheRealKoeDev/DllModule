using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KoeLib.ModularServices.Configuration
{
    public interface ISubServiceGenerator<TService>
    {
        ISubServiceGenerator<TService> AddSubService<TSubService>(Func<TService, TSubService> subServiceSelector)
            where TSubService: class;
    }
}
