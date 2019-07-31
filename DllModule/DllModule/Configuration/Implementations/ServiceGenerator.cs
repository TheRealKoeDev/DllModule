
using KoeLib.DllModule.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KoeLib.DllModule.Configuration.Implementations
{
    internal class ServiceGenerator: IServiceGenerator
    {
        private IConfiguration _configuration;
        private IServiceCollection _services;
        private IEnumerable<ModularServiceSettings> _settings;

        public ServiceGenerator(IServiceCollection services, IConfiguration configuration, IEnumerable<ModularServiceSettings> settings)
        {
            _services = services;
            _settings = settings;
            _configuration = configuration;
        }

        public IServiceGenerator AddSingletonModularService<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService<TService>(ServiceLifetime.Singleton);
            subServiceGeneratorAction?.Invoke(new ModularServiceGenerator<TService>(_services, ServiceLifetime.Singleton));
            return this;
        }

        public IServiceGenerator AddScopedModularService<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService<TService>(ServiceLifetime.Scoped);
            subServiceGeneratorAction?.Invoke(new ModularServiceGenerator<TService>(_services, ServiceLifetime.Scoped));
            return this;
        }

        public IServiceGenerator AddTransientModularService<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
        {
            AddModularService<TService>(ServiceLifetime.Transient);
            subServiceGeneratorAction?.Invoke(new ModularServiceGenerator<TService>(_services, ServiceLifetime.Transient));
            return this;
        }

        private IServiceGenerator AddModularService<TService>(ServiceLifetime lifetime)
            where TService : class
        {
            Type serviceType = typeof(TService);

            ModularServiceSettings settings = _settings.SingleOrDefault(sett => sett.Typename == serviceType.Name && sett.Namespace == serviceType.Namespace);
            if (settings == null)
            {
                throw new Exception();
            }

            HashSet<Type> moduleTypes = new HashSet<Type>();
            foreach (ServiceModuleSettings module in settings.Modules)
            {
                string dllPath = Path.IsPathRooted(module.DllPath) ? module.DllPath : _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + Path.DirectorySeparatorChar + module.DllPath;

                Assembly ass = LoadAssembly(dllPath);
                var x = ass.GetTypes();

                moduleTypes.Add(LoadType<TService>(ass, module.FullNameOfType));
            }

            _services.AddSingleton(typeof(IServiceModuleBuilder<TService>), new ServiceModuleBuilder<TService>(moduleTypes.ToArray()));

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    _services.AddSingleton<TService>();
                    _services.AddSingleton<IModularService<TService>, ModularService<TService>>();
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient<TService>();
                    _services.AddTransient<IModularService<TService>, ModularService<TService>>();
                    break;
                case ServiceLifetime.Scoped:
                    _services.AddScoped<TService>();
                    _services.AddScoped<IModularService<TService>, ModularService<TService>>();
                    break;               
            }

            return this;
        }

        private Type LoadType<TService>(Assembly assenmbly, string typename)
            where TService: class
        {
            Type type = assenmbly.GetType(typename);
            if (!type.GetInterfaces().Contains(typeof(IModule<TService>)))
            {
                throw new TypeLoadException($"The type does not inherit from {nameof(IModule<TService>)}");
            }

            return type;
        }

        private Assembly LoadAssembly(string dllPath)
        {
            if (!File.Exists(dllPath))
            {
                throw new FileNotFoundException("Dll of Modular service not found.");
            }
            else if (!dllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("File of Modular service is no Dll.");
            }

            return Assembly.LoadFrom(dllPath);
        }
    }
}
