using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }
    public int currentCoins;
    public int totalCoins;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    private void Start()
    {
        currentCoins = 0;
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

    public void IncreaseCoins(int amount)
    {
        currentCoins += amount;
        totalCoins += amount;
    }

    public void LoadData(GameData data)
    {
        this.totalCoins = data.totalCoins;
    }

    public void SaveData(ref GameData data)
    {
        data.totalCoins = this.totalCoins;
    }
}
