using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{

    public GameObject xpBarUI;
    public Slider xpBarSlider;

    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI NextLevelText;

    int nextLevel;

    public void UpdateXP(float xp, int level)
    {
        nextLevel = level + 1;
        xpBarSlider.value = xp;

        currentLevelText.text = "Level "+level;
        NextLevelText.text = "Level " + nextLevel;
    }
}
