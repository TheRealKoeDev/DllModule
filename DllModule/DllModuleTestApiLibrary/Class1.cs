using KoeLib.DllModule;
using System;

namespace DllModuleTestApiLibrary
{
    public class TestSingleton
    {
        public int Value = 0;
    }

    public interface ExternTestModule : IInitializable<TestSingleton>
    {

    }
}
