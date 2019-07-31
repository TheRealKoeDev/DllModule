﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace KoeLib.DllModule.Configuration.Implementations
{
    internal class ModularServiceGenerator<TService>: ISubServiceGenerator<TService>
        where TService: class
    {
        private readonly IServiceCollection _services;
        private readonly ServiceLifetime _serviceLifetime;

        internal ModularServiceGenerator(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            _services = services;
            _serviceLifetime = serviceLifetime;
        }

        public ISubServiceGenerator<TService> AddSubService<TSubService>(Expression<Func<TService, TSubService>> subServiceSelector) 
            where TSubService : class
        {
            if (subServiceSelector == null)
            {
                throw new ArgumentException($"{nameof(Expression)} of {nameof(subServiceSelector)} is null.", nameof(subServiceSelector));
            }
            _services.AddSingleton<ISubServiceSelector<TService, TSubService>>(new SubServiceSelector<TService, TSubService>(subServiceSelector));

            switch (_serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    _services.AddScoped<IModularService<TSubService>, ModularSubService<TService, TSubService>>();
                    break;
                case ServiceLifetime.Singleton:
                    _services.AddSingleton<IModularService<TSubService>, ModularSubService<TService, TSubService>>();
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient<IModularService<TSubService>, ModularSubService<TService, TSubService>>();
                    break;
            }

            return this;
        }
    }
}
