using KoeLib.DllModule.Configuration.Implementations;
using KoeLib.DllModule.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KoeLib.DllModule.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddModularServices(this IServiceCollection services, IConfiguration configuration, string settingsPath, Action<IServiceGenerator> generateServiceAction)
        {
            ModularServiceSettings[]  settings = configuration.GetSection(settingsPath).Get<ModularServiceSettings[]>() ?? new ModularServiceSettings[0];
            generateServiceAction(new ServiceGenerator(services, configuration, settings));
            return services;
        }     
    }
}
