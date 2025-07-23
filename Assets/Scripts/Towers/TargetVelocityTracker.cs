using UnityEngine;

public class TargetVelocityTracker : MonoBehaviour
{
    private Transform target;
    private Vector3 previousPosition;
    private float previousTime;

    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private bool skipNextUpdate = false;

    public Vector3 CurrentVelocity => velocity;

    public void Initialize(Transform target)
    {
        this.target = target;
        previousPosition = target.position;
        previousTime = Time.time;
        velocity = Vector3.zero;
        skipNextUpdate = true;
    }

    private void Update()
    {
        if (target == null) return;

        if (skipNextUpdate)
        {
            previousPosition = target.position;
            previousTime = Time.time;
            skipNextUpdate = false;
            return;
        }

        float deltaTime = Time.time - previousTime;
        Vector3 displacement = target.position - previousPosition;

        // Exponential smoothing
        velocity = Vector3.Lerp(velocity, displacement / deltaTime, Time.deltaTime / smoothTime);

        previousPosition = target.position;
        previousTime = Time.time;
    }
}