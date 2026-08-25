using UnityEngine;

namespace ZooWorld.Gameplay.Movement
{
    public interface IMovementStrategy
    {
        Vector3 CurrentDirection { get; }
        void SetDirection(Vector3 direction);
        void Tick(Rigidbody body, float deltaTime);
    }
}
