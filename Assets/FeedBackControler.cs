using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackControler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    Camerashake camerashake;
    private void Start()
    {
        camerashake = playerCamera.GetComponent<Camerashake>();
    }
    public void  CameraSharke()
    {
        camerashake.Shake();
    }




}
