using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("OnMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
