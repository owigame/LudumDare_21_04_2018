using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrigger : MonoBehaviour {

	public Rocket _rocket;

	void Start () {

	}

	void Update () {

	}

	private void OnTriggerEnter (Collider other) {
		_rocket.OnExplode (other.transform.gameObject.layer == 8, (other.transform.gameObject.layer == 8 ? other.attachedRigidbody.gameObject : other.gameObject));
	}
}