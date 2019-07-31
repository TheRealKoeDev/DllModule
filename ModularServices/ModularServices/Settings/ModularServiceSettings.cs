
namespace KoeLib.ModularServices.Settings
{
    public class ModularServiceSettings
    {
        public string Typename { get; set; }
        public string Namespace { get; set; }

        public ServiceModuleSettings[] Modules { get; set; } = new ServiceModuleSettings[0];
        
    }
}
