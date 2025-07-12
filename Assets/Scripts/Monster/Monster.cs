using UnityEngine;

[RequireComponent(typeof(MonsterMovement), typeof(Health))]
public class Monster : MonoBehaviour, IMoveTargetProvider
{
    [SerializeField] private Transform moveTarget;
    public Transform Target => moveTarget;

    private MonsterMovement movement;
    private Health health;
    private ObjectPool<Monster> pool;

    private void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        health = GetComponent<Health>();

        movement.ReachedTarget += HandleReachedTarget;
        health.OnDied += HandleDeath;
    }

    public void Initialize(ObjectPool<Monster> sourcePool, Transform target)
    {
        pool = sourcePool;
        moveTarget = target;
    }

    private void HandleReachedTarget()
    {
        health.ApplyDamage(health.CurrentHP);
    }

    private void HandleDeath()
    {
        if (pool != null) pool.Release(this);
        else Destroy(gameObject);
    }
}
