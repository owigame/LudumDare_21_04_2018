using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public int Value;

    public float speed;

    public bool imExploding;


    public void Die()
    {
        gameObject.layer = 0;
        Debug.Log("Zombie killed");
        Destroy(gameObject); //TEMP
    }
 	
	void Update ()
    {
        float step = speed * Time.deltaTime;
        if (Player._player != null && !imExploding){
            transform.position = Vector3.MoveTowards(transform.position, Player._player.playerObject.transform.position, step); 
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
