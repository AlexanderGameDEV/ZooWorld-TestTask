using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using ZooWorld.Common;

namespace ZooWorld.Infrastructure
{
    public sealed class UpdateHandler : IInitializable, IDisposable, IFixedTickRegistry
    {
        private readonly List<IFixedTickTarget> _targets = new();
        private readonly CompositeDisposable _subscriptions = new();

        public void Initialize()
        {
            Observable.EveryFixedUpdate()
                .Subscribe(_ => Tick())
                .AddTo(_subscriptions);
        }

        public void Register(IFixedTickTarget target)
        {
            _targets.Add(target);
        }

        public void Unregister(IFixedTickTarget target)
        {
            _targets.Remove(target);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        private void Tick()
        {
            float deltaTime = Time.fixedDeltaTime;
            for (int i = _targets.Count - 1; i >= 0; i--)
            {
                _targets[i].FixedTick(deltaTime);
            }
        }
    }
}
