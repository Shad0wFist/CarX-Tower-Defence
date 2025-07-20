using UnityEngine;

public class CannonAimCondition : MonoBehaviour, IShootCondition
{
    private CannonRotator rotator;

    private void Awake()
    {
        rotator = GetComponent<CannonRotator>();
    }

    public bool CanShoot()
    {
        return rotator.IsAimed();
    }
}