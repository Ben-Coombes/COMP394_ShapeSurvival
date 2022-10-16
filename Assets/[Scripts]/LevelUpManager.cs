using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance { get; private set; }
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

    public float currentXp = 0;
    public float XpLeft;
    public float XpToLvl;
    public int startingLvl = 1;
    public int currentLvl;
    public float XpMultiplier = 1f;
    public GameObject upgradeMenu;

    private void Start()
    {
        upgradeMenu = GameObject.Find("Upgrade Menu");
        upgradeMenu.SetActive(false);
        currentLvl = startingLvl;

        XpToLvl = Mathf.Pow(currentLvl + 1, 3);
        XpLeft = XpToLvl - currentXp;
    }
    public void AddXp(float amount)
    {
        currentXp += amount * XpMultiplier;

        if (currentXp >= XpToLvl)
        {
            currentLvl++;
            XpToLvl = Mathf.Pow(currentLvl + 1, 3);
            Pause();
        }
        XpLeft = XpToLvl - currentXp;

        Debug.Log($"Level: {currentLvl} \nXP Left: {XpLeft} \nCurrent XP: {currentXp} \nXP Needed for Level {XpToLvl}");

    }

    public void Pause()
    {
        upgradeMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        upgradeMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 1f;
    }

}
