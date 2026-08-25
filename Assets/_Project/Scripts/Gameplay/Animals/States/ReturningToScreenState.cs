using ZooWorld.Common;

namespace ZooWorld.Gameplay.Animals
{
    public sealed class ReturningToScreenState : IAnimalState
    {
        private readonly Animal _animal;
        private readonly ScreenBounds _bounds;

        public ReturningToScreenState(Animal animal, ScreenBounds bounds)
        {
            _animal = animal;
            _bounds = bounds;
        }

        public void Enter()
        {
            _animal.Steer(_bounds.DirectionToCenter(_animal.Position));
        }

        public void Tick(float deltaTime)
        {
            if (_animal.IsInsideBounds())
            {
                _animal.EnterMoving();
                return;
            }

            _animal.MoveTick(deltaTime);
        }

        public void Exit() { }
    }
}
