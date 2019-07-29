using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Settings
{
    public enum KindOfInstance : int
    {
        Singleton = 0,
        Scoped = 1,
        Transient = 2
    }
}
