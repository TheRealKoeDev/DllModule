using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration.Dependencies
{
    public interface ISubServiceSelector<TService, TSubService>
        where TService: class
        where TSubService: class
    {
        TSubService Select(TService service);
    }
}
