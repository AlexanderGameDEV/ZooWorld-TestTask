using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZooWorld.Common;
using ZooWorld.Data;
using ZooWorld.Gameplay.Animals;
using ZooWorld.Gameplay.FoodChain;

namespace ZooWorld.Gameplay.Pool
{
    public sealed class AnimalPoolLifecycleCoordinator : IDisposable
    {
        private readonly AnimalPoolService _pool;
        private readonly AnimalFactory _factory;
        private readonly IFixedTickRegistry _tickRegistry;
        private readonly AnimalCollisionRegistry _collisionRegistry;
        private readonly IDeathReporter _deathReporter;
        private readonly CancellationTokenSource _cancellation = new();
        private int _activeCount;

        public AnimalPoolLifecycleCoordinator(
            AnimalPoolService pool,
            AnimalFactory factory,
            IFixedTickRegistry tickRegistry,
            AnimalCollisionRegistry collisionRegistry,
            IDeathReporter deathReporter)
        {
            _pool = pool;
            _factory = factory;
            _tickRegistry = tickRegistry;
            _collisionRegistry = collisionRegistry;
            _deathReporter = deathReporter;
        }

        public int ActiveCount => _activeCount;

        public Animal Spawn(AnimalDefinition definition, Vector3 position, Vector3 direction)
        {
            AnimalView view = _pool.Get(definition);
            Animal animal = _factory.Create(view, definition, position, direction);
            animal.Died += OnDied;
            _tickRegistry.Register(animal);
            _collisionRegistry.Register(animal);
            _activeCount++;
            return animal;
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private void OnDied(Animal animal)
        {
            animal.Died -= OnDied;
            _tickRegistry.Unregister(animal);
            _collisionRegistry.Unregister(animal);
            _deathReporter.ReportDeath(animal.Role);
            animal.Deactivate();
            _activeCount--;
            DespawnAsync(animal).Forget();
        }

        private async UniTaskVoid DespawnAsync(Animal animal)
        {
            try
            {
                await animal.View.PlayDeathAsync(_cancellation.Token);
            }
            finally
            {
                _pool.Release(animal.Definition, animal.View);
            }
        }
    }
}
