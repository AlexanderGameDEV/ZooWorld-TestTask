using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.UI
{
    public sealed class TastyLabelService : ITastyLabelSpawner, IDisposable
    {
        private readonly TastyLabelPool _pool;
        private readonly CancellationTokenSource _cancellation = new();

        public TastyLabelService(TastyLabelPool pool)
        {
            _pool = pool;
        }

        public void Show(Vector3 worldPosition)
        {
            ShowAsync(worldPosition).Forget();
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid ShowAsync(Vector3 worldPosition)
        {
            TastyLabelView label = _pool.Get();
            try
            {
                await label.PlayAsync(worldPosition, _cancellation.Token).SuppressCancellationThrow();
            }
            finally
            {
                _pool.Release(label);
            }
        }
    }
}
