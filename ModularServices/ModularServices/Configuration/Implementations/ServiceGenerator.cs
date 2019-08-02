
using KoeLib.ModularServices.Configuration;
using KoeLib.ModularServices.Configuration.Implementations.ExceptionHandlers;
using KoeLib.ModularServices.Configuration.Dependencies;
using KoeLib.ModularServices.Settings;
using KoeLib.ModularServices.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    //[DebuggerStepThrough]
    internal class ServiceGenerator: IServiceGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly IEnumerable<ModularServiceSettings> _settings;

        public ServiceGenerator(IServiceCollection services, IConfiguration configuration, IEnumerable<ModularServiceSettings> settings)
        {
            _services = services;
            _settings = settings;
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
            ModularServiceSettings settings = _settings.FirstOrDefault(sett => sett != null && sett.Typename == service.Name && sett.Namespace == service.Namespace);
            if (settings == null)
            {
                throw new InvalidDataException($"Modular service settings for {service.FullName} are missing or have a false format.");
            }

            IEnumerable<Type> moduleTypes = CreateServiceModuleTypes(typeof(IModule<>).MakeGenericType(service), settings);

            object moduleBuilder = Activator.CreateInstance(typeof(ServiceModuleContainer<>).MakeGenericType(service), new object[] { moduleTypes.ToArray() });
            _services.AddSingleton(typeof(IServiceModuleContainer<>).MakeGenericType(service), moduleBuilder);            

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

            switch (settings.OnModuleExceptionAction)
            {
                case OnModuleExceptionAction.Throw:
                    _services.AddTransient(typeof(IModuleExceptionHandler<>).MakeGenericType(service), typeof(ThorwModuleExceptionHandler<>).MakeGenericType(service));
                    break;
                case OnModuleExceptionAction.Continue:
                    _services.AddTransient(typeof(IModuleExceptionHandler<>).MakeGenericType(service), typeof(ContinueModuleExceptionHandler<>).MakeGenericType(service));
                    break;
                case OnModuleExceptionAction.Stop:
                    _services.AddTransient(typeof(IModuleExceptionHandler<>).MakeGenericType(service), typeof(StopModuleExceptionHandler<>).MakeGenericType(service));
                    break;
            }

            return this;
        }

        private IEnumerable<Type> CreateServiceModuleTypes(Type moduleType, ModularServiceSettings settings)
        {
            HashSet<Type> moduleTypes = new HashSet<Type>();
            string appRootPath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            foreach (ServiceModuleSettings module in settings.Modules ?? new ServiceModuleSettings[0])
            {
                if (module == null || module.Ignore)
                {
                    continue;
                }

                try
                {
                    string dllPath = module.PathType == PathType.Absolute ? module.DllPath : Path.Combine(appRootPath, module.DllPath);
                    moduleTypes.Add(TypeHelper.LoadModuleTypeFromAssembly(dllPath, module.FullNameOfType, moduleType));
                }
                catch
                {
                    if (module.IsRequired)
                    {
                        throw;
                    }
                }
            }
            return moduleTypes;
        }
    }
}
