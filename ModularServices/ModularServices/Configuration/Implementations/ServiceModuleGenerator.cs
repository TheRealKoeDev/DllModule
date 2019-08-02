using KoeLib.ModularServices.Configuration.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    //[DebuggerStepThrough]
    internal class ServiceModuleContainer<TService> : IServiceModuleContainer<TService>
        where TService : class
    {
        public IEnumerable<Func<IModule<TService>>> Constructors { get; }

        public ServiceModuleContainer(Type[] types)
        {
            Queue<Func<IModule<TService>>> constructors = new Queue<Func<IModule<TService>>>();
            foreach (Type type in types ?? new Type[0])
            {
                constructors.Enqueue(Expression.Lambda<Func<IModule<TService>>>(Expression.New(type)).Compile());
            }
            Constructors = constructors;
        }
    }
}
