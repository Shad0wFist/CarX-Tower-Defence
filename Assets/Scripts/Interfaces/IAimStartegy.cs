using UnityEngine;

public interface IAimStrategy
{
    bool CalculateAimDirection(Vector3 towerPosition, Vector3 targetPosition, Vector3 targetVelocity, out Vector3 aimDirection);
}