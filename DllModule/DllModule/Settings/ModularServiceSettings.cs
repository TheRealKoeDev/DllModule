using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    public class ModularServiceSettings
    {
        public string Typename { get; set; }
        public string Namespace { get; set; }

        public ServiceLifetime? Lifetime { get; set; }

        public ServiceModuleSettings[] Modules { get; set; } = new ServiceModuleSettings[0];
        
    }
}
