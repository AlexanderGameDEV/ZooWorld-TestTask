using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZooWorld.Data;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.Pool
{
    public sealed class AnimalPoolService
    {
        private readonly Dictionary<AnimalDefinition, ObjectPool<AnimalView>> _pools = new();

        public AnimalView Get(AnimalDefinition definition)
        {
            return GetPool(definition).Get();
        }

        public void Release(AnimalDefinition definition, AnimalView view)
        {
            GetPool(definition).Release(view);
        }

        public void Prewarm(AnimalDefinition definition, int count)
        {
            ObjectPool<AnimalView> pool = GetPool(definition);
            var buffer = new AnimalView[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = pool.Get();
            }

            for (int i = 0; i < count; i++)
            {
                pool.Release(buffer[i]);
            }
        }

        private ObjectPool<AnimalView> GetPool(AnimalDefinition definition)
        {
            if (!_pools.TryGetValue(definition, out ObjectPool<AnimalView> pool))
            {
                pool = CreatePool(definition);
                _pools.Add(definition, pool);
            }

            return pool;
        }

        private ObjectPool<AnimalView> CreatePool(AnimalDefinition definition)
        {
            return new ObjectPool<AnimalView>(
                () => InstantiateView(definition),
                null,
                OnRelease,
                view => Object.Destroy(view.gameObject));
        }

        private static void OnRelease(AnimalView view)
        {
            view.ResetForPool();
            view.gameObject.SetActive(false);
        }

        private static AnimalView InstantiateView(AnimalDefinition definition)
        {
            var view = Object.Instantiate(definition.Prefab).GetComponent<AnimalView>();
            view.gameObject.SetActive(false);
            return view;
        }
    }
}
