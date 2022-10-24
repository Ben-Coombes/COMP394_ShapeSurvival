using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour
{
    public static MenuInput Instance { get; private set; }
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private PlayerInputActions.OnGroundActions onGround;
    private PlayerInputActions.OnMenuActions onMenu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("OnMenu");
    }

    public void PlayGame()
    {

    }
}