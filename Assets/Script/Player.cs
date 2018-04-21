using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player _player;

    public int RequiredScore;

    void Awake () {
        _player = this; 
	}

    
}
