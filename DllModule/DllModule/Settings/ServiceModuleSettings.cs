using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    public class ServiceModuleSettings
    {
        public bool IsRequired { get; set; } = true;

        public string DllPath { get; set; }
        public PathType? PathType { get; set; }

        public string FullNameOfType { get; set; }
    }
}
