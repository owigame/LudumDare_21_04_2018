using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public int Value;

    public float speed;
    
    public void Die()
    {
        Debug.Log("Zombie killed");
        Destroy(gameObject);
    }
 	
	void Update ()
    {
        float step = speed * Time.deltaTime;
        if (Player._player != null){
            transform.position = Vector3.MoveTowards(transform.position, Player._player.transform.position, step); 
        }
	}
}
