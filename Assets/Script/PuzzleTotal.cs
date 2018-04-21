using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTotal : MonoBehaviour {

	public int total;
	void Start () {
		Scoring score = FindObjectOfType<Scoring> ();
		score.RequiredReached.AddListener (IncTotal);
	}
	void IncTotal () {
		total++;
	}

}