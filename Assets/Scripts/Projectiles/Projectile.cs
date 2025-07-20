using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour, IPoolableProjectile
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float maxLifeTime = 5f;
    private float lifeTimer;

    protected Rigidbody rb;
    protected float spawnTime;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public event System.Action<GameObject> OnRelease;

    public virtual void Initialize(params object[] args)
    {
        lifeTimer = maxLifeTime;
        rb.isKinematic = false;
    }

    protected virtual void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Release();
        }
        else
            Move();
    }

    protected abstract void Move();

    protected virtual void OnTriggerEnter(Collider other)
    {
        var dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            dmg.ApplyDamage(damage);
            Release();
        }
    }

    public virtual void Release()
    {
        // Reset all forces
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        rb.isKinematic = true;
        OnRelease?.Invoke(gameObject);
    }
}