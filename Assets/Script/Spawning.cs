using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    //Bennet


    public GameObject SpawnPosition;
    public GameObject SpawnOBJ;


    void SpawnEnemy()
    {

        Vector3 SpaRot = transform.eulerAngles;
        SpaRot.z = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = SpaRot;

        Instantiate(SpawnOBJ, SpawnPosition.transform);
    }
	
}
