using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public interface IModularService<TService>
        where TService: class
    {
        TService Service { get; }
    }
}
