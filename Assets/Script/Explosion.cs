using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float radius = 50.0F;
    //public float power = 100.0F;
    private Vector3 explosionPos;
    public GameObject ExplosionPrefabEffect;

    public void Explode(Vector3 explosionPosition)
    {
        explosionPos = explosionPosition;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        Debug.Log(colliders.Length);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Enemy")
            {
                colliders[i].GetComponent<Zombie>().Die();
            }
        }

        Instantiate(ExplosionPrefabEffect, explosionPos, Quaternion.identity);
    }
}
