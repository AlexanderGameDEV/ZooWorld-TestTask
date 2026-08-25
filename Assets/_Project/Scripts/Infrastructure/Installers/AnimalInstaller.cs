using Zenject;
using ZooWorld.Gameplay.Animals;
using ZooWorld.Gameplay.FoodChain;
using ZooWorld.Gameplay.Movement;
using ZooWorld.Gameplay.Pool;

namespace ZooWorld.Infrastructure.Installers
{
    public sealed class AnimalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMovement();
            BindFoodChain();
            BindAnimals();
            BindPool();
        }

        private void BindMovement()
        {
            Container.Bind<IMovementStrategyBuilder>().To<LinearMovementStrategyBuilder>().AsSingle();
            Container.Bind<IMovementStrategyBuilder>().To<JumpMovementStrategyBuilder>().AsSingle();
            Container.Bind<MovementStrategyFactory>().AsSingle();
        }

        private void BindFoodChain()
        {
            Container.Bind<ICollisionRule>().To<PredatorEatsPreyRule>().AsSingle();
            Container.Bind<ICollisionRule>().To<PredatorVsPredatorRule>().AsSingle();
            Container.Bind<ICollisionRule>().To<PreyBounceRule>().AsSingle();
            Container.Bind<AnimalCollisionRegistry>().AsSingle();
            Container.Bind<AnimalFoodChainService>().AsSingle();
        }

        private void BindAnimals()
        {
            Container.Bind<AnimalFactory>().AsSingle();
        }

        private void BindPool()
        {
            Container.Bind<AnimalPoolService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnimalPoolLifecycleCoordinator>().AsSingle();
        }
    }
}
