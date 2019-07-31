namespace KoeLib.ModularServices.Settings
{
    public class ServiceModuleSettings
    {
        public bool IsRequired { get; set; } = true;
        public bool Ignore { get; set; } = false;

        public string DllPath { get; set; }
        public PathType? PathType { get; set; }

        public string FullNameOfType { get; set; }
    }
}
