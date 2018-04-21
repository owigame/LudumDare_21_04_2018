﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scoring : MonoBehaviour {

    public delegate void ScoreEvents(int _score = 0);
    public static ScoreEvents onScoreUpdated;
    public static ScoreEvents onCurrentValueUpdated;

    public static Scoring _scoring;

    public int score; //Overall Score
    public int currentValue; //Dynamic value
    public int targetValue; //Value needed to increase score

    public UnityEvent RequiredReached;

    private void Awake()
    {
        _scoring = this;
    }

    void Start () {
        ResetRequiredScore ();
        GunScript[] AllGuns = FindObjectsOfType<GunScript> ();
        foreach (var gun in AllGuns) {
            if (gun.HitEvent != null) gun.HitEvent.AddListener (UpdateScore);
        }
    }
    void ResetRequiredScore () {
        Scoring._scoring.targetValue = Mathf.RoundToInt (Random.Range (99, 999));
    }
    public void UpdateScore (Operator Operator, int value) {
        switch (Operator) {
            case Operator.plus:
                currentValue += value;
                break;
            case Operator.minus:
                currentValue -= value;
                break;
            case Operator.multiply:
                currentValue *= value;
                break;
            case Operator.divide:
                currentValue /= value;
                break;
        }
        if (currentValue == targetValue) {
            RequiredReached.Invoke ();
            ResetRequiredScore ();
            onScoreUpdated(score++);
        } else
        {
            onCurrentValueUpdated(currentValue);
        }
    }
}