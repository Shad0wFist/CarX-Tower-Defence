using UnityEngine;

[RequireComponent(typeof(NearestMonsterSelector), typeof(ProjectilePool))]
public class SimpleTower : Tower
{
    [SerializeField] private Vector3 shootOffset = new Vector3(0, 1.5f, 0);

    protected override void Shoot(Transform target)
    {
        Vector3 pos = transform.position + shootOffset;
        projectileProvider.SpawnProjectile(pos, Quaternion.identity, target);
    }
}