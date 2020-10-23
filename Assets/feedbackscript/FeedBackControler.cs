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
    private void Start()
    {
        camerashake = playerCamera.GetComponent<Camerashake>();
    }
    public void  CameraSharke()
    {
        camerashake.Shake(shakeDuration,shakeIntensity);
    }
    public void InstanciateDashPrefabOnPosition(Vector3 position)
    {
        ParticleSystem o = Instantiate(ParticleSystem,position,Quaternion.identity);
        StartCoroutine(DestritroyInstance(o, 1));
    }
    IEnumerator DestritroyInstance(ParticleSystem ps, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(ps);
    }




}
