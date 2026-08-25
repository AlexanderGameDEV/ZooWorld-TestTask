using System.Collections.Generic;
using System.Linq;
using ZooWorld.Common;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Movement
{
    public sealed class MovementStrategyFactory
    {
        private readonly IReadOnlyDictionary<MovementType, IMovementStrategyBuilder> _builders;

        public MovementStrategyFactory(IEnumerable<IMovementStrategyBuilder> builders)
        {
            _builders = builders.ToDictionary(builder => builder.Type);
        }

        public IMovementStrategy Create(MovementConfig config)
        {
            return _builders[config.Type].Build(config);
        }
    }
}
