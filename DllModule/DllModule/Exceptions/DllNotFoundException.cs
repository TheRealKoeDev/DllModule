using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Exceptions
{
    public class DllNotFoundException : Exception
    {
        public DllNotFoundException(string dllName): base(dllName + " could not be found.")
        {

        }
    }
}
