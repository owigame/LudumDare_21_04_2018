using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float radius = 50.0F;
    //public float power = 100.0F;
    
    public void Explode(Vector3 explosionPosition)
    {
        Vector3 explosionPos = explosionPosition;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        Debug.Log(colliders.Length);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Enemy")
            {
                colliders[i].GetComponent<Zombie>().Die();
            }
        }

        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
         = transform.GetChild(0);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
