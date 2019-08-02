using KoeLib.ModularService.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    //[DebuggerStepThrough]
    internal class ServiceConfigurator<TService>: IServiceConfigurator<TService>
        where TService: class
    {
        private readonly IServiceCollection _services;

        internal ServiceConfigurator(IServiceCollection services)
        {
            _services = services;
        }

        IServiceConfigurator<TService> IServiceConfigurator<TService>.SetExceptionHandler<TExceptionHandler>()
        {
            _services.AddTransient<IModuleExceptionHandler<TService>, TExceptionHandler>();
            return this;
        }
    }
}
