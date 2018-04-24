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
        if (other.tag == "StartGame")
        {
            SceneManager.LoadScene(1);
        }
        if (other.tag == "QuiteGame")
        {
            Application.Quit();
        }
        _rocket.OnExplode (other.transform.gameObject.layer == 8, (other.transform.gameObject.layer == 8 ? other.attachedRigidbody.gameObject : other.gameObject));
	}
}