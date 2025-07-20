using UnityEngine;

public interface IProjectileProvider
{
    float ProjectileMass { get; }
    void SpawnProjectile(
        Vector3 position,
        Quaternion rotation,
        Transform target = null,
        float initialForce = 0f,
        bool usePhysics = false);
}
