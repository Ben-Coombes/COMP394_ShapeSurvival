using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    FiniteStateMachine fsm;

    public LayerMask whatIsGround, whatIsPlayer;

    public bool spawnComplete = false;

    //chasing

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //death
    public bool isDead = false;

    //health
    public float health = 100;

    //knockback
    public bool isknockedBack = false;
    private Vector3 knockbackDirection;

    public Rigidbody rb;

    private void Awake()
    {
        player = GameObject.Find("Player").transform; //find the player in the scene
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();
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
            Invoke("CompleteSpawning", 1);

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

            if (CheckIfInAttackRange())
            {
                fsm.TransitionTo("Attacking");
            }
            if (CheckIfDead())
            {
                fsm.TransitionTo("Dead");
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
            if (CheckIfDead())
            {
                fsm.TransitionTo("Dead");
            }
        };

        AttackingState.onExit = delegate
        {
            print("Exited Attacking State!");
        };

        deadState.onEnter = delegate
        {
            print("Entered Dead State!");
        };

        deadState.onFrame = delegate
        {
            //put death code here
            print("ENEMY IS DEAD");
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

    public bool CheckIfDead()
    {
        if (isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CompleteSpawning()
    {
        spawnComplete = true;
    }

    IEnumerator Knockback()
    {
        isknockedBack = true;
        agent.enabled = false;
        rb.AddForce(knockbackDirection, ForceMode.Impulse);
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector3.zero;
        agent.enabled = true;
        isknockedBack = false;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 rot = Quaternion.LookRotation(player.position - transform.position).eulerAngles;
        rot.x = rot.z = 0;
        transform.rotation = Quaternion.Euler(rot);

        
        

        if (!alreadyAttacked)
        {
            OnKnockback(-transform.forward * 3);
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
        health -= damage;
        if (health <= 0)
        {
            fsm.TransitionTo("Dead");
        }
        else
        {
            OnKnockback(force);
        }

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
