using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reload Trigger Enter " + other.attachedRigidbody.transform.name);
        if (other.attachedRigidbody.tag == "Gun")
        {
            other.attachedRigidbody.GetComponent<GunScript>().Reload();
        }
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
