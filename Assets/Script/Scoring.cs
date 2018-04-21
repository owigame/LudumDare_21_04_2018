using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour {

    public float TotalScore;

	void Start () {

        GunScript[] AllGuns = FindObjectsOfType<GunScript>();
        foreach (var gun in AllGuns)
        {
            gun.HitEvent.AddListener(UpdateScore);
        }
	}

    void UpdateScore(Operator OP, int VAL)
    {
        
    }
}
