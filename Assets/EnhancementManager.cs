using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancementManager : MonoBehaviour
{
    public List<Upgrade> damageEnhList = new();
    public List<Upgrade> pickupEnhList = new();
    public List<Upgrade> xpEnhList = new();
    public List<Upgrade> movementEnhList = new();
    public List<Upgrade> maxHealthEnhList = new();
    public List<Upgrade> recoveryEnhList = new();
    public List<Upgrade> projectileEnhList = new();
    public List<Upgrade> armourEnhList = new();
    public static EnhancementManager Instance { get; private set; }

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
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

}
