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

    private void Awake()
    {
        lastShotTime = Time.time;
        projectileProvider = GetComponent<IProjectileProvider>();
    }
    protected virtual void Start()
    {
        lastShotTime = Time.time - shootInterval;
        targetSelector = NearestMonsterSelector.Instance;
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position;

        // 3 rings:
        DrawRings(center, Vector3.right, Vector3.up);    // XY
        DrawRings(center, Vector3.right, Vector3.forward); // XZ
        DrawRings(center, Vector3.forward, Vector3.up);  // YZ
    }

    private void DrawRings(Vector3 center, Vector3 axis1, Vector3 axis2, int segments = 64)
    {
        float angleStep = 2 * Mathf.PI / segments;
        Vector3 prevPoint = center + (axis1.normalized * range);
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            Vector3 newPoint = center
                + axis1.normalized * Mathf.Cos(angle) * range
                + axis2.normalized * Mathf.Sin(angle) * range;
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}