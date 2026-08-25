using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using ZooWorld.Data;

namespace ZooWorld.Gameplay.Animals
{
    public sealed class AnimalView : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Rigidbody _body;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private LayerMask _obstacleLayers;
        [SerializeField] private float _spawnScaleDuration = 0.2f;
        [SerializeField] private float _deathScaleDuration = 0.2f;

        private MaterialPropertyBlock _propertyBlock;
        private Vector3 _baseScale;
        private Tween _scaleTween;

        public event Action<Vector3> ObstacleHit;
        public event Action<Rigidbody> AnimalHit;

        public Rigidbody Body => _body;

        private void Awake()
        {
            _baseScale = transform.localScale;
        }

        private void OnDestroy()
        {
            KillScaleTween();
        }

        public void PlaceAt(Vector3 position)
        {
            KillScaleTween();
            transform.position = position;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            _body.position = position;
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _scaleTween = transform.DOScale(_baseScale, _spawnScaleDuration);
        }

        public void ApplyAppearance(AnimalDefinition definition)
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            _propertyBlock.SetColor(BaseColorId, definition.Color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public async UniTask PlayDeathAsync(CancellationToken token)
        {
            KillScaleTween();
            _scaleTween = transform.DOScale(Vector3.zero, _deathScaleDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(_deathScaleDuration), cancellationToken: token)
                .SuppressCancellationThrow();
        }

        public void ResetForPool()
        {
            KillScaleTween();
            transform.localScale = _baseScale;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ReportObstacle(collision);
            ReportAnimal(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            ReportObstacle(collision);
        }

        private void ReportObstacle(Collision collision)
        {
            if (!IsObstacle(collision.gameObject.layer))
            {
                return;
            }

            Vector3 away = AwayDirection(collision);
            if (away.sqrMagnitude > Mathf.Epsilon)
            {
                ObstacleHit?.Invoke(away);
            }
        }

        private void ReportAnimal(Collision collision)
        {
            if (collision.rigidbody != null)
            {
                AnimalHit?.Invoke(collision.rigidbody);
            }
        }

        private Vector3 AwayDirection(Collision collision)
        {
            int count = collision.contactCount;
            if (count == 0)
            {
                return Vector3.zero;
            }

            Vector3 contactPoint = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                contactPoint += collision.GetContact(i).point;
            }

            Vector3 away = _body.position - contactPoint / count;
            away.y = 0f;
            return away.normalized;
        }

        private bool IsObstacle(int layer)
        {
            return (_obstacleLayers.value & (1 << layer)) != 0;
        }

        private void KillScaleTween()
        {
            _scaleTween?.Kill();
            _scaleTween = null;
        }
    }
}
