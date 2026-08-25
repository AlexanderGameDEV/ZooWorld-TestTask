using UnityEngine;
using UnityEngine.Pool;

namespace ZooWorld.UI
{
    public sealed class TastyLabelPool
    {
        private readonly TastyLabelView _prefab;
        private readonly ObjectPool<TastyLabelView> _pool;

        public TastyLabelPool(TastyLabelView prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<TastyLabelView>(Create, null, OnRelease, OnDestroyLabel);
        }

        public TastyLabelView Get()
        {
            TastyLabelView label = _pool.Get();
            label.gameObject.SetActive(true);
            return label;
        }

        public void Release(TastyLabelView label)
        {
            _pool.Release(label);
        }

        private TastyLabelView Create()
        {
            TastyLabelView label = Object.Instantiate(_prefab);
            label.gameObject.SetActive(false);
            return label;
        }

        private static void OnRelease(TastyLabelView label)
        {
            label.ResetState();
            label.gameObject.SetActive(false);
        }

        private static void OnDestroyLabel(TastyLabelView label)
        {
            Object.Destroy(label.gameObject);
        }
    }
}
