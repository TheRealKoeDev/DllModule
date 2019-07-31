using KoeLib.DllModule.Configuration.Implementations;
using KoeLib.DllModule.Configuration.Implementations.Generators;
using KoeLib.DllModule.Settings;
using KoeLib.DllModule.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace KoeLib.DllModule.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddModularServices(this IServiceCollection services, IConfiguration configuration, IEnumerable<ModularServiceSettings> serviceSettings, Action<IServiceGenerator> generateServiceAction)
        {
            Args.ThrowExceptionIfNull(services, nameof(services), configuration, nameof(configuration), generateServiceAction, nameof(generateServiceAction));
            Args.ThrowExceptionIfNull(serviceSettings, nameof(serviceSettings));

           generateServiceAction(new ServiceGenerator(services, configuration, serviceSettings));
           return services;
        }

        public static IServiceCollection AddModularServices(this IServiceCollection services, IConfiguration configuration, string settingsPath, Action<IServiceGenerator> generateServiceAction)
        {
            Args.ThrowExceptionIfNull(settingsPath, nameof(settingsPath));

            ModularServiceSettings[] settings = string.IsNullOrWhiteSpace(settingsPath) ? configuration?.Get<ModularServiceSettings[]>() : configuration?.GetSection(settingsPath).Get<ModularServiceSettings[]>();
            
            return AddModularServices(services, configuration, settings, generateServiceAction);            
        }     
    }
}
