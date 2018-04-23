using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeMatColour : MonoBehaviour {

    public enum scoreTypes {
        score,
        currentValue,
        targetValue,
        multiplier
    }

    public scoreTypes _scoreType;
    public MeshRenderer _scoreMesh;
    public Material _positiveCol;
    public Material _negativeCol;

    private void OnEnable () {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onTargetValueUpdated += ValueUpdated;
        if (_scoreType == scoreTypes.multiplier) Scoring.onMultiplierUpdated += ValueUpdated;
    }

    private void OnDisable () {
        if (_scoreType == scoreTypes.score) Scoring.onScoreUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.currentValue) Scoring.onCurrentValueUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onTargetValueUpdated -= ValueUpdated;
        if (_scoreType == scoreTypes.targetValue) Scoring.onMultiplierUpdated -= ValueUpdated;
    }

    void ValueUpdated (int _value, string _suffix = "") {
        if (_value >= 0) {
            //postive
            _scoreMesh.material = _positiveCol;
        } else {
            //negative
            _scoreMesh.material = _negativeCol;
        }
    }
}