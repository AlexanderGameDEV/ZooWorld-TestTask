using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.Data
{
    [CreateAssetMenu(menuName = "Zoo World/Movement/Jump", fileName = "JumpMovementConfig")]
    public sealed class JumpMovementConfig : MovementConfig
    {
        [SerializeField] private float _jumpInterval = 1f;
        [SerializeField] private float _jumpDistance = 2f;
        [SerializeField] private float _jumpHeight = 1.5f;

        public override MovementType Type => MovementType.Jump;
        public float JumpInterval => _jumpInterval;
        public float JumpDistance => _jumpDistance;
        public float JumpHeight => _jumpHeight;
    }
}
