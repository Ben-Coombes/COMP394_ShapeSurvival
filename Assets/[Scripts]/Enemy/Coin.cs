using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform player;

    public float acceleration;
    public float coinMovementSpeed;

    public int coinAmount;

    public float pickupRange;
    public LayerMask whatIsPlayer;

    public bool enteredPickupRange = false;

    [Header("Anaimation")]
    public Animator anaimator;
    
    void Start()
    {
        //anaimator.Play("Coin_Spin");
        player = GameObject.FindGameObjectWithTag("Player").transform; //find the player in the scene
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CheckIfInPickupRange() || enteredPickupRange)
        {
            coinMovementSpeed += Time.deltaTime * acceleration;
            transform.position = Vector3.MoveTowards(transform.position, player.position, coinMovementSpeed);
        }
    }

    private bool CheckIfInPickupRange()
    {
        if (Physics.CheckSphere(transform.position, player.GetComponent<PlayerController>().pickupRange + UpgradeManager.Instance.pickupRangeIncrease + EnhancementManager.Instance.pickupRangeIncrease, whatIsPlayer))
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
            CollectCoin();
        }
    }

    public void CollectCoin()
    {
        GameManager.Instance.IncreaseCoins(coinAmount);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //draw ranges on screen
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
