using UnityEngine;

public class CannonRotator : MonoBehaviour
{
    [SerializeField] private Transform baseTransform; // rotation Y
    [SerializeField] private Transform barrelTransform; // elevation X
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float elevationSpeed = 30f;

    [SerializeField] private float minElevation = -45f;
    [SerializeField] private float maxElevation = 85f;
    [SerializeField] private float rotationTolerance = 0.5f;

    private Vector3 desiredDirection;
    private bool isAimed = false;
    public bool IsAimed() => isAimed;

    public void SetAimDirection(Vector3 direction)
    {
        desiredDirection = direction.normalized;
        isAimed = false;
    }

    public void RotateCannon()
    {
        if (isAimed) return;

        // Rotate the base along Y
        Vector3 desiredHorizontal = new Vector3(desiredDirection.x, 0, desiredDirection.z).normalized;
        Quaternion targetBase = Quaternion.LookRotation(desiredHorizontal);
        baseTransform.rotation = Quaternion.RotateTowards(
            baseTransform.rotation,
            targetBase,
            rotationSpeed * Time.deltaTime
        );

        // Calculating the barrel elevation angle
        Vector3 relativeDirection = baseTransform.InverseTransformDirection(desiredDirection);
        float angle = Mathf.Atan2(relativeDirection.y, relativeDirection.z) * Mathf.Rad2Deg;

        float clampedAngle = Mathf.Clamp(angle, minElevation, maxElevation);

        // Target barrel rotation
        Quaternion targetElevation = Quaternion.Euler(-clampedAngle, 0, 0);
        barrelTransform.localRotation = Quaternion.RotateTowards(
            barrelTransform.localRotation,
            targetElevation,
            elevationSpeed * Time.deltaTime
        );

        float baseAngle = Quaternion.Angle(baseTransform.rotation, targetBase);
        float barrelAngle = Quaternion.Angle(barrelTransform.localRotation, targetElevation);

        // If the muzzle is at the limit, but the target angle is outside the limits
        bool isBarrelAtLimit = (clampedAngle != angle);
        bool isBarrelAligned = (barrelAngle <= rotationTolerance);

        isAimed = (baseAngle <= 0.5f && isBarrelAligned && !isBarrelAtLimit);
    }

    public void ResetAim()
    {
        isAimed = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && !isAimed)
        {
            // Target direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(baseTransform.position, baseTransform.position + desiredDirection * 10f);

            // Current barrel direction
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(barrelTransform.position, barrelTransform.position + barrelTransform.forward * 5f);
        }
    }
}