using UnityEngine;
public enum TargetingMode { Direct, Ballistic }

[RequireComponent(typeof(NearestMonsterSelector))]
[RequireComponent(typeof(ProjectilePool))]
[RequireComponent(typeof(CannonRotator))]
[RequireComponent(typeof(TargetVelocityTracker))]

public class CannonTower : Tower
{
	[SerializeField] private Transform shootPoint = null;
	[SerializeField] private float launchForce = 20f;
	[SerializeField] private TargetingMode targetingMode = TargetingMode.Direct;
	[SerializeField] private float aimUpdateInterval = 0.3f;
	private float lastAimUpdateTime = 0f;

	public float LaunchForce => launchForce;
	private float projectileMass = 0f;

	private CannonRotator rotator;
	private IAimStrategy aimStrategy;
	private TargetVelocityTracker velocityTracker;
    private TargetingMode lastMode;

	protected override void Awake()
	{
		base.Awake();
		rotator = GetComponent<CannonRotator>();
		aimStrategy = GetComponent<IAimStrategy>();
		velocityTracker = GetComponent<TargetVelocityTracker>();
	}

	void Start()
	{
		projectileMass = projectileProvider.ProjectileMass;
		SetupAimStrategy();
        lastMode = targetingMode;
	}

	protected override void Update()
	{
		base.Update();
		
		if (targetingMode != lastMode)
		{
			SetupAimStrategy();
			lastMode = targetingMode;
		}

		if (target == null)
		{
			rotator.ResetAim();
			return;
		}

		if (Time.time >= lastAimUpdateTime + aimUpdateInterval)
		{
			lastAimUpdateTime = Time.time;
			velocityTracker.Initialize(target);
		};

		Vector3 targetPos = target.position;
		Vector3 targetVel = velocityTracker.CurrentVelocity;

		aimStrategy.CalculateAimDirection
		(shootPoint.position,
		targetPos,
		targetVel,
		out Vector3 aimDir);

		rotator.SetAimDirection(aimDir);
		rotator.RotateCannon();
	}

	private void SetupAimStrategy()
    {
        float speed = launchForce / projectileMass;
        if (targetingMode == TargetingMode.Direct)
            aimStrategy = new DirectAimStrategy(speed);
        else
            aimStrategy = new BallisticAimStrategy(speed);
    }

	protected override void Shoot(Transform target)
	{
		bool usePhysics = targetingMode == TargetingMode.Ballistic;
		projectileProvider.SpawnProjectile(
			shootPoint.position,
			shootPoint.rotation,
			target,
			launchForce,
			usePhysics);
	}
}