using UnityEngine;

[RequireComponent(typeof(NearestMonsterSelector), typeof(ProjectilePool))]
public class CannonTower : Tower
{
    [SerializeField] private Transform shootPoint = null;
	[SerializeField] private float launchForce = 20f;

    protected override void Shoot(Transform target)
	{
		projectileProvider.SpawnProjectile(shootPoint.position, shootPoint.rotation, null, launchForce);
	}
}
