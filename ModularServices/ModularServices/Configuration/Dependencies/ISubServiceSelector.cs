
namespace KoeLib.ModularServices.Configuration.Dependencies
{
    public interface ISubServiceSelector<TService, TSubService>
        where TService: class
        where TSubService: class
    {
        TSubService Select(TService service);
    }
}
