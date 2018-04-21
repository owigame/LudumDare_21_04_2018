using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFetchScore : MonoBehaviour {

    public enum scoreTypes
    {
        score, currentValue
    }

    public scoreTypes _scoreType;
    public Text _scoreText;

    private void OnEnable()
    {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated += ValueUpdated;
    }

    private void OnDisable()
    {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated -= ValueUpdated;
    }

    void ValueUpdated(int _value)
    {
        _scoreText.text = _value.ToString();
    }
}
