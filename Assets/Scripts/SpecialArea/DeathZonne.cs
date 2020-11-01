using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonne : MonoBehaviour
{
    [SerializeField] Playercontroler playercontroler;
    [SerializeField] Vector2 reSpwanPosition;
    [SerializeField] float incapacityDuration;
    void OnTriggerEnter2D(Collider2D other)
    {
        playercontroler.Respawn(reSpwanPosition, incapacityDuration);
    }
    
}
