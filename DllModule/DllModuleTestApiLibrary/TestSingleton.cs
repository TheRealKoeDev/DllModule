using KoeLib.DllModule;
using System;

namespace DllModuleTestApiLibrary
{
    public class TestService
    {
        public DateTime CreatedAt = DateTime.Now;
        public DateTime InitializedAt;

        public TestSubService SubService = new TestSubService();
    }

    public class TestSubService
    {

    }
}
