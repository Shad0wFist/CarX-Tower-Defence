using UnityEngine;

public class NearestMonsterSelector : MonoBehaviour, ITargetSelector
{
    public static NearestMonsterSelector Instance { get; private set; }

    private ObjectPool<Monster> monsterPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        Spawner.OnPoolCreated += Initialize;
    }

    private void OnDisable()
    {
        Spawner.OnPoolCreated -= Initialize;
    }

    public void Initialize(ObjectPool<Monster> pool)
    {
        monsterPool = pool;
        
    }

    public Transform SelectTarget(Vector3 origin, float range)
    {
        if (monsterPool == null)
        {
            Debug.Log("monsterPool == null");
            return null;
        }

        float minDistSq = range * range;
        Monster nearest = null;

        foreach (var m in monsterPool.ActiveObjects)
        {
            float dsq = (m.transform.position - origin).sqrMagnitude;
            if (dsq < minDistSq)
            {
                minDistSq = dsq;
                nearest   = m;
            }
        }

        return nearest?.transform;
    }
}
