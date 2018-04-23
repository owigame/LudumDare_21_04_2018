using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float radius = 20.0F;
    public GameObject ExplosionPrefabEffect;

    public void Explode(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Enemy")
            {
                colliders[i].GetComponent<Zombie>().Die();
            }
        }

        Instantiate(ExplosionPrefabEffect, explosionPosition, Quaternion.identity);
    }
}
