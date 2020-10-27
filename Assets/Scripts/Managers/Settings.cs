using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle cameraShakeToogle;
    [SerializeField] Toggle dashParticulesToogle;
    [SerializeField] Toggle dashRecoverToogle;
    [SerializeField] Toggle wallGrabRecoverToogle;

    public static bool InGameSettings { get; set; } = false;

    private void Awake()
    {
        cameraShakeToogle.isOn = FeedBackControler.CameraShakeEnabled;
        dashParticulesToogle.isOn = FeedBackControler.DashParticulesEnabled;
        dashRecoverToogle.isOn = FeedBackControler.DashRecoverEnabled;
        wallGrabRecoverToogle.isOn = FeedBackControler.WallGrabRecoverEnabled;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("UINavCancel"))
        {
            OK();
        }
    }

    public void SetCameraShake()
    {
        FeedBackControler.CameraShakeEnabled = cameraShakeToogle.isOn;
    }

    public void SetDashParticles()
    {
        FeedBackControler.DashParticulesEnabled = dashParticulesToogle.isOn;
    }

    public void SetDashRecover()
    {
        FeedBackControler.DashRecoverEnabled = dashRecoverToogle.isOn;
    }

    public void SetWallGrabRecover()
    {
        FeedBackControler.WallGrabRecoverEnabled = wallGrabRecoverToogle.isOn;
    }

    public void OK()
    {
        if (InGameSettings)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            GameManager.Instance.LoadMenu();
        }   
    }
}
