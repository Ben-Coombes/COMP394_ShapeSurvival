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

    //chasing

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform; //find the player in the scene
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        fsm = new FiniteStateMachine();

        //var patrolingState = fsm.CreateState("Patroling");
        //var wanderingState = fsm.CreateState("Wandering");
        var spawningState = fsm.CreateState("Spawning");
        var AttackingState = fsm.CreateState("Attacking");
        var ChasingState = fsm.CreateState("Chasing");

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

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

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

   

    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
