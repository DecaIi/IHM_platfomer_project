using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingZonne : MonoBehaviour
{
    Playercontroler playercontroler;
    [SerializeField] float incapacityDuration;
    void OnTriggerEnter2D(Collider2D other)
    {
        playercontroler = other.gameObject.GetComponent<Playercontroler>();
        if (playercontroler != null)
        {
            playercontroler.Bouncing();
        }
    }
}
