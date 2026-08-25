using UnityEngine;

namespace ZooWorld.Gameplay.Animals
{
    public sealed class DeadState : IAnimalState
    {
        private readonly Rigidbody _body;

        public DeadState(Rigidbody body)
        {
            _body = body;
        }

        public void Enter()
        {
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
        }

        public void Tick(float deltaTime) { }

        public void Exit() { }
    }
}
