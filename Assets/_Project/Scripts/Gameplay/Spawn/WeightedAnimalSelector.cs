using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Spawn
{
    public sealed class WeightedAnimalSelector
    {
        private readonly IReadOnlyList<AnimalDefinition> _animals;
        private readonly float _totalWeight;

        public WeightedAnimalSelector(GameSettings settings)
        {
            _animals = settings.AnimalRegistry.Animals;
            _totalWeight = SumWeights(_animals);
        }

        public AnimalDefinition Next()
        {
            float roll = Random.Range(0f, _totalWeight);
            for (int i = 0; i < _animals.Count; i++)
            {
                roll -= _animals[i].SpawnWeight;
                if (roll <= 0f)
                {
                    return _animals[i];
                }
            }

            return _animals[_animals.Count - 1];
        }

        private static float SumWeights(IReadOnlyList<AnimalDefinition> animals)
        {
            float total = 0f;
            for (int i = 0; i < animals.Count; i++)
            {
                total += animals[i].SpawnWeight;
            }

            return total;
        }
    }
}
