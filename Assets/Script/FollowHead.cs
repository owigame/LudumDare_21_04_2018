using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour {

    public Transform _Head;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(_Head.position.x, transform.position.y, _Head.position.z);
        transform.eulerAngles = new Vector3(0, _Head.eulerAngles.y, 0);
    }
}
