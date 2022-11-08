using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI scoreText;
    public float score = 0;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        scoreText.text = "Score: " + score;
    }
    public void Pause(GameObject obj)
    {
        obj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 0f;
    }

    public void Resume(GameObject obj)
    {
        obj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.SwitchActionMap();
        Time.timeScale = 1f;
    }

    public void IncreaseScore(float amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}
