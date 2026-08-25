using UnityEngine;

namespace ZooWorld.Data
{
    [CreateAssetMenu(menuName = "Zoo World/Game Settings", fileName = "GameSettings")]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private AnimalRegistry _animalRegistry;
        [SerializeField] private float _minSpawnInterval = 1f;
        [SerializeField] private float _maxSpawnInterval = 2f;
        [SerializeField] private int _maxAnimals = 30;
        [SerializeField] private int _prewarmPerAnimal = 5;
        [SerializeField] private float _spawnHeight = 1f;
        [SerializeField] private float _bounceForce = 3f;

        public AnimalRegistry AnimalRegistry => _animalRegistry;
        public float MinSpawnInterval => _minSpawnInterval;
        public float MaxSpawnInterval => _maxSpawnInterval;
        public int MaxAnimals => _maxAnimals;
        public int PrewarmPerAnimal => _prewarmPerAnimal;
        public float SpawnHeight => _spawnHeight;
        public float BounceForce => _bounceForce;
    }
}
