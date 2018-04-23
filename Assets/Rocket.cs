using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Operator opp;

	public GameObject _redRocket;
	public GameObject _blueRocket;
	public GameObject _explosion;

	public float Rate = 5;
	void Start () {

	}

	void Update () {
		transform.position += transform.forward * Time.deltaTime * Rate;
	}

	public void SetRocket (Operator _opp) {
		_opp = _opp;
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
		Debug.Log (other.GetComponent<Zombie> ());
		_explosion.SetActive (true);
		_explosion.transform.parent = null;
		if (enemy) {
			Zombie _zombie = other.GetComponent<Zombie> ();
			Debug.Log ("Rocket Hit Enemy: " + _zombie.transform.name);
			Scoring._scoring.UpdateScore (opp, _zombie.Value);
			_zombie.Die ();
		}
		Destroy (gameObject);
	}
}