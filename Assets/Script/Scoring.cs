using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scoring : MonoBehaviour {

    public int CurrentScore;
    [HideInInspector]
    public UnityEvent RequiredReached;
    void Start () {
        ResetRequiredScore ();
        GunScript[] AllGuns = FindObjectsOfType<GunScript> ();
        foreach (var gun in AllGuns) {
            if (gun.HitEvent != null) gun.HitEvent.AddListener (UpdateScore);
        }
    }
    void ResetRequiredScore () {
        Player._player.RequiredScore = Mathf.RoundToInt (Random.Range (99, 999));
    }
    void UpdateScore (Operator Operator, int value) {
        switch (Operator) {
            case Operator.plus:
                CurrentScore += value;
                break;
            case Operator.minus:
                CurrentScore -= value;
                break;
        }
        if (CurrentScore == Player._player.RequiredScore) {
            RequiredReached.Invoke ();
            ResetRequiredScore ();
        }
    }
}