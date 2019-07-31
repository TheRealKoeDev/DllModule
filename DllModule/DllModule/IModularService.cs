
namespace KoeLib.DllModule
{
    public interface IModularService<TService>
        where TService: class
    {
        TService Service { get; }
    }
}
