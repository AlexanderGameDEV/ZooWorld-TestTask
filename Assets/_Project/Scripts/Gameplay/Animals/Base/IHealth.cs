namespace ZooWorld.Gameplay.Animals
{
    public interface IHealth
    {
        bool IsAlive { get; }
        void Damage(int amount);
        void Reset();
    }
}
