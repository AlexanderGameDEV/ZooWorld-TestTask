using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public sealed class AnimalCollisionRegistry
    {
        private readonly Dictionary<Rigidbody, Animal> _animals = new();

        public void Register(Animal animal)
        {
            _animals[animal.Body] = animal;
        }

        public void Unregister(Animal animal)
        {
            _animals.Remove(animal.Body);
        }

        public bool TryResolve(Rigidbody body, out Animal animal)
        {
            return _animals.TryGetValue(body, out animal);
        }
    }
}
