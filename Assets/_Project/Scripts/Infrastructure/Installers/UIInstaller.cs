using UnityEngine;
using Zenject;
using ZooWorld.UI;

namespace ZooWorld.Infrastructure.Installers
{
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private TastyLabelView _tastyLabelPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AnimalStatisticsService>().AsSingle();
            Container.Bind<DeathCounterViewModel>().AsSingle();
            Container.Bind<TastyLabelPool>().AsSingle().WithArguments(_tastyLabelPrefab);
            Container.BindInterfacesAndSelfTo<TastyLabelService>().AsSingle();
        }
    }
}
