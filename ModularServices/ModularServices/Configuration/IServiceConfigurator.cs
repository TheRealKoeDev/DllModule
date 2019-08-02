using KoeLib.ModularService.Configuration;

namespace KoeLib.ModularServices.Configuration
{
    public interface IServiceConfigurator<TService>
        where TService: class
    {
        IServiceConfigurator<TService> AddExceptionHandler<TExceptionHandler>()
            where TExceptionHandler : class, IModuleExceptionHandler<TService>;
    }
}
