using UnityEngine;

public class Spawning : MonoBehaviour
{
    //Bennet

    
    public GameObject SpawnPosition;
    public GameObject SpawnOBJ;
    public int EnemySpawnAmount,SpawnEveryxSeconds=2;
    private float TimeSinceLastSpawn;


    private void Start()
    {
        EnemySpawnAmount = 0;
        SpawnEnemy();
    }

    private void Update()
    {
        TimeSinceLastSpawn += Time.deltaTime;
        if (TimeSinceLastSpawn> SpawnEveryxSeconds)
        {
            SpawnEnemy();
            TimeSinceLastSpawn = 0.0f;
        }
    }

    void SpawnEnemy()
    {
        
        // set random rotation
        Vector3 SpaRot = transform.eulerAngles;
        SpaRot.y = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = SpaRot;
        EnemySpawnAmount++;
        //spawn enemy
        Instantiate(SpawnOBJ,SpawnPosition.transform.position,Quaternion.identity);
        
    }
	
}
