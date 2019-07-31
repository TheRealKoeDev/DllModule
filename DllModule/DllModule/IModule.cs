using System;

namespace KoeLib.DllModule
{
    public interface IModule<TService>
        where TService: class
    {
        void Initialize(TService module);
    }
}
