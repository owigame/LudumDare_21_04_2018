﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardAdInfinitum : MonoBehaviour {

    public float Rate = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.forward*Rate);
	}
}
