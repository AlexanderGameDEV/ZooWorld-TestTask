using UnityEngine;
using ZooWorld.Common;
using ZooWorld.Data;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public sealed class PreyBounceRule : ICollisionRule
    {
        private readonly float _force;

        public PreyBounceRule(GameSettings settings)
        {
            _force = settings.BounceForce;
        }

        public bool CanApply(Animal first, Animal second)
        {
            return first.Role == AnimalRole.Prey && second.Role == AnimalRole.Prey;
        }

        public void Apply(Animal first, Animal second)
        {
            Vector3 direction = SeparationDirection(first, second);
            first.Push(direction, _force);
            second.Push(-direction, _force);
        }

        private static Vector3 SeparationDirection(Animal first, Animal second)
        {
            Vector3 delta = first.Position - second.Position;
            delta.y = 0f;
            return delta.sqrMagnitude > Mathf.Epsilon ? delta.normalized : RandomDirection.OnPlane();
        }
    }
}
