using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillProjectiles_OutsideCarnival : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Projectile p))
        {
            p.destroySelf();
        }
    }
}
