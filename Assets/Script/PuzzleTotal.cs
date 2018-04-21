using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTotal : MonoBehaviour {

	int total;
	void Start () {
		Scoring score = FindObjectOfType<Scoring> ();
		score.RequiredReached.AddListener (TotalPuzzle);
	}
	void TotalPuzzle () {
		total++;
	}
}