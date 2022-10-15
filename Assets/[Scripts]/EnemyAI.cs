using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    FiniteStateMachine fsm;

    public LayerMask whatIsGround, whatIsPlayer;

    public bool spawnComplete = false;

    //spawning
    public bool enemySpawned = false;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //death
    public bool isDead = false;

    //health
    public float health;
    public float maxHealth;

    public GameObject healthBarUI;
    public Slider healthBarSlider; 

    //knockback
    public bool isknockedBack = false;
    private Vector3 knockbackDirection;

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
            print("Entered Spawning State!");
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
            print("Exited Spawning State!");
        };

        //chasing state
        ChasingState.onEnter = delegate
        {
            print("Entered Chasing State!");
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
            print("Exited Chasing State!");
        };

        //attack state
        AttackingState.onEnter = delegate
        {
            print("Entered Attacking State!");
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
            print("Exited Attacking State!");
        };

        deadState.onEnter = delegate
        {
            print("Entered Dead State!");
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
            print("Exited Dead State!");
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
            print("Exited Knockback State!");
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
            Debug.Log("Enemy has attacked player!");


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

        Debug.Log("Enemy Hit");
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
