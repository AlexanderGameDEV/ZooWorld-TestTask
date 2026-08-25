using UnityEngine;

namespace ZooWorld.Common
{
    public sealed class ScreenBounds
    {
        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _minZ;
        private readonly float _maxZ;
        private readonly Vector3 _center;

        public ScreenBounds(Camera camera)
        {
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            Vector3 origin = camera.transform.position;
            _minX = origin.x - halfWidth;
            _maxX = origin.x + halfWidth;
            _minZ = origin.z - halfHeight;
            _maxZ = origin.z + halfHeight;
            _center = new Vector3(origin.x, 0f, origin.z);
        }

        public bool Contains(Vector3 position)
        {
            return position.x >= _minX && position.x <= _maxX
                && position.z >= _minZ && position.z <= _maxZ;
        }

        public Vector3 RandomPoint(float y)
        {
            return new Vector3(Random.Range(_minX, _maxX), y, Random.Range(_minZ, _maxZ));
        }

        public Vector3 DirectionToCenter(Vector3 position)
        {
            Vector3 delta = _center - position;
            delta.y = 0f;
            return delta.normalized;
        }
    }
}
