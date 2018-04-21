using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour {

    public int Value;

    public float speed;
    public UnityEvent IDie;
 	
	void Update ()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Player._Player.transform.position, step);
	}

    public void ZombieDie()
    {
        IDie.Invoke();
    }

}
