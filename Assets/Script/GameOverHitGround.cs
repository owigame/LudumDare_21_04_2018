using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHitGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		Debug.Log("Game Over collided " + CameraShake._CameraShake.gameObject.name);
		CameraShake._CameraShake.DoCameraShakeExplicit(2, 3);
	}
}
