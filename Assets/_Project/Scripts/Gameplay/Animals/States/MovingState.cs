namespace ZooWorld.Gameplay.Animals
{
    public sealed class MovingState : IAnimalState
    {
        private readonly Animal _animal;

        public MovingState(Animal animal)
        {
            _animal = animal;
        }

        public void Enter() { }

        public void Tick(float deltaTime)
        {
            if (!_animal.IsInsideBounds())
            {
                _animal.EnterReturning();
                return;
            }

            _animal.MoveTick(deltaTime);
        }

        public void Exit() { }
    }
}
