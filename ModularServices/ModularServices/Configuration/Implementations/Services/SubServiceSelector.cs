using KoeLib.ModularServices.Configuration.Dependencies;
using KoeLib.ModularServices.Tools;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace KoeLib.ModularServices.Configuration.Implementations.Services
{

    [DebuggerStepThrough]
    internal class SubServiceSelector<TService, TSubService> : ISubServiceSelector<TService, TSubService>
        where TService : class
        where TSubService : class
    {
        private readonly Func<TService, TSubService> _selector;

        public SubServiceSelector(Func<TService, TSubService> selector)
        {
            Args.ThrowExceptionIfNull(selector, nameof(selector));
            _selector = selector;

        }
        

        public TSubService Select(TService service)
            => _selector(service);
    }
}
