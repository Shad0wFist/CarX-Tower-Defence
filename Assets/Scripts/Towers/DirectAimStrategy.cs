using UnityEngine;

public class DirectAimStrategy : IAimStrategy
{
    private float projectileSpeed;

    public DirectAimStrategy(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public bool CalculateAimDirection(Vector3 towerPosition, Vector3 targetPosition, Vector3 targetVelocity, out Vector3 aimDirection)
    {
        Vector3 toTarget = targetPosition - towerPosition;
        float distance = toTarget.magnitude;
        float timeToTarget = distance / projectileSpeed;

        for (int i = 0; i < 3; i++)
        {
            Vector3 predictedPosition = targetPosition + targetVelocity * timeToTarget;
            toTarget = predictedPosition - towerPosition;
            distance = toTarget.magnitude;
            timeToTarget = distance / projectileSpeed;
        }

        aimDirection = toTarget.normalized;
        return true;
    }
}