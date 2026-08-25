using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using ZooWorld.Common;

namespace ZooWorld.UI
{
    public sealed class DeathCounterView : MonoBehaviour
    {
        [SerializeField] private DeathCounterEntryView _entryTemplate;
        [SerializeField] private Transform _container;

        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public void Construct(DeathCounterViewModel viewModel)
        {
            foreach (KeyValuePair<AnimalRole, IReadOnlyReactiveProperty<int>> entry in viewModel.Counters)
            {
                BindEntry(entry.Key, entry.Value);
            }
        }

        private void BindEntry(AnimalRole role, IReadOnlyReactiveProperty<int> counter)
        {
            DeathCounterEntryView view = Instantiate(_entryTemplate, _container);
            view.gameObject.SetActive(true);
            view.Initialize(role);
            counter.Subscribe(view.SetCount).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
