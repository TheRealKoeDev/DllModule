using KoeLib.DllModule.Configuration.Implementations;
using KoeLib.DllModule.Settings;
using KoeLib.DllModule.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDllModules(this IServiceCollection serviceCollection, IConfiguration configuration, Action<IDllModuleConfigurator> configurationAction)
            => AddDllModules(serviceCollection, configuration, "DllModuleSettings", configurationAction);

        public static IServiceCollection AddDllModules(this IServiceCollection serviceCollection, IConfiguration configuration, string appSettingsPath, Action<IDllModuleConfigurator> configurationAction)
        {
            Args.ThrowExceptionIfNull(serviceCollection, nameof(serviceCollection), configuration, nameof(configuration), configurationAction, nameof(configurationAction));

            ModuleRootDirectorySettings[] settings = configuration.GetSection(appSettingsPath )?.Get<ModuleRootDirectorySettings[]>();
            if (settings == null)
            {
                throw new ConfigurationErrorsException("ModuleSettings not found.");
            }

            configurationAction(new DllModuleConfigurator(configuration, serviceCollection, settings));
            return serviceCollection;
        }
    }
}
