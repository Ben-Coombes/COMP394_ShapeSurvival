using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public UpgradeUIHolder[] upgradesUI;

    public void UpdateUI(Upgrade upgrade, int i)
    {
        upgradesUI[i].image.sprite = upgrade.image;
        upgradesUI[i].title.text = upgrade.upgradeName + " - " + upgrade.level;
        upgradesUI[i].description.text = upgrade.description;
    }

    public void OnUpgradePressed(TextMeshProUGUI text)
    {
        LevelUpManager.Instance.UpgradeSelected(text);
    }
}
