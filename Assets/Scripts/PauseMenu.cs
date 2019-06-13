using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    public string menuSceneName = "Menu";

    private void Start()
    {
        
    }

    public void TogglePauseMenu()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    public void Menu(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}

