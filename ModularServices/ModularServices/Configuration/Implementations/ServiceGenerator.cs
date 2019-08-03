using KoeLib.ModularServices.Configuration.Implementations.ExceptionHandlers;
using KoeLib.ModularServices.Settings;
using KoeLib.ModularServices.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    [DebuggerStepThrough]
    internal class ServiceGenerator: IServiceGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly string _configPath;

        public ServiceGenerator(IServiceCollection services, IConfiguration configuration, string configPath)
        {
            _services = services;
            _configPath = configPath;
            _configuration = configuration;
        }

        public IServiceGenerator AddSingleton(Type serviceType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType));
            TypeHelper.ValidateService(serviceType);

            AddModularService(serviceType, ServiceLifetime.Singleton);
            return this;
        }

        public IServiceGenerator AddSingleton(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            TypeHelper.ValidateService(serviceType, implementationType);

            AddModularService(serviceType, ServiceLifetime.Singleton, implementationType);
            return this;
        }

        public IServiceGenerator AddSingleton<TService>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Singleton);
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this;
        }

        public IServiceGenerator AddSingleton<TService, TServiceImplementation>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), ServiceLifetime.Singleton, typeof(TServiceImplementation));
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this;
        }

        public IServiceGenerator AddScoped(Type serviceType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType));
            TypeHelper.ValidateService(serviceType);

            AddModularService(serviceType, ServiceLifetime.Scoped);
            return this;
        }

        public IServiceGenerator AddScoped(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            TypeHelper.ValidateService(serviceType, implementationType);

            AddModularService(serviceType, ServiceLifetime.Scoped, implementationType);
            return this;
        }

        public IServiceGenerator AddScoped<TService>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Scoped);
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this;
        }

        public IServiceGenerator AddScoped<TService, TServiceImplementation>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
           where TService : class
           where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), ServiceLifetime.Scoped, typeof(TServiceImplementation));
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this; ;
        }

        public IServiceGenerator AddTransient(Type serviceType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType));
            TypeHelper.ValidateService(serviceType);

            AddModularService(serviceType, ServiceLifetime.Scoped);
            return this;
        }

        public IServiceGenerator AddTransient(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            TypeHelper.ValidateService(serviceType, implementationType);

            AddModularService(serviceType, ServiceLifetime.Transient, implementationType);
            return this;
        }

        public IServiceGenerator AddTransient<TService>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Transient);
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this;
        }

        public IServiceGenerator AddTransient<TService, TServiceImplementation>(Action<IServiceConfigurator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), ServiceLifetime.Transient, typeof(TServiceImplementation));
            subServiceGeneratorAction?.Invoke(new ServiceConfigurator<TService>(_services));
            return this;
        }
        
        private IServiceGenerator AddModularService(Type service, ServiceLifetime lifetime, Type implementationType = null)
        {
            typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", new Type[] { typeof(IServiceCollection), typeof(IConfiguration) })
                .MakeGenericMethod(new Type[] { typeof(ModularServiceSettingsList<>).MakeGenericType(service) })
                .Invoke(null, new object[] { _services, _configuration.GetSection(_configPath + ':' + service.FullName)});

            _services.AddSingleton(typeof(IServiceModuleContainer<>).MakeGenericType(service), typeof(ServiceModuleContainer<>).MakeGenericType(service));

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    _ = implementationType == null ? _services.AddSingleton(service) : _services.AddSingleton(service, implementationType);
                    _services.AddSingleton(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Transient:
                    _ = implementationType == null ? _services.AddTransient(service) : _services.AddTransient(service, implementationType);
                    _services.AddTransient(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Scoped:
                    _ = implementationType == null ? _services.AddScoped(service) : _services.AddScoped(service, implementationType);
                    _services.AddScoped(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
            }

            _services.AddTransient(typeof(IServiceExceptionHandler<>).MakeGenericType(service), typeof(ContinueExceptionHandler<>).MakeGenericType(service));
            return this;
        }
    }
}
