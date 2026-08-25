using ZooWorld.Common;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Movement
{
    public interface IMovementStrategyBuilder
    {
        MovementType Type { get; }
        IMovementStrategy Build(MovementConfig config);
    }
}
