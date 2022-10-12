using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private PlayerInput.PlayerMovementActions playerMovement;

    private PlayerController playerController;

    private PlayerLook playerLook;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        playerMovement = playerInput.PlayerMovement;
        playerController = GetComponent<PlayerController>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement.Jump.performed += ctx => playerController.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerController.Move(playerMovement.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        playerLook.Look(playerMovement.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerMovement.Enable();
    }
    private void OnDisable()
    {
        playerMovement.Disable();
    }
}
