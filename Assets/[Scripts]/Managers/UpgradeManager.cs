using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
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

    public Gun[] guns;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        guns = player.GetComponentsInChildren<Gun>();
        Upgrade test = new Upgrade(
            "t",
            "t", 
            null,
            1,
            "rifle",
            0,
            0,
            1,
            false,
            0,
            0,
            0,
            false,
            0);
        Upgrade(test);
    }
    public void Upgrade(Upgrade upgrade)
    {
        Gun gunToBeUpgraded = null;
        
        foreach (Gun gun in guns)
        {
            if (gun.name == upgrade.gunName)
                gunToBeUpgraded = gun;
        }
        if (gunToBeUpgraded == null)
            return;
        Bullet bulletToBeUpgraded = gunToBeUpgraded.bulletPrefab.GetComponent<Bullet>();

        //Gun Upgrades - fire rate increase, spread decrease, bullets per tap increase, manual to automatic
        gunToBeUpgraded.fireRate -= upgrade.fireRateIncrease;
        gunToBeUpgraded.spread -= upgrade.spreadDecrease;
        gunToBeUpgraded.bulletsPerTap += upgrade.bulletsIncrease;
        if (upgrade.manualToAuto)
            gunToBeUpgraded.automatic = true;

        //Bullet Upgrades - damage increase, knockback increase, max collisions/piercing
        bulletToBeUpgraded.damage += upgrade.damageIncrease;
        bulletToBeUpgraded.knockback += upgrade.knockbackIncrease;
        bulletToBeUpgraded.maxCollisions += upgrade.collisionsIncrease;

        //Niche Upgrades - Explode on impact, Explosion range
        bulletToBeUpgraded.range = upgrade.explosionRange;
        bulletToBeUpgraded.isBullet = !upgrade.explodeOnImpact;
    }
}




