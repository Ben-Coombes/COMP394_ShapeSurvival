using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    
    public Transform player;

    public float xpMovementSpeed;
    public float acceleration;

    public float pickupRange;
    public LayerMask whatIsPlayer;

    public bool enteredPickupRange = false; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform; //find the player in the scene

        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfInPickupRange() || enteredPickupRange)
        {
            xpMovementSpeed += Time.deltaTime * acceleration;
            transform.position = Vector3.MoveTowards(transform.position, player.position, xpMovementSpeed);
        }
        
    }

    private bool CheckIfInPickupRange()
    {
        if (Physics.CheckSphere(transform.position, player.GetComponent<PlayerController>().pickupRange, whatIsPlayer))
        {
            enteredPickupRange = true;
            return true;
        }
        return false;
        
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CollectXP();
        }
    }

    public void CollectXP()
    {
        //collect XP code goes here
        Destroy(this.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

}
