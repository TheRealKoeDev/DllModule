using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    internal class ModuleSettings
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string TypeOfInstance { get; set; }
    }
}
