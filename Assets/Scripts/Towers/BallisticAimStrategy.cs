using UnityEngine;

public class BallisticAimStrategy : IAimStrategy
{
    private float projectileSpeed;
    private float gravity = 9.81f;

    public BallisticAimStrategy(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public bool CalculateAimDirection(
        Vector3 towerPosition,
        Vector3 targetPosition,
        Vector3 targetVelocity,
        out Vector3 aimDirection)
    {
        // Iterative prediction
        Vector3 predictedPos = targetPosition;
        float angle = 0f;
        const int maxIters = 3;

        for (int i = 0; i < maxIters; i++)
        {
            Vector3 toTarget = predictedPos - towerPosition;
            float x = new Vector2(toTarget.x, toTarget.z).magnitude;
            float y = toTarget.y;

            float v2 = projectileSpeed * projectileSpeed;
            float underSqrt = v2 * v2 
                              - gravity * (gravity * x * x + 2f * y * v2);

            if (underSqrt < 0f)
            {
                aimDirection = Vector3.zero;
                return false;
            }

            float sqrtD = Mathf.Sqrt(underSqrt);
            angle = Mathf.Atan2(v2 - sqrtD, gravity * x);

            // Estimated flight time
            float time = x / (projectileSpeed * Mathf.Cos(angle));
            predictedPos = targetPosition + targetVelocity * time;
        }

        Vector3 finalToTarget = predictedPos - towerPosition;
        Vector3 flat = new Vector3(finalToTarget.x, 0f, finalToTarget.z).normalized;
        Vector3 dir = flat * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);

        aimDirection = dir.normalized;
        return true;
    }
}
