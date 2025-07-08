using UnityEngine;
[RequireComponent(typeof(MonsterMovement))]
public class Monster : MonoBehaviour, IDamageable, IMoveTargetProvider
{
	[SerializeField] int maxHP = 30;
	int currentHP;

	[SerializeField] private Transform moveTarget;
	public Transform Target => moveTarget;

	private MonsterMovement movement;
	private ObjectPool<Monster> pool;

	void Awake()
	{
		movement = GetComponent<MonsterMovement>();
		movement.ReachedTarget += HandleReachedTarget;
	}

	private void OnEnable()
    {
        currentHP = maxHP;
    }

	public void Initialize(ObjectPool<Monster> sourcePool, Transform target)
    {
        pool = sourcePool;
        moveTarget = target;
    }

    public void ApplyDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
    }

	void HandleReachedTarget()
	{
		Die();
	}

	void Die()
	{
		if (pool != null)
            pool.Release(this);
        else
            Destroy(gameObject);
	}
}
