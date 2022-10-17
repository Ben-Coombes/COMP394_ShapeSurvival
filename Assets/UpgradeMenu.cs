using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public UpgradeUIHolder[] upgradesUI;

    public void UpdateUI(GunUpgrade upgrade, int i)
    {
        upgradesUI[i].image.sprite = upgrade.image;
        upgradesUI[i].title.text = upgrade.title;
        upgradesUI[i].description.text = upgrade.description;
    }

    public void OnUpgradePressed(TextMeshProUGUI text)
    {
        LevelUpManager.Instance.UpgradeSelected(text);
    }
}
