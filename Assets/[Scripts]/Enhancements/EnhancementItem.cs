using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnhancementItem : MonoBehaviour
{
    public int currentLevel = 0;
    public Enhancement.EnhancementType enhancementType;
    public List<Enhancement> enhancementList = new();
    public List<Toggle> toggleBoxes = new();
    private UnityEvent enhancementPurchased;

    private void Awake()
    {
        
    }
    private void Start()
    {
        foreach (Enhancement enh in EnhancementManager.Instance.unlockedEnhancements)
        {
            if (enh._EnhancementType == enhancementType)
            {
                enhancementList.ElementAt(enh.level - 1).isUnlocked = true;
            }
        }
        foreach (Enhancement enh in enhancementList)
        {
            toggleBoxes.ElementAt(enh.level - 1).isOn = enh.isUnlocked;
        }

        foreach (Toggle toggleBox in toggleBoxes)
        {
            if (toggleBox.isOn)
            {
                currentLevel++;
            }
        }
    }
    public void OnButtonClicked()
    {
        if (currentLevel < toggleBoxes.Count)
        {
            Enhancement enhToBeUnlocked = enhancementList.ElementAt(currentLevel);
            int totalUnlocked = EnhancementManager.Instance.unlockedEnhancements.Count;
            int newCost = enhToBeUnlocked.cost * (1 + currentLevel) + 20 * (int)Mathf.Pow(1.1f, totalUnlocked - 1);
            if (newCost <= GameManager.Instance.totalCoins)
            {
                GameManager.Instance.totalCoins -= newCost;
                enhToBeUnlocked.isUnlocked = true;
                toggleBoxes.ElementAt(enhToBeUnlocked.level - 1).isOn = true;
                currentLevel++;
                EnhancementManager.Instance.unlockedEnhancements.Add(enhToBeUnlocked);
                EnhancementManager.Instance.ApplyEnhancement(enhToBeUnlocked);
            }
        }
    }
}
