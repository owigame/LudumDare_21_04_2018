using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class Scoring : MonoBehaviour {

    public delegate void ScoreEvents(int _score = 0, string _suffix = "");
    public static ScoreEvents onScoreUpdated;
    public static ScoreEvents onCurrentValueUpdated;
    public static ScoreEvents onTargetValueUpdated;
    public static ScoreEvents onMultiplierUpdated;

    public static Scoring _scoring;

    public int score; //Overall Score
    public int currentValue; //Dynamic value
    public int targetValue; //Value needed to increase score
    public int highestScore;
    public int multiplier;

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
        if (onScoreUpdated != null) onScoreUpdated(score);
        if (onCurrentValueUpdated != null) onCurrentValueUpdated(currentValue);
        if (onTargetValueUpdated != null) onTargetValueUpdated(targetValue);
    }
    void ResetRequiredScore () {
        targetValue = Mathf.RoundToInt (Random.Range (99, 999));
        if (onTargetValueUpdated != null) onTargetValueUpdated(targetValue);
    }
    public void UpdateScore (Operator Operator, int value) {
        switch (Operator) {
            case Operator.plus:
                currentValue += value*multiplier;
                break;
            case Operator.minus:
                currentValue -= value*multiplier;
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
            score++;
            highestScore = score > highestScore ? score : highestScore;
            if (onScoreUpdated != null) onScoreUpdated(score);
        } else
        {
            if (onCurrentValueUpdated != null) onCurrentValueUpdated(currentValue);
        }
    }
    public void TakeDamage()
    {
        if (score > 1)
        {
            //Do damage FX
            score--;
        } else
        {
            Debug.Log("END ROUND");
            Time.timeScale = 0;
            //Display end round
        }
        if (onScoreUpdated != null) onScoreUpdated(score);
    }

    public void UpdateMultiplier(int _multiplier)
    {
        multiplier = _multiplier;
        onMultiplierUpdated(_multiplier, "x");
    }
}