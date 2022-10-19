using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{

    public GameObject xpBarUI;
    public Image xpBarSlider;

   

    int nextLevel;

    public void UpdateXP(float xp, int level)
    {
        nextLevel = level + 1;
        xpBarSlider.fillAmount = xp;

        
    }
}
