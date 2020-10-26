using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackControler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeIntensity;
    [SerializeField] ParticleSystem ParticleSystem;
    Camerashake camerashake;
    Material material;
    
    private void Start()
    {
        camerashake = playerCamera.GetComponent<Camerashake>();
        material = GetComponent<Renderer>().material;
    }
    public void  CameraSharke()
    {
        camerashake.Shake(shakeDuration,shakeIntensity);
    }
    /** instanciate the particule system on the given position
     *  param/ float position   position were we want the particul system to be instanciate  
     */
    public void InstanciateDashPrefabOnPosition(Vector3 position)
    {
        ParticleSystem particleSystem = Instantiate(ParticleSystem,position,Quaternion.identity);
        Destroy(particleSystem, particleSystem.main.duration); //destroy the particul system after his lif time 
    }
   /**change the color of the object for the given color 
    * param/ Color color        the color we want the object became 
    */
   public void ChangeColor(Color color)
    {
        material.color = color;
    }

    public void ChangeToRed()
    {
        ChangeColor(Color.red);
    }
    public void ChangeToBleu()
    {
        ChangeColor(Color.green);
    }
    /** Smouthly change color from start to end with gthe given speed 
     *  param/ Color startColor the color the object start
     *  param/ Color endColor   the color the object will end
     *  param/ float speed      the speed the color must change
     */
    private IEnumerator SmouthChangeColor(Color startColor , Color  endColor, float speed )
    {
        float tick = 0f;
        while (material.color != endColor)
        {
            tick += Time.deltaTime * speed;
            material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }
}
