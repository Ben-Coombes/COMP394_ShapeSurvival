using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuInput : MonoBehaviour
{
    public static MenuInput Instance { get; private set; }
    private PlayerInput playerInput;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
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
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpgradeMenu()
    {

    }

    public void LevelMenu()
    {

    }

    public void HelpMenu()
    {

    }
}