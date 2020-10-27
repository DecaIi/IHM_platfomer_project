using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FeedBackControler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeIntensity;
    [SerializeField] ParticleSystem ParticleSystem;

    public AudioSource audioSource;
    public AudioClip jumpAudioClip;
    public AudioClip dashAudioClip;

    Camerashake camerashake;
    SpriteRenderer renderer;
    
    private void Start()
    {
        camerashake = playerCamera.GetComponent<Camerashake>();
        renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        
    }
    public void  CameraSharke()
    {
        if(true) { 
            camerashake.Shake(shakeDuration,shakeIntensity);
        }
    }
    /** instanciate the particule system on the given position
     *  param/ float position   position were we want the particul system to be instanciate  
     */
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpAudioClip);
    }
    public void PlayDashSound()
    {
        audioSource.PlayOneShot(dashAudioClip);
    }

    public void InstanciateDashPrefabOnPosition(Vector3 position)
    {
       if( true ) { 
            ParticleSystem particleSystem = Instantiate(ParticleSystem,position,Quaternion.identity);
            Destroy(particleSystem, particleSystem.main.duration); //destroy the particul system after his lif time 
        }
    }
   /**change the color of the object for the given color 
    * param/ Color color        the color we want the object became 
    */
   public void ChangeColor(Color color)
    {
        if (true)
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
        if (true)
        {
            if (canchangecolor)
            {
                StartCoroutine(ClignoteEnumerator(color1, color2, time));
            }
        }
    }
    public void Clignote(Color color1, Color color2)
    {
        if (true)
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
