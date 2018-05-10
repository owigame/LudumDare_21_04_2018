using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnAnimatorState : MonoBehaviour {

	public string stateName;
	public int layerNum = -1;
	public Animator _animator;
	public GameObject destroyTarget;
	bool destroyed = false;

	void Start () {

	}

	void Update () {
		if (destroyed == false) {
			if (_animator.GetCurrentAnimatorStateInfo (layerNum).IsName (stateName)) {
				Destroy (destroyTarget);
				destroyed = true;
			}
		}
	}
}