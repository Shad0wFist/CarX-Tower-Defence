using UnityEngine;
using System.Collections;

public class CannonProjectile : Projectile
{
    public override void Initialize(params object[] args)
    {
        base.Initialize();
        if (args.Length > 0 && args[0] is float force)
        {
            // One-time impulse
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
        }

        rb.useGravity = false;
        if (args.Length > 1 && args[1] is bool flag)
        {
            // Is balistic?
            rb.useGravity = flag;
        }
    }

    protected override void Move() { }
}