using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void SetCameraShake(bool setting)
    {

    }

    public void SetDashParticles(bool setting)
    {

    }

    public void SetDashRecover(bool setting)
    {

    }

    public void SetWallGrabRecover(bool setting)
    {

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
