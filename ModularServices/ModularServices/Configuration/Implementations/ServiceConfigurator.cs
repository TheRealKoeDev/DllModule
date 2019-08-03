using KoeLib.ModularServices.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    [DebuggerStepThrough]
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
            _services.AddTransient<IServiceExceptionHandler<TService>, TExceptionHandler>();
            return this;
        }
    }
}
