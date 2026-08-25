namespace ZooWorld.Gameplay.Animals
{
    public sealed class HealthComponent : IHealth
    {
        private readonly int _max;
        private int _current;

        public HealthComponent(int max)
        {
            _max = max;
            _current = max;
        }

        public bool IsAlive => _current > 0;

        public void Damage(int amount)
        {
            _current -= amount;
        }

        public void Reset()
        {
            _current = _max;
        }
    }
}
