using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnhancementItem : MonoBehaviour
{
    public int currentLevel = 0;
    public List<Enhancement> enhancementList = new();
    public Enhancement.EnhancementType enhancementType;
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
            if (enhToBeUnlocked.cost <= GameManager.Instance.totalCoins)
            {
                GameManager.Instance.totalCoins -= enhToBeUnlocked.cost;
                enhToBeUnlocked.isUnlocked = true;
                toggleBoxes.ElementAt(enhToBeUnlocked.level - 1).isOn = true;
                currentLevel++;
                EnhancementManager.Instance.unlockedEnhancements.Add(enhToBeUnlocked);
                EnhancementManager.Instance.ApplyEnhancement(enhToBeUnlocked);
            }
        }
    }
}
