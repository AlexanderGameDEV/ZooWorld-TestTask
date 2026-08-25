using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.Data
{
    [CreateAssetMenu(menuName = "Zoo World/Animal Definition", fileName = "AnimalDefinition")]
    public sealed class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private AnimalRole _role;
        [SerializeField] private int _hp = 1;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _spawnWeight = 1f;
        [SerializeField] private MovementConfig _movementConfig;

        public string Id => _id;
        public string DisplayName => _displayName;
        public AnimalRole Role => _role;
        public int Hp => _hp;
        public Color Color => _color;
        public GameObject Prefab => _prefab;
        public float SpawnWeight => _spawnWeight;
        public MovementConfig MovementConfig => _movementConfig;
    }
}
