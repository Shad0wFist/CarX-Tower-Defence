using UnityEngine;

public interface IProjectileProvider
{
    void SpawnProjectile(Vector3 position, Quaternion rotation, Transform target = null, float initialForce = 0f);
}
