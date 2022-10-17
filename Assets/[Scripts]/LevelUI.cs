using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    public GameObject xpBarUI;
    public Slider xpBarSlider;

    

    public void UpdateXP(float xp, int level)
    {
        xpBarSlider.value = xp;
    }
}
