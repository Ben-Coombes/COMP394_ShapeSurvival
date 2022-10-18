using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Upgrade
{
    public enum UpgradeType
    {
        None,
        Rifle,
        Shotgun,
        Damage,
        Pickup,
        Xp,
        Movement,
        MaxHealth,
        Recovery,
        Armour,
        Projectile
    }
    public UpgradeType _upgradeType;
    [Header("UI")]
    [ConditionalField("_upgradeType", true, UpgradeType.None)] public string description;
    [ConditionalField("_upgradeType", true, UpgradeType.None)] public Sprite image;
    [Header("Upgrade Values")]
    [ConditionalField("_upgradeType", true, UpgradeType.None)] public int level;
    [ConditionalField("_upgradeType", true, UpgradeType.None)] public string upgradeName;
    [ConditionalField("_upgradeType", false, UpgradeType.Damage)] public float uDamageIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Pickup)] public float pickupRangeIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Xp)] public float xpMultiplierIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Movement)] public float movementSpeedIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.MaxHealth)] public float healthIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Recovery)] public float healthRecoveryIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Armour)] public float armourIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Projectile)] public float projectileIncrease;
    //gun
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public float fireRateIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public float spreadDecrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public int bulletsIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public bool manualToAuto;
    //bullet
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public float gDamageIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public float knockbackIncrease;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public int collisionsIncrease;
    //optional stuff
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public bool explodeOnImpact;
    [ConditionalField("_upgradeType", false, UpgradeType.Rifle, UpgradeType.Shotgun)] public float explosionRange;

}
