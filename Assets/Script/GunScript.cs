using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour {
    Rewired.Player _input;
    public int playerHand = 0; // 0 Left | 1 Right
    public UnityEvent<Operator, int> HitEvent;
    public Operator opp;
    //int raycastLayerMask = ;

    RaycastHit hit;

    private void Shoot () {
        if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {
            Debug.DrawLine(transform.position, hit.transform.position, Color.green);
            if (hit.transform.tag == "Enemy") {
                HitEvent.Invoke (opp, GetComponent<Zombie> ().Value);
            }
        } else {
            Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
        }
    }

    void Start () {
        _input = ReInput.players.GetPlayer (playerHand);
    }

    void Update () {
        //Submit operator choice
        if (_input.GetButtonUp ("OperatorSubmit")) {
            float _X = _input.GetAxis("TouchPadX");
            float _Y = _input.GetAxis("TouchPadY");
            if (_X > 0 && Mathf.Abs(_Y) < 0.5f){
                opp = Operator.multiply;
            }
            if (_X < 0 && Mathf.Abs(_Y) < 0.5f){
                opp = Operator.divide;
            }
            if (_Y > 0 && Mathf.Abs(_X) < 0.5f){
                opp = Operator.plus;
            }
            if (_Y < 0 && Mathf.Abs(_X) < 0.5f){
                opp = Operator.minus;
            }
            Debug.Log("Operator Submit " + opp);
        }

        //Shoot
        if (_input.GetButtonDown("Shoot")){
            Debug.Log("Shoot");
            Shoot();
        }
    }
}