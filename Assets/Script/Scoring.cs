using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VRTK;

public class Scoring : MonoBehaviour {

    public delegate void ScoreEvents (int _score = 0, string _suffix = "");
    public static ScoreEvents onScoreUpdated;
    public static ScoreEvents onCurrentValueUpdated;
    public static ScoreEvents onTargetValueUpdated;
    public static ScoreEvents onMultiplierUpdated;
    public static ScoreEvents onTimeUpdated;

    public static Scoring _scoring;

    public int score; //Overall Score
    public int currentValue; //Dynamic value
    public int targetValue; //Value needed to increase score
    public float highestScore;
    public int multiplier;
    public int timeDuration = 180;
    public int timeRemaining;

    public UnityEvent RequiredReached;

    [Header ("Audio")]
    AudioSource _audio;
    public AudioClip _onScoreUpdateClip;

    private void Awake () {
        _scoring = this;
    }

    void Start () {
        highestScore = PlayerPrefs.GetFloat ("highestScore", 0);
        Debug.Log ("Highest Score: " + highestScore);
        _audio = GetComponent<AudioSource> ();
        ResetRequiredScore ();
        GunScript[] AllGuns = FindObjectsOfType<GunScript> ();
        foreach (var gun in AllGuns) {
            if (gun.HitEvent != null) gun.HitEvent.AddListener (UpdateScore);
        }
        if (onScoreUpdated != null) onScoreUpdated (score);
        if (onCurrentValueUpdated != null) onCurrentValueUpdated (currentValue);
        if (onTargetValueUpdated != null) onTargetValueUpdated (targetValue);
        timeRemaining = timeDuration;
        StartCoroutine (CountDown ());
    }

    private void Update () {

    }

    IEnumerator CountDown () {
        WaitForSeconds _wait = new WaitForSeconds (1);
        while (timeRemaining > 0) {
            timeRemaining--;
            if (onTimeUpdated != null) onTimeUpdated (timeRemaining);
            yield return _wait;
        }
        Debug.Log ("GAME OVER");
        DoGameOver ();
    }

    void DoGameOver () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex == 2 ? 1 : 3);
    }

    void ResetRequiredScore () {
        targetValue = Mathf.RoundToInt (Random.Range (99, 999));
        if (onTargetValueUpdated != null) onTargetValueUpdated (targetValue);
        timeRemaining = timeDuration;
    }

    public void UpdateScore (Operator Operator, int value) {
        switch (Operator) {
            case Operator.plus:
                currentValue += value * multiplier;
                break;
            case Operator.minus:
                currentValue -= value * multiplier;
                break;
            case Operator.multiply:
                currentValue *= value;
                break;
            case Operator.divide:
                currentValue /= value;
                break;
        }
        if (currentValue == targetValue) {
            if (onCurrentValueUpdated != null) onCurrentValueUpdated (currentValue);
            RequiredReached.Invoke ();
            ResetRequiredScore ();
            score++;
            highestScore = score > highestScore ? score : highestScore;
            PlayerPrefs.SetFloat ("highestScore", highestScore);
            Debug.Log ("New Highscore: " + PlayerPrefs.GetFloat ("highestScore", highestScore));
            if (onScoreUpdated != null) onScoreUpdated (score);
            _audio.PlayOneShot (_onScoreUpdateClip);
        } else {
            if (onCurrentValueUpdated != null) onCurrentValueUpdated (currentValue);
        }
    }

    public void TakeDamage () {
        if (score > 1) {
            //Do damage FX
            score--;
        } else {
            Debug.Log ("END ROUND");
            Time.timeScale = 0;
            //Display end round
        }
        if (onScoreUpdated != null) onScoreUpdated (score);
    }

    public void UpdateMultiplier (int _multiplier) {
        multiplier = _multiplier;
        if (onMultiplierUpdated != null) onMultiplierUpdated (_multiplier, "x");
    }
}