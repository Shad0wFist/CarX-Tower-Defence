using UnityEngine;

public interface ITargetSelector
{
    Transform SelectTarget(Vector3 origin, float range);
}