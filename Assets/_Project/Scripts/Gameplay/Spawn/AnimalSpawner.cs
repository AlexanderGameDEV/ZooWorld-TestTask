using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZooWorld.Common;
using ZooWorld.Data;
using ZooWorld.Gameplay.Pool;

namespace ZooWorld.Gameplay.Spawn
{
    public sealed class AnimalSpawner
    {
        private readonly AnimalPoolLifecycleCoordinator _coordinator;
        private readonly RandomSpawnIntervalStrategy _interval;
        private readonly WeightedAnimalSelector _selector;
        private readonly ScreenBounds _bounds;
        private readonly GameSettings _settings;

        public AnimalSpawner(
            AnimalPoolLifecycleCoordinator coordinator,
            RandomSpawnIntervalStrategy interval,
            WeightedAnimalSelector selector,
            ScreenBounds bounds,
            GameSettings settings)
        {
            _coordinator = coordinator;
            _interval = interval;
            _selector = selector;
            _bounds = bounds;
            _settings = settings;
        }

        public async UniTaskVoid RunAsync(CancellationToken token)
        {
            while (true)
            {
                float delay = _interval.Next();
                bool canceled = await UniTask
                    .Delay(TimeSpan.FromSeconds(delay), cancellationToken: token)
                    .SuppressCancellationThrow();

                if (canceled)
                {
                    return;
                }

                TrySpawn();
            }
        }

        private void TrySpawn()
        {
            if (_coordinator.ActiveCount >= _settings.MaxAnimals)
            {
                return;
            }

            AnimalDefinition definition = _selector.Next();
            Vector3 position = _bounds.RandomPoint(_settings.SpawnHeight);
            _coordinator.Spawn(definition, position, RandomDirection.OnPlane());
        }
    }
}
