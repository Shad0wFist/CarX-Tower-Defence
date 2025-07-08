using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour, IProjectileProvider
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<GameObject> pool;

    private void Awake()
    {
        pool = new Queue<GameObject>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(projectilePrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public void SpawnProjectile(Vector3 position, Quaternion rotation, Transform target = null, float initialForce = 0f)
    {
        GameObject proj;
        if (pool.Count > 0)
            proj = pool.Dequeue();
        else
            proj = Instantiate(projectilePrefab, transform);

        proj.transform.position = position;
        proj.transform.rotation = rotation;
        proj.SetActive(true);

        var guided = proj.GetComponent<GuidedProjectile>();
        if (guided != null)
            guided.Initialize(target, GetComponent<ITargetSelector>());

        var cannon = proj.GetComponent<CannonProjectile>();
        if (cannon != null)
            cannon.Initialize(initialForce);

        var releaser = proj.GetComponent<IPoolableProjectile>();
        if (releaser != null)
            releaser.OnRelease += ReturnToPool;
    }

    private void ReturnToPool(GameObject proj)
    {
        proj.SetActive(false);
        var poolable = proj.GetComponent<IPoolableProjectile>();
        if (poolable != null)
            poolable.OnRelease -= ReturnToPool;
        pool.Enqueue(proj);
    }
}