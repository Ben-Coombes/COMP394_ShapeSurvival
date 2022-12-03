using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton, menuButton;
    public void Resume()
    {
        GameManager.Instance.Resume(this.gameObject);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }
}
