using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public int Value;

    public float speed;
    
    public void Die()
    {
        gameObject.layer = 0;
        Debug.Log("Zombie killed");
        Destroy(gameObject); //TEMP
    }
 	
	void Update ()
    {
        float step = speed * Time.deltaTime;
        if (Player._player != null){
            transform.position = Vector3.MoveTowards(transform.position, Player._player.transform.position, step); 
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Destroy(gameObject);
            Scoring._scoring.TakeDamage();
        }
    }

}
