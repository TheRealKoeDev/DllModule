
using KoeLib.DllModule.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace KoeLib.DllModule.Configuration.Implementations
{
    public class ServiceGenerator
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

        public void AddSingletonModularService<TServiceType>()
            where TServiceType : class
            => AddModularService<TServiceType>(ServiceLifetime.Singleton);

        public void AddScopedModularService<TServiceType>()
            where TServiceType : class
            => AddModularService<TServiceType>(ServiceLifetime.Scoped);

        public void AddTransientModularService<TServiceType>()
            where TServiceType : class
            => AddModularService<TServiceType>(ServiceLifetime.Transient);

        private void AddModularService<TServiceType>(ServiceLifetime lifetime)
            where TServiceType : class
        {
            Type serviceType = typeof(TServiceType);

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

                moduleTypes.Add(LoadType<TServiceType>(ass, module.FullNameOfType));
            }

            _services.AddSingleton(typeof(IServiceModuleBuilder<TServiceType>), new ServiceModuleBuilder<TServiceType>(moduleTypes.ToArray()));

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    _services.AddSingleton<TServiceType>();
                    _services.AddSingleton<IModularService<TServiceType>, ModularService<TServiceType>>();
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient<TServiceType>();
                    _services.AddTransient<IModularService<TServiceType>, ModularService<TServiceType>>();
                    break;
                case ServiceLifetime.Scoped:
                    _services.AddScoped<TServiceType>();
                    _services.AddScoped<IModularService<TServiceType>, ModularService<TServiceType>>();
                    break;               
            }
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
