using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Operator {
    plus,
    minus,
    multiply,
    divide
}

public class Player : MonoBehaviour {

    public static Player _player;

    [Header("Hand References")]
    public GunScript _leftHand;
    public GunScript _rightHand;


    void Awake () {
        _player = this;
    }

    

}