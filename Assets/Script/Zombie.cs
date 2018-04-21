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
        transform.position = Vector3.MoveTowards(transform.position, Player._player.transform.position, step); 
	}

    public void RombieDie()
    {
        IDie.Invoke();
        Destroy(gameObject);
    }

}
