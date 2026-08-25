using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.Data
{
    [CreateAssetMenu(menuName = "Zoo World/Movement/Linear", fileName = "LinearMovementConfig")]
    public sealed class LinearMovementConfig : MovementConfig
    {
        [SerializeField] private float _speed = 3f;

        public override MovementType Type => MovementType.Linear;
        public float Speed => _speed;
    }
}
