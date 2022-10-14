using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject spawnPoint;

    public float spawnTimer;
    public float timer = 3;

    public float range = 20.0f;

    //cluster
    public int maxClusterSize;
    public int minClusterSize;

    public float spawnOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > timer)
        {
            spawnTimer -= timer;
            
            SpawnEnemy();

            
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
            print(xOffset+yOffset);
            Instantiate(enemyToSpawn, clusterPoint, Quaternion.identity);
        }
        
    }


    Vector3 RandomPoint()
    {
        Vector3 rand = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        Vector3 randomPoint = rand;
        print(randomPoint);
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
