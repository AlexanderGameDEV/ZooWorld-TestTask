using Zenject;
using ZooWorld.Gameplay.Spawn;

namespace ZooWorld.Infrastructure.Installers
{
    public sealed class SpawnInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RandomSpawnIntervalStrategy>().AsSingle();
            Container.Bind<WeightedAnimalSelector>().AsSingle();
            Container.Bind<AnimalSpawner>().AsSingle();
        }
    }
}
