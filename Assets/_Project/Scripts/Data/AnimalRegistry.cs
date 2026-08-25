using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Data
{
    [CreateAssetMenu(menuName = "Zoo World/Animal Registry", fileName = "AnimalRegistry")]
    public sealed class AnimalRegistry : ScriptableObject
    {
        [SerializeField] private List<AnimalDefinition> _animals = new();

        public IReadOnlyList<AnimalDefinition> Animals => _animals;
    }
}
