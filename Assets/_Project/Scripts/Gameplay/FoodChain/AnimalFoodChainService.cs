using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public sealed class AnimalFoodChainService
    {
        private readonly IReadOnlyList<ICollisionRule> _rules;
        private readonly AnimalCollisionRegistry _registry;

        public AnimalFoodChainService(IEnumerable<ICollisionRule> rules, AnimalCollisionRegistry registry)
        {
            _rules = new List<ICollisionRule>(rules);
            _registry = registry;
        }

        public void ResolveCollision(Animal source, Rigidbody otherBody)
        {
            if (!_registry.TryResolve(otherBody, out Animal other))
            {
                return;
            }

            if (source.Id >= other.Id)
            {
                return;
            }

            Apply(source, other);
        }

        private void Apply(Animal first, Animal second)
        {
            if (!first.IsAlive || !second.IsAlive)
            {
                return;
            }

            for (int i = 0; i < _rules.Count; i++)
            {
                if (!_rules[i].CanApply(first, second))
                {
                    continue;
                }

                _rules[i].Apply(first, second);
                return;
            }
        }
    }
}
