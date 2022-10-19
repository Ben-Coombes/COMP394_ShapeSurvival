using System;
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

    private Gun[] guns;
    private GameObject player;
    public float damageMultiplier; //
    public float pickupRangeIncrease; //
    public float xpMultiplier; //
    public float speedIncrease; //
    public float healthIncrease; //
    public float recoveryMultiplier; //
    public float projectileIncrease; //
    public float armourMultiplier;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        guns = player.GetComponentsInChildren<Gun>();
        damageMultiplier = 1;
        pickupRangeIncrease = 0;
        xpMultiplier = 1;
        speedIncrease = 0;
        healthIncrease = 0;
        recoveryMultiplier = 1;
        projectileIncrease = 0;
        armourMultiplier = 1;
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade._upgradeType is Upgrade.UpgradeType.Rifle or Upgrade.UpgradeType.Shotgun)
        {
            Gun gunToBeUpgraded = null;

            foreach (Gun gun in guns)
            {
                if (gun.name == upgrade.upgradeName)
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
            bulletToBeUpgraded.damage += upgrade.gDamageIncrease;
            bulletToBeUpgraded.knockback += upgrade.knockbackIncrease;
            bulletToBeUpgraded.maxCollisions += upgrade.collisionsIncrease;

            //Niche Upgrades - Explode on impact, Explosion range
            bulletToBeUpgraded.range = upgrade.explosionRange;
            bulletToBeUpgraded.isBullet = !upgrade.explodeOnImpact;
        }
        else
        {
            switch (upgrade._upgradeType)
            {
                case Upgrade.UpgradeType.Damage:
                    damageMultiplier += upgrade.uDamageIncrease;
                    break;
                case Upgrade.UpgradeType.Pickup:
                    pickupRangeIncrease += upgrade.pickupRangeIncrease;
                    break;
                case Upgrade.UpgradeType.Xp:
                    xpMultiplier += upgrade.xpMultiplierIncrease;
                    break;
                case Upgrade.UpgradeType.Movement:
                    speedIncrease += upgrade.movementSpeedIncrease;
                    break;
                case Upgrade.UpgradeType.MaxHealth:
                   player.GetComponent<PlayerController>().IncreaseMaxHealth(upgrade.healthIncrease);
                    break;
                case Upgrade.UpgradeType.Recovery:
                    recoveryMultiplier += upgrade.healthRecoveryIncrease;
                    break;
                case Upgrade.UpgradeType.Armour:
                    armourMultiplier -= upgrade.armourIncrease;
                    break;
                case Upgrade.UpgradeType.Projectile:
                    foreach (Gun gun in guns)
                    {
                        gun.bulletsPerTap += upgrade.projectileIncrease;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}




