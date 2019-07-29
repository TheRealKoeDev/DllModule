using System;

namespace KoeLib.DllModule
{
    public interface IInitializable<TModule>
    {
        void Initialize(TModule module);
    }
}
