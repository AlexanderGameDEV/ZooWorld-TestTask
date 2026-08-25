using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.Data
{
    public abstract class MovementConfig : ScriptableObject
    {
        public abstract MovementType Type { get; }
    }
}
