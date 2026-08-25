using UnityEngine;

namespace ZooWorld.Common
{
    public static class RandomDirection
    {
        public static Vector3 OnPlane()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        }

        public static Vector3 AwayFrom(Vector3 normal)
        {
            float angle = Random.Range(-75f, 75f);
            return Quaternion.AngleAxis(angle, Vector3.up) * normal;
        }
    }
}
