using UnityEngine;

public class Spawning : MonoBehaviour
{
    //Bennet

    
    public GameObject SpawnPosition;
    public GameObject SpawnOBJ;
    public int EnemySpawnAmount;


    private void Start()
    {
        EnemySpawnAmount = 0;
        Invoke("SpawnEnemy", 2);
    }


    void SpawnEnemy()
    {
        
        // set random rotation
        Vector3 SpaRot = transform.eulerAngles;
        SpaRot.z = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = SpaRot;
        EnemySpawnAmount++;
        //spawn enemy
        Instantiate(SpawnOBJ, SpawnPosition.transform);
        //loop 
        if (EnemySpawnAmount < 50)
        {
            Invoke("SpawnEnemy", 2);
        }
    }
	
}
