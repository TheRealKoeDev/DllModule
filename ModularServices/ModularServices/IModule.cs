
namespace KoeLib.ModularServices
{
    public interface IModule<TService>
        where TService: class
    {
        void Initialize(TService module);
    }
}
