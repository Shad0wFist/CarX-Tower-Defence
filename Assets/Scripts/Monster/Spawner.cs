using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour
{
    public static event Action<ObjectPool<Monster>> OnPoolCreated;
    [SerializeField] private Monster monsterPrefab = null;
    [SerializeField] private Transform moveTarget = null;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int initialPoolSize = 20;

    private ObjectPool<Monster> monsterPool;
    private Coroutine spawnRoutine;

    private void Awake()
    {
        monsterPool = new ObjectPool<Monster>(monsterPrefab, initialPoolSize);
    }

    private void OnEnable()
    {
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private void Start()
    {
        OnPoolCreated?.Invoke(monsterPool);
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster()
    {
        Monster m = monsterPool.Get();
        m.transform.position = transform.position;
        m.Initialize(monsterPool, moveTarget);
        var hb = m.GetComponentInChildren<HealthBarController>();
        var health = m.GetComponent<Health>();
        hb.Bind(health);
    }
}
