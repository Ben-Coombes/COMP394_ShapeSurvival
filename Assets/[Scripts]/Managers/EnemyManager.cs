using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyList = new();
    private GameObject[] totalEnemies;

    public GameObject spawnPoint;

    [Header("Spawning")]
    public int maxEnemyCount;
    private float spawnTimer;
    private float increaseSpawnTimer;
    public float timeBetweenIncrease = 10; //time between each spawning speed increase
    public float timer = 5; //starting time between spawns
    public float minTimeBetweenSpawns = 2; //timer increase cutoff speed
    private bool maxSpawnSpeedReached = false;
    public float range = 20.0f;

    [Header("Cluster")]
    private int maxClusterSize;
    private int minClusterSize;
    private float clusterTimer; //cluster timer
    public float timeBetweenClusterIncrease = 5; // time between cluster increase
    public int clusterCap;
    public float spawnOffset;

    [Header("Pooling")] 
    private ObjectPool<EnemyAI> _pool;
    public int InactiveCount;
    public int ActiveCount;

    void Awake() => _pool = new ObjectPool<EnemyAI>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool);

    private void OnReturnEnemyToPool(EnemyAI obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnTakeEnemyFromPool(EnemyAI obj)
    {
        obj.gameObject.SetActive(true);
    }
    private EnemyAI CreateEnemy()
    {
        Debug.Log("Create enemy");
        int randomOption = Random.Range(0, enemyList.Count);
        var enemy = Instantiate(enemyList[randomOption]);
        enemy.GetComponent<EnemyAI>().SetPool(_pool);
        return enemy.GetComponent<EnemyAI>();
    }
    void Start()
    {
        clusterCap = 25;
        minClusterSize = 1;
        maxClusterSize = 2;
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
            if (maxEnemyCount > _pool.CountActive)
            {
                SpawnEnemy();
            }
            
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

    private void Update()
    {
        //totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        InactiveCount = _pool.CountInactive;
        ActiveCount = _pool.CountActive;
    }
    public void SpawnEnemy()
    {
        Vector3 clusterPoint = RandomPoint();
        int amount = Random.Range(minClusterSize, maxClusterSize + 1);
        if (amount > clusterCap)
        {
            Debug.Log("amount > clustre cap");
            amount = clusterCap;
        }
        Debug.Log(amount);
        for (int i = 0; i < amount; i++)
        {
            float xOffset = Random.Range(-spawnOffset, spawnOffset);
            float yOffset = Random.Range(-spawnOffset, spawnOffset);
            clusterPoint += new Vector3(xOffset,0, yOffset);
            var enemy = _pool.Get();
            enemy.transform.position = clusterPoint;
            Debug.Log("dwadawdwad");
        }
    }

    
    Vector3 RandomPoint()
    {
        Vector3 rand = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        Vector3 randomPoint = rand;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 120f, NavMesh.AllAreas);

        return hit.position;
        
        
    }

    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}
