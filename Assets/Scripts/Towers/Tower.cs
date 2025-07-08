using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ITargetSelector), typeof(IProjectileProvider))]
public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected float shootInterval = 1f;
    [SerializeField] protected float range = 10f;

    private float lastShotTime = 0f;
    protected ITargetSelector targetSelector;
    protected IProjectileProvider projectileProvider;

    protected virtual void Awake()
    {
        targetSelector = GetComponent<ITargetSelector>();
        projectileProvider = GetComponent<IProjectileProvider>();
        lastShotTime = -shootInterval;
    }

    protected virtual void Update()
    {
        if (Time.time < lastShotTime + shootInterval)
            return;

        Transform target = targetSelector.SelectTarget(transform.position, range);
        if (target == null)
            return;

        Shoot(target);
        lastShotTime = Time.time;
    }

    protected abstract void Shoot(Transform target);
}