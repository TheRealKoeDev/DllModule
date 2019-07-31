using KoeLib.DllModule.Configuration.Dependencies;
using System;
using System.Linq.Expressions;

namespace KoeLib.DllModule.Configuration.Implementations.Services
{
    internal class SubServiceSelector<TService, TSubService> : ISubServiceSelector<TService, TSubService>
        where TService : class
        where TSubService : class
    {
        private readonly Func<TService, TSubService> _selector;

        public SubServiceSelector(Func<TService, TSubService> selector)
            => _selector = selector;

        public TSubService Select(TService service)
            => _selector(service);
    }
}
