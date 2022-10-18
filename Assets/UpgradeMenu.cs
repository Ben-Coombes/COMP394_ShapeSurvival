using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

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

    public void HideUpgradeButtons(int length)
    {
        for (int i = 3; i > length; i--)
        {
            GameObject container = upgradesUI[i - 1].image.rectTransform.parent.gameObject;
            //Navigation navigation = new Navigation();
            //navigation.mode = Navigation.Mode.None;
            //container.GetComponent<Button>().navigation = navigation;
            Destroy(container);
        }
    }
}
