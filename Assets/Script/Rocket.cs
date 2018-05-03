using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


	public GameObject _redRocket;
	public GameObject _blueRocket;
	public GameObject _explosion;

	public float Rate = 5;

	[Header("Output")]
	public Operator opp;
	void Start () {

	}

	void Update () {
		transform.position += transform.forward * Time.deltaTime * Rate;
	}

	public void SetRocket (Operator _opp) {
		opp = _opp;
		switch (_opp) {
			case Operator.minus:
				_blueRocket.SetActive (true);
				break;
			case Operator.plus:
				_redRocket.SetActive (true);
				break;
		}
	}

	public void OnExplode (bool enemy, GameObject other) {
		_explosion.SetActive (true);
		_explosion.transform.parent = null;
		_explosion.GetComponent<Explosion>().Explode(transform.position, opp);
		CameraShake._CameraShake.DoCameraShake(_explosion.transform.position);
		// if (enemy) {
		// 	Debug.Log (other.GetComponent<Zombie> ());	
		// 	Zombie _zombie = other.GetComponent<Zombie> ();
		// 	Debug.Log ("Rocket Hit Enemy: " + _zombie.transform.name);
		// 	Scoring._scoring.UpdateScore (opp, _zombie.Value);
		// 	Debug.Log("Zombie killed " + opp + " value " + _zombie.Value);
		// 	_zombie.Die (opp);
		// }
		Destroy (gameObject);
	}
}