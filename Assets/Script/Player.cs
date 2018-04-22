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

    public ClipSet Clips_Plus;
    public ClipSet Clips_Minus;
    public ClipSet Clips_Mul;
    public ClipSet Clips_Div;

    void Awake () {
        _player = this;
    }
}
[System.Serializable]
public class ClipSet
{
    public AudioClip WeaponFire,Reload,ClipEmpty;
}