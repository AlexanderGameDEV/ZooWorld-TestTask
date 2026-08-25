using UnityEngine;

namespace ZooWorld.Gameplay.Movement
{
    public abstract class DirectedMovementStrategyBase : IMovementStrategy
    {
        protected Vector3 Direction { get; private set; }

        public Vector3 CurrentDirection => Direction;

        public void SetDirection(Vector3 direction)
        {
            Direction = direction.normalized;
        }

        public abstract void Tick(Rigidbody body, float deltaTime);
    }
}
