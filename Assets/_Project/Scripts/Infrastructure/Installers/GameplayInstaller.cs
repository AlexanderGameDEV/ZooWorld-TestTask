using UnityEngine;
using Zenject;
using ZooWorld.Common;

namespace ZooWorld.Infrastructure.Installers
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;

        public override void InstallBindings()
        {
            Container.BindInstance(_camera);
            Container.Bind<ScreenBounds>().AsSingle();
            Container.BindInterfacesAndSelfTo<UpdateHandler>().AsSingle();
            Container.BindInterfacesTo<GameBootstrap>().AsSingle();
        }
    }
}
