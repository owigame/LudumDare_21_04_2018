using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpNumber : MonoBehaviour {

	Animator _anim;
	bool alive;

	public Text numberText;

	void Start () {
		_anim = GetComponent<Animator> ();
	}

	void Update () {
		if (_anim.GetCurrentAnimatorStateInfo (-1).IsName ("Destroy") && alive) {
			Destroy (gameObject);
			alive = false;
		}
	}

	public void SetNumber (int number, Operator opp) {
		numberText.text = (opp == Operator.plus ? "+" : "-") + number.ToString ();
	}
}