using KoeLib.DllModule.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace KoeLib.DllModule.Configuration.Implementations
{
    internal class DllModuleConfigurator : IDllModuleConfigurator
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _serviceCollection;
        private readonly ModuleRootDirectorySettings[] _directorySettings;

        public DllModuleConfigurator(IConfiguration configuration, IServiceCollection serviceCollection, ModuleRootDirectorySettings[] directorySettings)
        {
            _configuration = configuration;
            _directorySettings = directorySettings;
            _serviceCollection = serviceCollection;
        }

        public IDllModuleConfigurator AddModule<TExternModule, TInternModule>(TInternModule internModule)
            where TExternModule : class, IInitializable<TInternModule>
            where TInternModule: class
        {
            Type internalType = typeof(TInternModule);    

            foreach (ModuleRootDirectorySettings directorySettigs in _directorySettings)
            {
                foreach (ModuleAssemblySettings assemblysettings in directorySettigs.Assemblies ?? new List<ModuleAssemblySettings>())
                {
                    foreach (ModuleSettings moduleSettings in assemblysettings.Modules ?? new List<ModuleSettings>())
                    {
                        if (moduleSettings.TypeOfInstance == internalType.FullName)
                        {
                            new DllModuleBuilder<TExternModule, TInternModule>
                            {
                                AppPath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey),
                                Connector = internModule,
                                ServiceCollection = _serviceCollection,

                                DirectorySettings = directorySettigs,
                                AssemblySettings = assemblysettings,
                                ModuleSettings = moduleSettings

                            }.Build();
                        }
                    }
                }
            }
            return this;
        }

        public IDllModuleConfigurator AddModule<TExternModule, TInternModule>()
            where TExternModule : class, IInitializable<TInternModule>
            where TInternModule : class, new()
            => AddModule<TExternModule, TInternModule>(new TInternModule());
    }
}
