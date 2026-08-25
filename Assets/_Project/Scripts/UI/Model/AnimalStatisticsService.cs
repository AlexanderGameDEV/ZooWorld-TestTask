using System;
using System.Collections.Generic;
using UniRx;
using ZooWorld.Common;

namespace ZooWorld.UI
{
    public sealed class AnimalStatisticsService : IDeathReporter
    {
        private readonly Dictionary<AnimalRole, ReactiveProperty<int>> _counters = new();
        private readonly Dictionary<AnimalRole, IReadOnlyReactiveProperty<int>> _readonlyCounters = new();

        public AnimalStatisticsService()
        {
            foreach (AnimalRole role in Enum.GetValues(typeof(AnimalRole)))
            {
                var counter = new ReactiveProperty<int>(0);
                _counters[role] = counter;
                _readonlyCounters[role] = counter;
            }
        }

        public IReadOnlyDictionary<AnimalRole, IReadOnlyReactiveProperty<int>> Counters => _readonlyCounters;

        public void ReportDeath(AnimalRole role)
        {
            _counters[role].Value++;
        }
    }
}
