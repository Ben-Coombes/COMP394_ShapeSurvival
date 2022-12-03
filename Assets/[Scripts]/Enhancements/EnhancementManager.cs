using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class EnhancementManager : MonoBehaviour, IDataPersistence
{
    public static EnhancementManager Instance { get; private set; }

    public List<Enhancement> unlockedEnhancements = new();

    public float damageMultiplier;
    public float pickupRangeIncrease;
    public float xpMultiplier;
    public float speedIncrease;
    public float healthIncrease;
    public float recoveryMultiplier;
    public float projectileIncrease;
    public float armourMultiplier;

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
    void Start()
    {
        damageMultiplier = 1;
        pickupRangeIncrease = 0;
        xpMultiplier = 1;
        speedIncrease = 1;
        healthIncrease = 0;
        recoveryMultiplier = 1;
        projectileIncrease = 0;
        armourMultiplier = 1;
        foreach (Enhancement enh in unlockedEnhancements)
        {
            ApplyEnhancement(enh);
        }
    }

    public void ApplyEnhancement(Enhancement enh)
    {
        switch (enh._EnhancementType)
        {
            case Enhancement.EnhancementType.Damage:
                damageMultiplier += enh.damageIncrease;
                break;
            case Enhancement.EnhancementType.Pickup:
                pickupRangeIncrease += enh.pickupRangeIncrease;
                break;
            case Enhancement.EnhancementType.Xp:
                xpMultiplier += enh.xpMultiplierIncrease;
                break;
            case Enhancement.EnhancementType.Movement:
                speedIncrease += enh.movementSpeedIncrease;
                break;
            case Enhancement.EnhancementType.MaxHealth:
                healthIncrease += enh.healthIncrease;
                break;
            case Enhancement.EnhancementType.Recovery:
                recoveryMultiplier += enh.healthRecoveryIncrease;
                break;
            case Enhancement.EnhancementType.Armour:
                armourMultiplier -= enh.armourIncrease;
                break;
            case Enhancement.EnhancementType.Projectile:
                projectileIncrease += enh.projectileIncrease;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void LoadData(GameData data)
    {
        this.unlockedEnhancements = data.unlockedEnhancements;
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedEnhancements = this.unlockedEnhancements;
    }
}
