using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketTrigger : MonoBehaviour {

	public Rocket _rocket;

	void Start () {

	}

	void Update () {

	}

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "StartGame") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex == 1 ? 2 : 4);
		}
		if (other.tag == "QuitGame") {
			Application.Quit ();
		}
		bool zombieLayer = other.transform.gameObject.layer == 8;
		GameObject _object = (other.transform.gameObject.layer == 8 ? other.attachedRigidbody.gameObject : other.transform.gameObject);

		Debug.Log ("Explode: " + other.transform.gameObject);
		Debug.Log ("Layer: " + other.transform.gameObject.layer);
		Debug.Log ("GameObject: " + (other.transform.gameObject.layer == 8 ? other.attachedRigidbody.gameObject : other.transform.gameObject));
		_rocket.OnExplode (zombieLayer, _object);
	}
}