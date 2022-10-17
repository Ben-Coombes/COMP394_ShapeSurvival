using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject spawnPoint;

    private float spawnTimer;
    private float increaseSpawnTimer;
    public float timeBetweenIncrease = 10; //time between each spawning speed increase
    public float timer = 5; //starting time between spawns
    public float minTimeBetweenSpawns = 2; //timer increase cutoff speed
    private bool maxSpawnSpeedReached = false;

    public float range = 20.0f;



    //cluster
    public int maxClusterSize;
    public int minClusterSize;
    private float clusterTimer; //cluster timer
    public float timeBetweenClusterIncrease = 5; // time between cluster increase

    public float spawnOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //increase spawn speed
        increaseSpawnTimer += Time.deltaTime;
        if (increaseSpawnTimer > timeBetweenIncrease && !maxSpawnSpeedReached)
        {
            increaseSpawnTimer = 0;
            timer = timer - 1;
            if (timer <= minTimeBetweenSpawns)
            {
                maxSpawnSpeedReached = true;
                
            }
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer > timer)
        {
            spawnTimer -= timer;
            SpawnEnemy();
        }

        //increase cluster size
        
        clusterTimer += Time.deltaTime;
        if (clusterTimer > timeBetweenClusterIncrease)
        {
            clusterTimer = 0;
            minClusterSize++;
            maxClusterSize++;
            
        }
        


    }

    public void SpawnEnemy()
    {
        Vector3 clusterPoint = RandomPoint();
        int amount = Random.Range(minClusterSize, maxClusterSize + 1);
        for (int i = 0; i < amount; i++)
        {
            float xOffset = Random.Range(-spawnOffset, spawnOffset);
            float yOffset = Random.Range(-spawnOffset, spawnOffset);
            clusterPoint += new Vector3(xOffset,0, yOffset);
            Instantiate(enemyToSpawn, clusterPoint, Quaternion.identity);
        }
        
    }


    Vector3 RandomPoint()
    {
        Vector3 rand = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        Vector3 randomPoint = rand;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 40.0f, NavMesh.AllAreas);

        return hit.position;
        
        
    }

    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}
