using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    public sealed class ModuleRootDirectorySettings
    {
        public string Directory { get; set; }

        public List<ModuleAssemblySettings> Assemblies { get; set; } = new List<ModuleAssemblySettings>();
    }
}
