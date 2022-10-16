using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
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
        }
        XpLeft = XpToLvl - currentXp;

        Debug.Log($"Level: {currentLvl} \nXP Left: {XpLeft} \nCurrent XP: {currentXp} \nXP Needed for Level {XpToLvl}");

    }

}
