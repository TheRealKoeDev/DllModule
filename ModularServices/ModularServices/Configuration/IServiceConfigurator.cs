using KoeLib.ModularServices.Configuration;

namespace KoeLib.ModularServices.Configuration
{
    public interface IServiceConfigurator<TService>
        where TService: class
    {
        IServiceConfigurator<TService> SetExceptionHandler<TExceptionHandler>()
            where TExceptionHandler : class, IModuleExceptionHandler<TService>;
    }
}
