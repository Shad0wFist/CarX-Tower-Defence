using UnityEngine;
public class GuidedProjectile : Projectile
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private float maxSteeringForce = 30f;
	[SerializeField] private float findingRange = 20f;
	private Transform target;
	private ITargetSelector targetSelector;

	protected override void Awake()
	{
		base.Awake();
		targetSelector = NearestMonsterSelector.Instance;
	}

	public override void Initialize(params object[] args)
	{
		base.Initialize();
		target = targetSelector.SelectTarget(transform.position, findingRange);
	}

	protected override void Move()
	{
		// Find new target
		if (target == null || !target.gameObject.activeInHierarchy)
		{
			target = targetSelector.SelectTarget(transform.position, findingRange);
			if (target == null)
			{
				return;
			}
		}

		Vector3 desiredVelocity = (target.position - transform.position).normalized * speed;
		Vector3 steering = desiredVelocity - rb.linearVelocity;
		
		if (steering.magnitude > maxSteeringForce)
			steering = steering.normalized * maxSteeringForce;

    	rb.AddForce(steering, ForceMode.Acceleration);
	}
}