using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    [Header("Movement")]
    private float speed = 5f;
    public float walkSpeed;
    public float sprintSpeed;
    public float gravity = -9.8f;
    public float groundDrag;
    public float airSpeedMultiplier;
    [Header("Player Stats")]
    public float health;
    public float maxHealth = 100f;
    public float recoveryRate;
    [Header("Ground Check")]
    private bool isGrounded;
    public LayerMask whatIsGround;
    public float playerHeight;
    [Header("Crouching")] 
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public bool isCrouching;




    private Transform playerCam;
    private Rigidbody rb;

    public float jumpHeight = 3f;

    public float pickupRange = 5f;
    public bool isSprinting;

    public MovementState currentState;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        health = maxHealth;
        recoveryRate = 0.2f;
        StartCoroutine(RestoreHealth());

        startYScale = transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        UpdateState();
        SpeedControl();

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void UpdateState()
    {
        if (isCrouching)
        {
            currentState = MovementState.crouching;
            speed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        if (isGrounded && isSprinting)
        {
            currentState = MovementState.sprinting;
            speed = sprintSpeed;
        } else if(isGrounded)

        {
            currentState = MovementState.walking;
            speed = walkSpeed;
        }
        else
        {
            currentState = MovementState.air;
        }
    }
    public void Move(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir = playerCam.forward * input.y + playerCam.right * input.x;
        if(isGrounded)
            rb.AddForce(moveDir * (speed + UpgradeManager.Instance.speedIncrease) * 25f, ForceMode.Force);
        else if (!isGrounded)
            rb.AddForce(moveDir * (speed + UpgradeManager.Instance.speedIncrease) * 25f * airSpeedMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (vel.magnitude > speed)
        {
            Vector3 controlVel = vel.normalized * (speed + UpgradeManager.Instance.speedIncrease);

            rb.velocity = new Vector3(controlVel.x, rb.velocity.y, controlVel.z);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage * UpgradeManager.Instance.armourMultiplier;
        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        SceneManager.LoadScene(2);
        
    }

    private IEnumerator RestoreHealth()
    {
        while (true)
        {
            if (health < maxHealth)
            {
                health += recoveryRate * UpgradeManager.Instance.recoveryMultiplier;

                if (health > maxHealth)
                {
                    health = maxHealth;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
    }
}
