using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadObject : MonoBehaviour {

	public bool dontDestroyOnLoad = true;

	// Use this for initialization
	void Start () {
		if (dontDestroyOnLoad)
			DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
