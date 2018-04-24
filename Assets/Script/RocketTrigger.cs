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
			SceneManager.LoadScene (1);
		}
		if (other.tag == "QuitGame") {
			Application.Quit ();
		}
		if (other.tag == "Menue") {
			SceneManager.LoadScene (0);
		}
		_rocket.OnExplode (other.transform.gameObject.layer == 8, (other.transform.gameObject.layer == 8 ? other.attachedRigidbody.gameObject : other.gameObject));
	}
}