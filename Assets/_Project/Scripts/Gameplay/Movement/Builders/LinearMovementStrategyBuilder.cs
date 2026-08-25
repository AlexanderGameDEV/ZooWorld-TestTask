using ZooWorld.Common;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Movement
{
    public sealed class LinearMovementStrategyBuilder : IMovementStrategyBuilder
    {
        public MovementType Type => MovementType.Linear;

        public IMovementStrategy Build(MovementConfig config)
        {
            var linear = (LinearMovementConfig)config;
            return new LinearMovementStrategy(linear.Speed);
        }
    }
}
