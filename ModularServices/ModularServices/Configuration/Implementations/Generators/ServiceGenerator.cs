
using KoeLib.ModularServices.Configuration.Dependencies;
using KoeLib.ModularServices.Configuration.Implementations.Services;
using KoeLib.ModularServices.Settings;
using KoeLib.ModularServices.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KoeLib.ModularServices.Configuration.Implementations.Generators
{

    [DebuggerStepThrough]
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
            if (!serviceType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }

            AddModularService(serviceType, ServiceLifetime.Singleton);
            return this;
        }

        public IServiceGenerator AddSingleton(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            if (!serviceType.IsClass && !serviceType.IsInterface)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!implementationType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException("Types dont match.");
            }

            AddModularService(serviceType, implementationType, ServiceLifetime.Singleton);
            return this;
        }

        public IServiceGenerator AddSingleton<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Singleton);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Singleton));
            return this;
        }

        public IServiceGenerator AddSingleton<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), typeof(TServiceImplementation), ServiceLifetime.Singleton);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Singleton));
            return this;
        }

        public IServiceGenerator AddScoped(Type serviceType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType));
            if (!serviceType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }

            AddModularService(serviceType, ServiceLifetime.Scoped);
            return this;
        }

        public IServiceGenerator AddScoped(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            if (!serviceType.IsClass && !serviceType.IsInterface)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!implementationType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException("Types dont match.");
            }

            AddModularService(serviceType, implementationType, ServiceLifetime.Scoped);
            return this;
        }


        public IServiceGenerator AddScoped<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Scoped);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Scoped));
            return this;
        }

        public IServiceGenerator AddScoped<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
           where TService : class
           where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), typeof(TServiceImplementation), ServiceLifetime.Scoped);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Singleton));
            return this; ;
        }

        public IServiceGenerator AddTransient(Type serviceType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType));
            if (!serviceType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }

            AddModularService(serviceType, ServiceLifetime.Scoped);
            return this;
        }

        public IServiceGenerator AddTransient(Type serviceType, Type implementationType)
        {
            Args.ThrowExceptionIfNull(serviceType, nameof(serviceType), implementationType, nameof(implementationType));
            if (!serviceType.IsClass && !serviceType.IsInterface)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!implementationType.IsClass)
            {
                throw new ArgumentException("Type is not a class.");
            }
            else if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException("Types dont match.");
            }

            AddModularService(serviceType, implementationType, ServiceLifetime.Transient);
            return this;
        }

        public IServiceGenerator AddTransient<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService(typeof(TService), ServiceLifetime.Transient);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Transient));
            return this;
        }

        public IServiceGenerator AddTransient<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService
        {
            AddModularService(typeof(TService), typeof(TServiceImplementation) ,ServiceLifetime.Transient);
            subServiceGeneratorAction?.Invoke(new SubServiceGenerator<TService>(_services, ServiceLifetime.Singleton));
            return this;
        }

        private IServiceGenerator AddModularService(Type service, ServiceLifetime lifetime)
        {
            IEnumerable<Type> moduleTypes = CreateServiceModuleTypes(service, typeof(IModule<>).MakeGenericType(service));

            object moduleBuilder = Activator.CreateInstance(typeof(ServiceModuleBuilder<>).MakeGenericType(service), new object[] { moduleTypes.ToArray() });
            _services.AddSingleton(typeof(IServiceModuleBuilder<>).MakeGenericType(service), moduleBuilder);

            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    _services.AddScoped(service);
                    _services.AddScoped(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Singleton:
                    _services.AddSingleton(service);
                    _services.AddSingleton(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient(service);
                    _services.AddTransient(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                
            }

            return this;
        }
                

        private IServiceGenerator AddModularService(Type service, Type implementationType, ServiceLifetime lifetime)
        {
            IEnumerable<Type> moduleTypes = CreateServiceModuleTypes(service, typeof(IModule<>).MakeGenericType(service));

            object moduleBuilder = Activator.CreateInstance(typeof(ServiceModuleBuilder<>).MakeGenericType(service), new object[] { moduleTypes.ToArray() });
            _services.AddSingleton(typeof(IServiceModuleBuilder<>).MakeGenericType(service), moduleBuilder);

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    _services.AddSingleton(service, implementationType);
                    _services.AddSingleton(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient(service, implementationType);
                    _services.AddTransient(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
                case ServiceLifetime.Scoped:
                    _services.AddScoped(service, implementationType);
                    _services.AddScoped(typeof(IModularService<>).MakeGenericType(service), typeof(ModularService<>).MakeGenericType(service));
                    break;
            }

            return this;
        }

        private IEnumerable<Type> CreateServiceModuleTypes(Type serviceType, Type moduleType)
        {
            ModularServiceSettings settings = _settings.FirstOrDefault(sett => sett.Typename == serviceType.Name && sett.Namespace == serviceType.Namespace);
            if (settings == null)
            {
                throw new ConfigurationErrorsException("Modular service settings not found.");
            }

            HashSet<Type> moduleTypes = new HashSet<Type>();
            string appRootPath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            foreach (ServiceModuleSettings module in settings.Modules)
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
