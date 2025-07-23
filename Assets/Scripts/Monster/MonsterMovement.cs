using UnityEngine;
using System;

[RequireComponent(typeof(IMoveTargetProvider))]

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float reachDistance = 0.3f;

    public event Action ReachedTarget;
    private Transform moveTarget;
    private IMoveTargetProvider provider;

    private void Awake()
    {
        provider = GetComponent<IMoveTargetProvider>();
    }

    void Update()
    {
        moveTarget = provider.Target;
        if (moveTarget == null)
            return;

        Vector3 dir = moveTarget.position - transform.position;
        if (dir.magnitude <= reachDistance)
        {
            ReachedTarget?.Invoke();
            return;
        }

        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }
}
