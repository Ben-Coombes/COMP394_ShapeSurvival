using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundtrack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SoundManager>().Play("MainMenuSoundtrack");
    }
}
