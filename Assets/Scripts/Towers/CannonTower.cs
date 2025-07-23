using UnityEngine;
public enum TargetingMode { Direct, Ballistic }

[RequireComponent(typeof(ProjectilePool))]
[RequireComponent(typeof(CannonRotator))]
[RequireComponent(typeof(TargetVelocityTracker))]

public class CannonTower : Tower
{
	[SerializeField] private Transform shootPoint = null;
	[SerializeField] private float launchForce = 20f;
	[SerializeField] private TargetingMode targetingMode = TargetingMode.Direct;

	public float LaunchForce => launchForce;
	private float projectileMass = 0f;

	private CannonRotator rotator;
	private IAimStrategy aimStrategy;
	private TargetVelocityTracker velocityTracker;
	private TargetingMode lastMode;
	private Transform lastTrackedTarget;

	public float ShootInterval
	{
		get => shootInterval;
		set => shootInterval = Mathf.Max(0f, value);
	}

	public TargetingMode TargetingMode
	{
		get => targetingMode;
		set
		{
			if (targetingMode == value) return;
			targetingMode = value;
			SetupAimStrategy();
		}
	}

	protected override void Start()
	{
		base.Start();
		rotator = GetComponent<CannonRotator>();
		aimStrategy = GetComponent<IAimStrategy>();
		velocityTracker = GetComponent<TargetVelocityTracker>();
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
			lastTrackedTarget = null;
			return;
		}

		if (target != lastTrackedTarget)
		{
			lastTrackedTarget = target;
			velocityTracker.Initialize(target);
			rotator.ResetAim();
			return;
		}

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

	protected override void Shoot()
	{
		bool usePhysics = targetingMode == TargetingMode.Ballistic;
		projectileProvider.SpawnProjectile(
			shootPoint.position,
			shootPoint.rotation,
			launchForce,
			usePhysics);
	}
}