using UnityEngine;

namespace ZooWorld.Gameplay.Movement
{
    public sealed class LinearMovementStrategy : DirectedMovementStrategyBase
    {
        private readonly float _speed;

        public LinearMovementStrategy(float speed)
        {
            _speed = speed;
        }

        public override void Tick(Rigidbody body, float deltaTime)
        {
            Vector3 velocity = body.velocity;
            velocity.x = Direction.x * _speed;
            velocity.z = Direction.z * _speed;
            body.velocity = velocity;
        }
    }
}
