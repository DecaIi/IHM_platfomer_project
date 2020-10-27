using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.Resume();
    }

    public void LoadSettings()
    {
        Settings.InGameSettings = true;
        GameManager.Instance.LoadSettings();
    }

    public void Quit()
    {
        GameManager.Instance.LoadMenu();
    }
}
