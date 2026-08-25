using UnityEngine;

namespace ZooWorld.Gameplay.Movement
{
    public sealed class JumpMovementStrategy : DirectedMovementStrategyBase
    {
        private readonly float _jumpInterval;
        private readonly float _jumpDistance;
        private readonly float _jumpHeight;
        private float _timeSinceJump;

        public JumpMovementStrategy(float jumpInterval, float jumpDistance, float jumpHeight)
        {
            _jumpInterval = jumpInterval;
            _jumpDistance = jumpDistance;
            _jumpHeight = jumpHeight;
        }

        public override void Tick(Rigidbody body, float deltaTime)
        {
            _timeSinceJump += deltaTime;
            if (_timeSinceJump < _jumpInterval || !IsGrounded(body))
            {
                return;
            }

            _timeSinceJump = 0f;
            body.velocity = LaunchVelocity();
        }

        private static bool IsGrounded(Rigidbody body)
        {
            return Mathf.Abs(body.velocity.y) < 0.05f;
        }

        private Vector3 LaunchVelocity()
        {
            float gravity = -Physics.gravity.y;
            float verticalSpeed = Mathf.Sqrt(2f * gravity * _jumpHeight);
            float airTime = 2f * verticalSpeed / gravity;
            float horizontalSpeed = _jumpDistance / airTime;
            return Direction * horizontalSpeed + Vector3.up * verticalSpeed;
        }
    }
}
