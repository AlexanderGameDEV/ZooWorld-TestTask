using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;
using ZooWorld.Data;
using ZooWorld.Gameplay.Pool;
using ZooWorld.Gameplay.Spawn;

namespace ZooWorld.Infrastructure
{
    public sealed class GameBootstrap : IInitializable, IDisposable
    {
        private readonly GameSettings _settings;
        private readonly AnimalPoolService _pool;
        private readonly AnimalSpawner _spawner;
        private readonly CancellationTokenSource _cancellation = new();

        public GameBootstrap(GameSettings settings, AnimalPoolService pool, AnimalSpawner spawner)
        {
            _settings = settings;
            _pool = pool;
            _spawner = spawner;
        }

        public void Initialize()
        {
            Prewarm();
            _spawner.RunAsync(_cancellation.Token).Forget();
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private void Prewarm()
        {
            foreach (AnimalDefinition definition in _settings.AnimalRegistry.Animals)
            {
                _pool.Prewarm(definition, _settings.PrewarmPerAnimal);
            }
        }
    }
}
