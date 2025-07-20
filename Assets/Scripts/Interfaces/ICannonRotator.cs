using UnityEngine;

public interface ITurretRotator
{
    bool RotateTowards(Vector3 desiredDirection, float deltaTime);
}
