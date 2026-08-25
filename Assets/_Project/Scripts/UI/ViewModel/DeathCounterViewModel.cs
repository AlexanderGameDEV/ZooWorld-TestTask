using System.Collections.Generic;
using UniRx;
using ZooWorld.Common;

namespace ZooWorld.UI
{
    public sealed class DeathCounterViewModel
    {
        private readonly AnimalStatisticsService _statistics;

        public DeathCounterViewModel(AnimalStatisticsService statistics)
        {
            _statistics = statistics;
        }

        public IReadOnlyDictionary<AnimalRole, IReadOnlyReactiveProperty<int>> Counters => _statistics.Counters;
    }
}
