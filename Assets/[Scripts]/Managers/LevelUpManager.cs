using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

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

    public float currentXp = 1;
    public float xpLeft;
    public float xpToLvl;
    public int startingLvl = 1;
    public int currentLvl;
    public float xpMultiplier = 1f;
    private GameObject upgradeMenu, xpMenu;
    public List<Upgrade> upgrades = new();
    [Header("Guns")]
    public List<Upgrade> rifleUpgrades = new();
    public List<Upgrade> shotgunUpgrades = new();
    [Header("Upgrades")]
    public List<Upgrade> damageUpgrades = new();
    public List<Upgrade> pickupUpgrades = new();
    public List<Upgrade> xpUpgrades = new();
    public List<Upgrade> movementUpgrades = new();
    public List<Upgrade> maxHealthUpgrades = new();
    public List<Upgrade> recoveryUpgrades = new();
    public List<Upgrade> projectileUpgrades = new();

    private void Start()
    {
        try
        {
            upgradeMenu = GameObject.Find("Upgrade Menu");
            upgradeMenu.SetActive(false);
        }
        catch (NullReferenceException)
        {
            Debug.Log("No Upgrade Menu found in Scene");
        }
        try
        {
            xpMenu = GameObject.Find("XP Menu");
            currentLvl = startingLvl;
            xpToLvl = Mathf.Pow(currentLvl + 1, 3);
            xpLeft = xpToLvl - currentXp;
            float percentageToLvl = (currentXp - Mathf.Pow(currentLvl, 3)) / (Mathf.Pow(currentLvl + 1, 3) - Mathf.Pow(currentLvl, 3));
            xpMenu.GetComponent<LevelUI>().UpdateXP(percentageToLvl, currentLvl);
        }
        catch (NullReferenceException)
        {
            Debug.Log("No XP Menu found in Scene");
        }

        
        //upgrades.Add(new Upgrade("t", "t", null, 1, "rifle", 0, 0, 1, false, 0, 0, 0, false, 0));
    }
    public void AddXp(float amount)
    {
        currentXp += amount * xpMultiplier;

        if (currentXp >= xpToLvl)
        {
            currentLvl++;
            xpToLvl = Mathf.Pow(currentLvl + 1, 3);
            UpdateMenu();
            xpMenu.SetActive(false);
            GameManager.Instance.Pause(upgradeMenu);
        }
        xpLeft = xpToLvl - currentXp;



        float percentageToLvl = (currentXp - Mathf.Pow(currentLvl, 3)) / (Mathf.Pow(currentLvl + 1, 3) - Mathf.Pow(currentLvl, 3));
        xpMenu.GetComponent<LevelUI>().UpdateXP(percentageToLvl, currentLvl);
        //Debug.Log($"Level: {currentLvl} \nXP Left: {xpLeft} \nCurrent XP: {currentXp} \nXP Needed for Level {xpToLvl}");
    }

    private void UpdateMenu()
    {
        List<Upgrade> pool = new();
        pool.AddRange(upgrades);

        int length = 3;
        if (upgrades.Count < 3)
            length = upgrades.Count;
        for (int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, pool.Count);
            Upgrade upgrade = pool[rand];
            upgradeMenu.GetComponent<UpgradeMenu>().UpdateUI(upgrade, i);
            pool.Remove(upgrade);
        }
    }

    public void UpgradeSelected(TextMeshProUGUI title)
    {
        Upgrade upgradeSelected = null;
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.upgradeName + " - " + upgrade.level == title.text)
            {
                upgradeSelected = upgrade;
                
            }
        }
        xpMenu.SetActive(true);
        GameManager.Instance.Resume(upgradeMenu);
        UpgradeManager.Instance.Upgrade(upgradeSelected);
        upgrades.Remove(upgradeSelected);
    }

    

}
