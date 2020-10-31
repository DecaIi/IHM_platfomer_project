using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle cameraShakeToogle;
    [SerializeField] Toggle dashParticulesToogle;
    [SerializeField] Toggle dashTrailToogle;
    [SerializeField] Toggle dashRecoverToogle;
    [SerializeField] Toggle wallGrabRecoverToogle;
    [SerializeField] Toggle jumpSoundToogle;
    [SerializeField] Toggle dashSoundToogle;

    [SerializeField] GameObject panel;
    [SerializeField] Color darkenColor;

    public static bool InGameSettings { get; set; } = false;

    private void Awake()
    {
        cameraShakeToogle.isOn = FeedBackControler.CameraShakeEnabled;
        dashParticulesToogle.isOn = FeedBackControler.DashParticulesEnabled;
        dashTrailToogle.isOn = FeedBackControler.TrailOnDashEnabled;
        dashRecoverToogle.isOn = FeedBackControler.DashRecoverEnabled;
        wallGrabRecoverToogle.isOn = FeedBackControler.WallGrabRecoverEnabled;
        jumpSoundToogle.isOn = FeedBackControler.JumpSoundEnabled;
        dashSoundToogle.isOn = FeedBackControler.DashSoundEnabled;

        if (InGameSettings)
        {
            panel.GetComponent<Image>().color = darkenColor;
        }
        else
        {
            panel.GetComponent<Image>().color = Color.clear;
        }
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

    public void SetDashTrail()
    {
        FeedBackControler.TrailOnDashEnabled = dashTrailToogle.isOn;
    }

    public void SetDashRecover()
    {
        FeedBackControler.DashRecoverEnabled = dashRecoverToogle.isOn;
    }

    public void SetWallGrabRecover()
    {
        FeedBackControler.WallGrabRecoverEnabled = wallGrabRecoverToogle.isOn;
    }

    public void SetJumpSound()
    {
        FeedBackControler.JumpSoundEnabled = jumpSoundToogle.isOn;
    }

    public void SetDashSound()
    {
        FeedBackControler.DashSoundEnabled = dashSoundToogle.isOn;
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
