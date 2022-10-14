using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private PlayerInputActions playerInputActions;
    private PlayerInputActions.OnGroundActions onGround;

    private PlayerController playerController;

    private PlayerLook playerLook;

    [SerializeField] private PlayerGunController playerGunController;
    // Start is called before the first frame update
    void Awake()
    {
        playerGunController = GetComponent<PlayerGunController>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        onGround = playerInputActions.OnGround;
        playerController = GetComponent<PlayerController>();
        playerLook = GetComponent<PlayerLook>();
        onGround.Jump.performed += ctx => playerController.Jump();
        onGround.SwitchWeapon.performed += ctx => playerGunController.SwitchGun();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        playerController.Move(onGround.Movement.ReadValue<Vector2>());
        playerGunController.currentGun.CheckInput(onGround.Fire);
    }

    void LateUpdate()
    {
        playerLook.Look(onGround.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onGround.Enable();
    }
    private void OnDisable()
    {
        onGround.Disable();
    }
}
