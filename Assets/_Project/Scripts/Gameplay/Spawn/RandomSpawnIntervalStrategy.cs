using UnityEngine;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Spawn
{
    public sealed class RandomSpawnIntervalStrategy
    {
        private readonly float _min;
        private readonly float _max;

        public RandomSpawnIntervalStrategy(GameSettings settings)
        {
            _min = settings.MinSpawnInterval;
            _max = settings.MaxSpawnInterval;
        }

        public float Next()
        {
            return Random.Range(_min, _max);
        }
    }
}
