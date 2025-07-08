using UnityEngine;
public class GuidedProjectile : Projectile
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private float maxSteeringForce = 30f;
	[SerializeField] private float findingRange = 20f;
	private Transform target;
	private ITargetSelector selector;

	// args: [0] Transform initialTarget, [1] ITargetSelector selector
	public override void Initialize(params object[] args)
	{
		base.Initialize();
        target = args.Length > 0 ? args[0] as Transform : null;
        selector = args.Length > 1 ? args[1] as ITargetSelector : null;
	}

	protected override void Move()
	{
		// Find new target
		if (target == null || !target.gameObject.activeInHierarchy)
		{
			target = selector.SelectTarget(transform.position, findingRange);
			if (target == null)
			{
				return;
			}
		}

		Vector3 desiredVelocity = (target.position - transform.position).normalized * speed;
		Vector3 steering = desiredVelocity - rb.velocity;
		
		if (steering.magnitude > maxSteeringForce)
			steering = steering.normalized * maxSteeringForce;

    	rb.AddForce(steering, ForceMode.Acceleration);
	}
}