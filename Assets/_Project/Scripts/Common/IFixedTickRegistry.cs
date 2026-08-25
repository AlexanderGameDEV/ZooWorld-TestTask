namespace ZooWorld.Common
{
    public interface IFixedTickRegistry
    {
        void Register(IFixedTickTarget target);
        void Unregister(IFixedTickTarget target);
    }
}
