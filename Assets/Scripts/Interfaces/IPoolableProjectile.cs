using UnityEngine;

public interface IPoolableProjectile
{
    event System.Action<GameObject> OnRelease;
    void Release();
}