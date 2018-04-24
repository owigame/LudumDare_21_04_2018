using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFetchScore : MonoBehaviour {

    public enum scoreTypes {
        score,
        currentValue,
        targetValue,
        multiplier,
        time,
        highScore
    }

    public scoreTypes _scoreType;
    public Text _scoreText;

    private void OnEnable () {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onTargetValueUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.multiplier) Scoring.onMultiplierUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.time) Scoring.onTimeUpdated += ValueUpdated;
    }

    private void OnDisable () {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onTargetValueUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onMultiplierUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.time) Scoring.onTimeUpdated -= ValueUpdated;
    }

    void ValueUpdated (int _value, string _suffix = "") {
        if (_scoreText != null) _scoreText.text = _value.ToString () + _suffix;
    }

    private void Start () {
        if (_scoreType == scoreTypes.highScore) {
            _scoreText.text = PlayerPrefs.GetInt ("highScore", 0).ToString ();
        }
    }
}