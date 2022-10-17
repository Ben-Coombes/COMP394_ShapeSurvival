using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("Unity")]
    public NavMeshAgent agent;
    public Transform player;
    FiniteStateMachine fsm;
    public GameObject xp;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Spawning/Death")]
    public bool spawnComplete = false;
    public bool enemySpawned = false;
    //death
    public bool isDead = false;
    private Vector3 deathPosition;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float attackRange;
    public bool playerInAttackRange;

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

    

    private void Awake()
    {
        player = GameObject.Find("Player").transform; //find the player in the scene
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        healthBarUI.SetActive(false);
    }

    private void Start()
    {
        fsm = new FiniteStateMachine();
       

        //var patrolingState = fsm.CreateState("Patroling");
        //var wanderingState = fsm.CreateState("Wandering");
        var spawningState = fsm.CreateState("Spawning");
        var AttackingState = fsm.CreateState("Attacking");
        var ChasingState = fsm.CreateState("Chasing");
        var deadState = fsm.CreateState("Dead");
        var knockbackState = fsm.CreateState("Knockback");

        spawningState.onEnter = delegate
        {
            
        };

        spawningState.onFrame = delegate
        {
            StartCoroutine(SpawnAnaimator());

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
            agent.SetDestination(player.position);
            anaimator.Play("EnemyIdle2");
            if (CheckIfInAttackRange())
            {
                fsm.TransitionTo("Attacking");
            }
            
        };

        ChasingState.onExit = delegate
        {
           
        };

        //attack state
        AttackingState.onEnter = delegate
        {
            
        };

        AttackingState.onFrame = delegate
        {
            AttackPlayer();
            if (!CheckIfInAttackRange())
            {
                fsm.TransitionTo("Chasing");
            }
            
        };

        AttackingState.onExit = delegate
        {
            
        };

        deadState.onEnter = delegate
        {
            deathPosition = transform.position;
            //disable everything 
            healthBarUI.SetActive(false);
            agent.enabled = false;
            rb.detectCollisions = false;
            Destroy(GetComponentInChildren<Rigidbody>());
            var collidersObj = gameObject.GetComponentsInChildren<Collider>();
            for (var index = 0; index < collidersObj.Length; index++)
            {
                var colliderItem = collidersObj[index];
                colliderItem.enabled = false;
            }
        };

        deadState.onFrame = delegate
        {
            //put death code here
            StartCoroutine(DeathAnaimator());
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
            }
        };

        knockbackState.onExit = delegate
        {
            
        };


    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    private bool CheckIfInAttackRange()
    {
        return Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    private void CompleteSpawning()
    {
        spawnComplete = true;
    }

    IEnumerator DeathAnaimator()
    {
        anaimator.Play("EnemyDeath");
        yield return new WaitForSeconds(anaimator.GetCurrentAnimatorStateInfo(0).length);
        //drop item
        Instantiate(xp, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
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

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 rot = Quaternion.LookRotation(player.position - transform.position).eulerAngles;
        rot.x = rot.z = 0;
        transform.rotation = Quaternion.Euler(rot);

        
        

        if (!alreadyAttacked)
        {
            
            //attack code goes under here


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //call reset attack function on delay
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        healthBarUI.SetActive(true);
        health -= damage;
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

    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
