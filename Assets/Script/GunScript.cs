using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public UnityEvent<Operator, int> HitEvent;
    public Operator opp;
    //int raycastLayerMask = ;

    RaycastHit hit;

    void Shoot()
    {
        if (Physics.Raycast(transform.position, transform.forward, Mathf.Infinity))
        {
            if (hit.transform.tag == "enemy")
            {
                HitEvent.Invoke(opp,GetComponent<Zombie>.value)
            }
        }
    }
}
public enum Operator
{
    plus,
    minus
}
