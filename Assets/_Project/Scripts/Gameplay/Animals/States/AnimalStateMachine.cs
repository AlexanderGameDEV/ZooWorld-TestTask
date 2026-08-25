namespace ZooWorld.Gameplay.Animals
{
    public sealed class AnimalStateMachine
    {
        private IAnimalState _current;

        public void Change(IAnimalState next)
        {
            _current?.Exit();
            _current = next;
            _current.Enter();
        }

        public void Tick(float deltaTime)
        {
            _current?.Tick(deltaTime);
        }
    }
}
