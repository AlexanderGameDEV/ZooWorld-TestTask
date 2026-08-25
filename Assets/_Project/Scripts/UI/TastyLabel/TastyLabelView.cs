using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ZooWorld.UI
{
    public sealed class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _riseDistance = 1.5f;
        [SerializeField] private float _duration = 1.1f;

        private Sequence _sequence;

        public async UniTask PlayAsync(Vector3 position, CancellationToken token)
        {
            Prepare(position);
            Animate(position);
            await UniTask.Delay(TimeSpan.FromSeconds(_duration), cancellationToken: token);
        }

        public void ResetState()
        {
            _sequence?.Kill();
            _sequence = null;
            SetAlpha(1f);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        private void Prepare(Vector3 position)
        {
            transform.position = position;
            SetAlpha(1f);
        }

        private void Animate(Vector3 position)
        {
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(position + Vector3.forward * _riseDistance, _duration));
            _sequence.Join(DOTween.To(() => _text.alpha, alpha => _text.alpha = alpha, 0f, _duration));
        }

        private void SetAlpha(float alpha)
        {
            _text.alpha = alpha;
        }
    }
}
