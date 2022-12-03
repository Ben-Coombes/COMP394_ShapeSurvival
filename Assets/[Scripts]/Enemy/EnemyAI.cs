using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("Unity")]
    public NavMeshAgent agent;
    public Transform player;
    FiniteStateMachine fsm;
    [Header("XP")]
    public GameObject xp;
    public List<GameObject> xpList = new();
    public int xpRange;

    [Header("Coin")]
    public GameObject coin;
    public List<GameObject> coinList = new();
    public int coinRange;
    private int coinDropChance;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Spawning/Death")]
    public bool spawnComplete = false;
    //death
    public bool isDead = false;
    private Vector3 deathPosition;

    [Header("Attacking")]
    public bool playerInTrigger = false;
    public float timeInCollider;
    public float timeNotInCollider;

    public float damage;

    [Header("Health")]
    public float health;
    public float maxHealth;
    //knockback
    public bool isknockedBack = false;
    private Vector3 knockbackDirection;

    [Header("UI/Anaimation")]
    public GameObject healthBarUI;
    public Slider healthBarSlider;
    public Animator anaimator;
    public Rigidbody rb;

    private IObjectPool<EnemyAI> _pool;


    private void Awake()
    {
        player = GameObject.Find("Player").transform; //find the player in the scene
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        healthBarUI.SetActive(false);
    }

    private void OnEnable()
    {
        if (fsm != null)
        {
            health = maxHealth;
            healthBarUI.SetActive(false);
            isknockedBack = false;
            playerInTrigger = false;
            isDead = false;
            spawnComplete = false;
            agent.enabled = true;
            //rb.detectCollisions = true;
            var collidersObj = gameObject.GetComponentsInChildren<Collider>();
            foreach (var colliderItem in collidersObj)
            {
                colliderItem.enabled = true;
            }

            fsm.TransitionTo("Spawning");
        }
    }

    private void Start()
    {
        
        fsm = new FiniteStateMachine();
        //var patrolingState = fsm.CreateState("Patroling");
        //var wanderingState = fsm.CreateState("Wandering");
        var spawningState = fsm.CreateState("Spawning");
        var ChasingState = fsm.CreateState("Chasing");
        var deadState = fsm.CreateState("Dead");
        var knockbackState = fsm.CreateState("Knockback");

        spawningState.onEnter = delegate
        {
            StartCoroutine(SpawnAnaimator());
        };

        spawningState.onFrame = delegate
        {
            if (spawnComplete)
            {
                fsm.TransitionTo("Chasing");
            }
        };

        spawningState.onExit = delegate
        {
           
        };

        //chasing state
        ChasingState.onEnter = delegate
        {
            
        };

        ChasingState.onFrame = delegate
        {
            if (agent != null)
            {
                agent.SetDestination(player.position);

            }
            anaimator.Play("EnemyIdle2");
            
            
        };

        ChasingState.onExit = delegate
        {
           
        };

        deadState.onEnter = delegate
        {
            deathPosition = transform.position;
            //disable everything 
            healthBarUI.SetActive(false);
            agent.enabled = false;
            rb.detectCollisions = false;
            //Destroy(GetComponentInChildren<Rigidbody>());
            var collidersObj = gameObject.GetComponentsInChildren<Collider>();
            for (var index = 0; index < collidersObj.Length; index++)
            {
                var colliderItem = collidersObj[index];
                colliderItem.enabled = false;
            }
            StartCoroutine(DeathAnaimator());
        };

        deadState.onFrame = delegate
        {
            //put death code here
            
        };

        deadState.onExit = delegate
        {

        };

        knockbackState.onEnter = delegate
        {
            StartCoroutine(Knockback());
        };

        knockbackState.onFrame = delegate
        {
            if (!isknockedBack)
            {
                fsm.TransitionTo("Chasing");
            }
        };

        knockbackState.onExit = delegate
        {
            
        };


    }

    public void SetPool(ObjectPool<EnemyAI> pool)
    {
        _pool = pool;
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    private void CompleteSpawning()
    {
        spawnComplete = true;
    }

    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(2);
        healthBarUI.SetActive(false);
    }

    IEnumerator DeathAnaimator()
    {
        anaimator.Play("EnemyDeath");
        yield return new WaitForSeconds(anaimator.GetCurrentAnimatorStateInfo(0).length);
        //drop XP
        Instantiate(xpList[XpToSpawn()], transform.position, Quaternion.identity);
        //drop coin
        CoinToSpawn();

        if (_pool != null)
            _pool.Release(this);
        else
            Destroy(gameObject);

    }

    public void CoinToSpawn()
    {
        coinDropChance = Random.Range(0,100);
        if (coinDropChance >= 40)
        {
            coinRange = Random.Range(0, 100);
            if (coinRange >= 0 && coinRange < 80)
            {
                Instantiate(coinList[0], transform.position, Quaternion.identity);
            }
            else if (coinRange >= 80 && coinRange < 90)
            {
                Instantiate(coinList[1], transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(coinList[2], transform.position, Quaternion.identity);
            }
        }
        
        
    }

    public int XpToSpawn()
    {
        xpRange = Random.Range(0, 100);
        if (xpRange >= 0 && xpRange <70)
        {
            return 0;
        }
        else if (xpRange >= 70 && xpRange < 90)
        {
            return 1;
        }
        else
        {
            return 2;
        }
        
    }

    IEnumerator SpawnAnaimator()
    {
        anaimator.Play("EnemySpawn");
        yield return new WaitForSeconds(anaimator.GetCurrentAnimatorStateInfo(0).length);
        CompleteSpawning();
    }

    IEnumerator Knockback()
    {
        if (rb != null)
        {
            isknockedBack = true;
            agent.enabled = false;
            rb.AddForce(knockbackDirection, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            agent.enabled = true;
            isknockedBack = false;
        }
    }

    public void DamagePlayer()
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
        OnKnockback(-transform.forward * 0.2f);
    }


    public void TakeDamage(float damage, Vector3 force)
    {
        StopCoroutine(HideHealthBar());
        healthBarUI.SetActive(true);
        health -= damage;
        StartCoroutine(HideHealthBar());
        healthBarSlider.value = CalculateHealth();

        if (health <= 0)
        {
            fsm.TransitionTo("Dead");
        }
        else
        {
            OnKnockback(force);
        }

    }

    public float CalculateHealth()
    {
        return health / maxHealth;
    }

   public void OnKnockback(Vector3 directionForce)
    {
        knockbackDirection = directionForce;
        fsm.TransitionTo("Knockback");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touch");
    }
   
}
