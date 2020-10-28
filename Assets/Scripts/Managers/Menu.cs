using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.LoadGame();
    }

    public void LoadSettings()
    {
        Settings.InGameSettings = false;
        GameManager.Instance.LoadSettings();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
