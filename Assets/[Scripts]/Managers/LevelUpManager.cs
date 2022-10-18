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
    public int rifleLvl = 0;
    public int shotgunLvl = 0;
    public int damageUpgradeLvl = 0;
    public int pickupUpgradeLvl = 0;
    public int xpUpgradeLvl = 0;
    public int movementUpgradeLvl = 0;
    public int healthUpgradeLvl = 0;
    public int recoveryUpgradeLvl = 0;
    public int projectileUpgradeLvl = 0;
    public int currentUpgrades = 0;
    public int maxUpgrades = 3;
    public int maxUpgradeLvl = 5;
    public int maxGunLvl = 8;
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
            upgradeMenu = GameObject.Find("ApplyUpgrade Menu");
            upgradeMenu.SetActive(false);
        }
        catch (NullReferenceException)
        {
            Debug.Log("No ApplyUpgrade Menu found in Scene");
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

        
        //upgrades.Add(new ApplyUpgrade("t", "t", null, 1, "rifle", 0, 0, 1, false, 0, 0, 0, false, 0));
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
        List<Upgrade> pool = UpdatePool();
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
        UpgradeManager.Instance.ApplyUpgrade(upgradeSelected);
        upgrades.Remove(upgradeSelected);
    }

    private List<Upgrade> UpdatePool()
    {
        List<Upgrade> pool = new();
        if (rifleLvl < maxGunLvl)
        {
            pool.Add(rifleUpgrades[rifleLvl]);
        }

        if (shotgunLvl < maxGunLvl)
        {
            pool.Add(shotgunUpgrades[shotgunLvl]);
        }

        if (currentUpgrades < maxUpgrades)
        {
            if (damageUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(damageUpgrades[damageUpgradeLvl]);
            }
            if (pickupUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(pickupUpgrades[pickupUpgradeLvl]);
            }
            if (xpUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(xpUpgrades[xpUpgradeLvl]);
            }
            if (movementUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(movementUpgrades[movementUpgradeLvl]);
            }
            if (healthUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(maxHealthUpgrades[healthUpgradeLvl]);
            }
            if (recoveryUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(recoveryUpgrades[recoveryUpgradeLvl]);
            }
            if (projectileUpgradeLvl < maxUpgradeLvl)
            {
                pool.Add(projectileUpgrades[projectileUpgradeLvl]);
            }
        }

        return pool;
    }

}
