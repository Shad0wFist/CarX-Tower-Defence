using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour, IProjectileProvider
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform poolContainer;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<GameObject> pool;

    private float projectileMass = 0f;
    public float ProjectileMass => projectileMass;

    private void Awake()
    {
        if (poolContainer == null)
            poolContainer = new GameObject(projectilePrefab.name + "PoolContainer").transform;

        pool = new Queue<GameObject>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(projectilePrefab, poolContainer);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        projectileMass = projectilePrefab.GetComponent<Rigidbody>().mass;
    }

    public void SpawnProjectile(
        Vector3 position,
        Quaternion rotation,
        float initialForce = 0f,
        bool usePhysics = false)
    {
        GameObject proj;
        if (pool.Count > 0)
            proj = pool.Dequeue();
        else
            proj = Instantiate(projectilePrefab);

        proj.transform.SetParent(poolContainer, worldPositionStays: true);

        proj.transform.position = position;
        proj.transform.rotation = rotation;
        proj.SetActive(true);

        var guided = proj.GetComponent<GuidedProjectile>();
        if (guided != null)
            guided.Initialize();

        var cannon = proj.GetComponent<CannonProjectile>();
        if (cannon != null)
            cannon.Initialize(initialForce, usePhysics);

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