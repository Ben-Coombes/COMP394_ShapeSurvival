using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float health;
    public float maxHealth = 100f;
    public float recoveryRate;

    public float jumpHeight = 3f;

    public float pickupRange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        health = maxHealth;
        recoveryRate = 0.2f;
        StartCoroutine(RestoreHealth());
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        controller.Move(transform.TransformDirection(moveDir) * (speed + UpgradeManager.Instance.speedIncrease) * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
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
        Destroy(this);
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
