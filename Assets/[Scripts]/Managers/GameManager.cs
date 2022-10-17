using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Pause(GameObject obj)
    {
        obj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 0f;
    }

    public void Resume(GameObject obj)
    {
        obj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 1f;
    }
}
