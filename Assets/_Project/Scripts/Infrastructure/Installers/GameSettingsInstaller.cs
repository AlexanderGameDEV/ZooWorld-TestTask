using UnityEngine;
using Zenject;
using ZooWorld.Data;

namespace ZooWorld.Infrastructure.Installers
{
    [CreateAssetMenu(menuName = "Zoo World/Installers/Game Settings Installer", fileName = "GameSettingsInstaller")]
    public sealed class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameSettings);
        }
    }
}
