using KoeLib.ModularServices.Configuration;

namespace KoeLib.ModularServices.Settings
{
    public class ServiceModuleSettings
    {
        public bool Ignore { get; set; } = false;

        public string DllPath { get; set; }
        public PathType? PathType { get; set; }
        public string FullNameOfType { get; set; }

        
        public OnExceptionAction? OnConfigApplyExceptionAction { get; set; }
        public OnExceptionAction? OnConstructorExceptionAction { get; set; }
        public OnExceptionAction? OnInitializeExceptionAction { get; set; }
    }
}
