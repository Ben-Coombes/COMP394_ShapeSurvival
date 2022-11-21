using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[Serializable]
public class Enhancement
{
    public enum EnhancementType
    {
        None,
        Damage,
        Pickup,
        Xp,
        Movement,
        MaxHealth,
        Recovery,
        Armour,
        Projectile
    }
    public EnhancementType _EnhancementType;

    [Header("ApplyUpgrade Values")]
    [ConditionalField("_EnhancementType", true, Enhancement.EnhancementType.None)] public int level;
    [ConditionalField("_EnhancementType", true, Enhancement.EnhancementType.None)] public bool isUnlocked;
    [ConditionalField("_EnhancementType", true, Enhancement.EnhancementType.None)] public int cost;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Damage)] public float damageIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Pickup)] public float pickupRangeIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Xp)] public float xpMultiplierIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Movement)] public float movementSpeedIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.MaxHealth)] public float healthIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Recovery)] public float healthRecoveryIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Armour)] public float armourIncrease;
    [ConditionalField("_EnhancementType", false, Enhancement.EnhancementType.Projectile)] public float projectileIncrease;
}
