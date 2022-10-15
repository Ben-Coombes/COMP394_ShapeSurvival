using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Gun[] guns;
    public GameObject player;
    public List<GunUpgrade> upgrades = new();


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        guns = player.GetComponentsInChildren<Gun>();
        GunUpgrade test = new GunUpgrade(
            "t",
            "t",
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
    public void Upgrade(GunUpgrade upgrade)
    {
        Gun gunToBeUpgraded = null;
        
        upgrades.Remove(upgrade);
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




