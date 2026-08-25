using ZooWorld.Common;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Movement
{
    public sealed class JumpMovementStrategyBuilder : IMovementStrategyBuilder
    {
        public MovementType Type => MovementType.Jump;

        public IMovementStrategy Build(MovementConfig config)
        {
            var jump = (JumpMovementConfig)config;
            return new JumpMovementStrategy(jump.JumpInterval, jump.JumpDistance, jump.JumpHeight);
        }
    }
}
