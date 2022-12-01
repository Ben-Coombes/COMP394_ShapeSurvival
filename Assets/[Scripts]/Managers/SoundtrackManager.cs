using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    bool playingLoop = false;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SoundManager>().Play("SoundtrackIntro");
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<SoundManager>().GetAudioSource("SoundtrackIntro").isPlaying && playingLoop == false)
        {
            playingLoop = true;
            FindObjectOfType<SoundManager>().Play("SoundtrackLoop");
        }
    }
}
