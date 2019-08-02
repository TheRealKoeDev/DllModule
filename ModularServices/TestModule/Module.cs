using TestLibrary;
using KoeLib.ModularServices;
using System;

namespace TestModule
{
    public class Module : IModule<ITestServiceInterface>
    {
        private int _test = 888;
        ITestServiceInterface _module;
        public Module()
        {
        }

        public void Initialize(ITestServiceInterface module)
        {
            throw new Exception();
            _module = module;
            _module.InitializedAt = DateTime.Now;
        }
    }
}
