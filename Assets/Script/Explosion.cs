using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float radius = 50.0F;
    public float power = 10.0F;
    
    public void Explode(Vector3 explosionPosition)
    {
        Vector3 explosionPos = explosionPosition;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Enemy")
            {
                colliders[i].GetComponent<Zombie>().Die();
            }
        }
    }
}
