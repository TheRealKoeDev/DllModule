using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    public sealed class ModuleSettings
    {
        public string Type { get; set; }

        public KindOfInstance? KindOfInstance { get; set; }
        public string TypeOfInstance { get; set; }
        public string NamespaceOfInstance { get; set; }
    }
}
