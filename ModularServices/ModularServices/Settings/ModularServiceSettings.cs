
using KoeLib.ModularServices.Configuration;

namespace KoeLib.ModularServices.Settings
{
    public class ModularServiceSettings
    {
        public string Typename { get; set; }
        public string Namespace { get; set; }
        public OnModuleExceptionAction OnModuleExceptionAction { get; set; } = OnModuleExceptionAction.Throw;

        public ServiceModuleSettings[] Modules { get; set; } = new ServiceModuleSettings[0];
        
    }
}
