using System;

namespace KoeLib.DllModule
{
    public interface IInitializable<TInstance>
    {
        void Initialize(TInstance module);
    }
}
