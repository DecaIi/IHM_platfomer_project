using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FeedBackControler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeIntensity;
    [SerializeField] ParticleSystem ParticleSystem;

    public AudioSource audioSource;
    public AudioClip jumpAudioClip;
    public AudioClip dashAudioClip;
    
    public static bool CameraShakeEnabled { get; set; } = true;
    public static bool DashParticulesEnabled { get; set; } = true;
    public static bool DashRecoverEnabled { get; set; } = true;
    public static bool WallGrabRecoverEnabled { get; set; } = true;
    public static bool TrailOnDashEnabled { get; set; } = true;
    
    
    /* Alow to enable/disable sound feedback */
    public static bool SoundDashEnabled { get; set; } = true;
    public static bool SoundJumpEnabled { get; set; } = true;

    Camerashake camerashake;
    SpriteRenderer renderer;
       
    private void Start()
    {
        camerashake = playerCamera.GetComponent<Camerashake>();
        renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        trailRenderer.enabled = false;
    }
    private void Update()
    {
        
    }
    public void  CameraSharke()
    {
        if (CameraShakeEnabled) { 
            camerashake.Shake(shakeDuration,shakeIntensity);
        }
    }
    /** instanciate the particule system on the given position
     *  param/ float position   position were we want the particul system to be instanciate  
     */
    public void PlayJumpSound()
    {
        if (SoundJumpEnabled){
            audioSource.PlayOneShot(jumpAudioClip);
        }
    }
    public void PlayDashSound()
    {
        if(SoundDashEnabled){
            audioSource.PlayOneShot(dashAudioClip);
        }
    }

    public void ActivateTrail(float time )
    {
        if(TrailOnDashEnabled){
            StartCoroutine(activateTrail(time));
        }
    }
    IEnumerator activateTrail( float t)
    {
        trailRenderer.enabled = true;
        yield return new WaitForSeconds(t);
        trailRenderer.enabled = false;
        yield return null;
    }


    public void InstanciateDashPrefabOnPosition(Vector3 position)
    {
       if (DashParticulesEnabled) { 
            ParticleSystem particleSystem = Instantiate(ParticleSystem,position,Quaternion.identity);
            Destroy(particleSystem, particleSystem.main.duration); //destroy the particul system after his lif time 
        }
    }
   /**change the color of the object for the given color 
    * param/ Color color        the color we want the object became 
    */
   public void ChangeColor(Color color)
    {
        if (DashRecoverEnabled)
        {
            if (canchangecolor)
            {
                renderer.color = color;
            }
        }
    }

    bool canchangecolor = true;
    public void Clignote(Color color1, Color color2, float time)
    {
        if (WallGrabRecoverEnabled)
        {
            if (canchangecolor)
            {
                StartCoroutine(ClignoteEnumerator(color1, color2, time));
            }
        }
    }
    public void Clignote(Color color1, Color color2)
    {
        if (WallGrabRecoverEnabled)
        {
            if (canchangecolor)
            {
                renderer.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time * 8, 1));
            }
        }
    }
    IEnumerator ClignoteEnumerator(Color color1, Color color2, float time)
    {
        canchangecolor = false;
        float curentime = 0f;
        while (curentime < time)
        {
            curentime += Time.deltaTime;
            renderer.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time* 8 , 1));
            yield return null;
        }
        canchangecolor = true;
        ChangeToRed();
        yield return null;
    }

    public void ChangeToRed()
    {
        ChangeColor(Color.red);
    }
    public void ChangeToBlue()
    {
        ChangeColor(Color.blue);
    }
 
}
