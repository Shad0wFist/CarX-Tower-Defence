using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IProjectileProvider))]
public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected float shootInterval = 1f;
    [SerializeField] protected float range = 10f;

    private float lastShotTime = 0f;
    protected ITargetSelector targetSelector;
    protected IProjectileProvider projectileProvider;
    protected Transform target;

    protected virtual void Awake()
    {
        targetSelector = NearestMonsterSelector.Instance;
        projectileProvider = GetComponent<IProjectileProvider>();
        lastShotTime = Time.time;
    }

    protected virtual void Update()
    {
        target = targetSelector.SelectTarget(transform.position, range);
        if (target == null)
            return;

        // If tower has IShootCondition, listen to it
        var condition = GetComponent<IShootCondition>();
        if (condition != null && !condition.CanShoot())
            return;
        
        if (Time.time < lastShotTime + shootInterval)
            return;

        Shoot();
        lastShotTime = Time.time;
    }

    protected abstract void Shoot();
}