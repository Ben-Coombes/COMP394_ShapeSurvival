using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public int armourUpgradeLvl;
    public int currentUpgrades = 0;
    public int maxUpgrades = 3;
    public int maxUpgradeLvl;
    public int maxGunLvl;
    private GameObject upgradeMenu, xpMenu;
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
    public List<Upgrade> armourUpgrades = new();
    private List<Upgrade> allUpgrades = new();

    private void Start()
    {
        currentXp = 1;

        startingLvl = 1;
        rifleLvl = 0;
        shotgunLvl = 0;
        damageUpgradeLvl = 0;
        pickupUpgradeLvl = 0;
        xpUpgradeLvl = 0;
        movementUpgradeLvl = 0;
        healthUpgradeLvl = 0;
        recoveryUpgradeLvl = 0;
        projectileUpgradeLvl = 0;
        currentUpgrades = 0;
        maxUpgrades = 3;
        try
        {
            upgradeMenu = GameObject.Find("Upgrade Menu");
            upgradeMenu.SetActive(false);
        }
        catch (NullReferenceException)
        {
            Debug.Log("Upgrade Menu found in Scene");
        }
        try
        {
            xpMenu = GameObject.Find("PlayerUI");
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

        maxGunLvl = rifleUpgrades.Count;
        maxUpgradeLvl = damageUpgrades.Count;
        allUpgrades = rifleUpgrades.Concat(shotgunUpgrades).Concat(damageUpgrades).Concat(pickupUpgrades).Concat(xpUpgrades).Concat(movementUpgrades).Concat(maxHealthUpgrades).Concat(recoveryUpgrades).Concat(projectileUpgrades).Concat(armourUpgrades).ToList();

        //upgrades.Add(new ApplyUpgrade("t", "t", null, 1, "rifle", 0, 0, 1, false, 0, 0, 0, false, 0));
    }
    public void AddXp(float amount)
    {
        currentXp += amount * (UpgradeManager.Instance.xpMultiplier + EnhancementManager.Instance.xpMultiplier);

        if (currentXp >= xpToLvl)
        {
            currentLvl++;
            xpToLvl = Mathf.Pow(currentLvl + 1, 3);
            UpdateMenu();
            EventSystem.current.SetSelectedGameObject(upgradeMenu.GetComponent<UpgradeMenu>().upgradesUI[0].image.rectTransform.parent.gameObject);
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
        if (pool.Count < 3)
            length = pool.Count;
        upgradeMenu.GetComponent<UpgradeMenu>().HideUpgradeButtons(length);
        Debug.Log(length);
        for (int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, pool.Count);
            Upgrade upgrade = pool[rand];
            upgradeMenu.GetComponent<UpgradeMenu>().UpdateUI(upgrade, i);
            pool.Remove(upgrade);
        }

        
        //if length is 1 or 2 hide the second/third update container (needs to be added)
    }

    public void UpgradeSelected(TextMeshProUGUI title)
    {
        Upgrade upgradeSelected = null;
        foreach (Upgrade upgrade in allUpgrades)
        {
            if (upgrade.upgradeName + " - " + upgrade.level == title.text)
            {
                upgradeSelected = upgrade;
            }
        }
        xpMenu.SetActive(true);
        UpdateCurrentUpgrades(upgradeSelected);
        UpgradeManager.Instance.ApplyUpgrade(upgradeSelected);
        GameManager.Instance.Resume(upgradeMenu);
    }

    private void UpdateCurrentUpgrades(Upgrade upgrade)
    {
        switch (upgrade._upgradeType)
        {
            case Upgrade.UpgradeType.None:
                break;
            case Upgrade.UpgradeType.Rifle:
                rifleLvl++;
                break;
            case Upgrade.UpgradeType.Shotgun:
                shotgunLvl++;
                break;
            case Upgrade.UpgradeType.Damage:
                if (damageUpgradeLvl == 0)
                    currentUpgrades++;
                damageUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Pickup:
                if (pickupUpgradeLvl == 0)
                    currentUpgrades++;
                pickupUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Xp:
                if (xpUpgradeLvl == 0)
                    currentUpgrades++;
                xpUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Movement:
                if (movementUpgradeLvl == 0)
                    currentUpgrades++;
                movementUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.MaxHealth:
                if (healthUpgradeLvl == 0)
                    currentUpgrades++;
                healthUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Recovery:
                if (recoveryUpgradeLvl == 0)
                    currentUpgrades++;
                recoveryUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Armour:
                if (armourUpgradeLvl == 0)
                    currentUpgrades++;
                armourUpgradeLvl++;
                break;
            case Upgrade.UpgradeType.Projectile:
                if (projectileUpgradeLvl == 0)
                    currentUpgrades++;
                projectileUpgradeLvl++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<Upgrade> UpdatePool()
    {
        List<Upgrade> pool = new();
        pool.Clear();
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
            if (damageUpgradeLvl < damageUpgrades.Count)
            {
                pool.Add(damageUpgrades[damageUpgradeLvl]);
            }
            if (pickupUpgradeLvl < pickupUpgrades.Count)
            {
                pool.Add(pickupUpgrades[pickupUpgradeLvl]);
            }
            if (xpUpgradeLvl < xpUpgrades.Count)
            {
                pool.Add(xpUpgrades[xpUpgradeLvl]);
            }
            if (movementUpgradeLvl < movementUpgrades.Count)
            {
                pool.Add(movementUpgrades[movementUpgradeLvl]);
            }
            if (healthUpgradeLvl < maxHealthUpgrades.Count)
            {
                pool.Add(maxHealthUpgrades[healthUpgradeLvl]);
            }
            if (recoveryUpgradeLvl < recoveryUpgrades.Count)
            {
                pool.Add(recoveryUpgrades[recoveryUpgradeLvl]);
            }
            if (projectileUpgradeLvl < projectileUpgrades.Count)
            {
                pool.Add(projectileUpgrades[projectileUpgradeLvl]);
            }
            if (armourUpgradeLvl < armourUpgrades.Count)
            {
                pool.Add(armourUpgrades[armourUpgradeLvl]);
            }
        }
        else
        {
            if (damageUpgradeLvl > 0 && damageUpgradeLvl < damageUpgrades.Count)
            {
                pool.Add(damageUpgrades[damageUpgradeLvl]);
            }
            if (pickupUpgradeLvl > 0 && pickupUpgradeLvl < pickupUpgrades.Count)
            {
                pool.Add(pickupUpgrades[pickupUpgradeLvl]);
            }
            if (xpUpgradeLvl > 0 && xpUpgradeLvl < xpUpgrades.Count)
            {
                pool.Add(xpUpgrades[xpUpgradeLvl]);
            }
            if (movementUpgradeLvl > 0 && movementUpgradeLvl < movementUpgrades.Count)
            {
                pool.Add(movementUpgrades[movementUpgradeLvl]);
            }
            if (healthUpgradeLvl > 0 && healthUpgradeLvl < maxHealthUpgrades.Count)
            {
                pool.Add(maxHealthUpgrades[healthUpgradeLvl]);
            }
            if (recoveryUpgradeLvl > 0 && recoveryUpgradeLvl < recoveryUpgrades.Count)
            {
                pool.Add(recoveryUpgrades[recoveryUpgradeLvl]);
            }
            if (projectileUpgradeLvl > 0 && projectileUpgradeLvl < projectileUpgrades.Count)
            {
                pool.Add(projectileUpgrades[projectileUpgradeLvl]);
            }
            if (armourUpgradeLvl > 0 && armourUpgradeLvl < armourUpgrades.Count)
            {
                pool.Add(armourUpgrades[armourUpgradeLvl]);
            }
        }
        

        return pool;
    }

}
