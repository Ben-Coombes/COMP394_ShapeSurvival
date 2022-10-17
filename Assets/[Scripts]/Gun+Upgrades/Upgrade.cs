using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Upgrade
{
    [Header("UI")] 
    public string title;
    public string description;
    public Sprite image;
    [Range(1,10)]
    public int level;
    [Header("Gun")]
    public string gunName;

    [Header("Gun Values")] 
    public float fireRateIncrease;
    public float spreadDecrease;
    public int bulletsIncrease;
    public bool manualToAuto;

    [Header("Bullet Values")] 
    public float damageIncrease;
    public float knockbackIncrease;
    public int collisionsIncrease;

    [Header("Other Values")] 
    public bool explodeOnImpact;
    public float explosionRange;

    public Upgrade(string title, string description, Sprite image, int level, string gunName, float fireRateIncrease, float spreadDecrease, int bulletsIncrease, bool manualToAuto, float damageIncrease, float knockbackIncrease, int collisionsIncrease, bool explodeOnImpact, float explosionRange)
    {
        this.title = title;
        this.description = description;
        this.image = image;
        this.level = level;
        this.gunName = gunName;
        this.fireRateIncrease = fireRateIncrease;
        this.spreadDecrease = spreadDecrease;
        this.bulletsIncrease = bulletsIncrease;
        this.manualToAuto = manualToAuto;
        this.damageIncrease = damageIncrease;
        this.knockbackIncrease = knockbackIncrease;
        this.collisionsIncrease = collisionsIncrease;
        this.explodeOnImpact = explodeOnImpact;
        this.explosionRange = explosionRange;
    }

}
