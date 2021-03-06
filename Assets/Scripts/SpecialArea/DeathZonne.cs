﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonne : MonoBehaviour
{
    [SerializeField] Vector2 reSpwanPosition;
    [SerializeField] float incapacityDuration;
    Playercontroler playercontroler;
    void OnTriggerEnter2D(Collider2D other)
    {
        playercontroler = other.gameObject.GetComponent<Playercontroler>();
        if (playercontroler != null)
        {
            playercontroler.Respawn(reSpwanPosition, incapacityDuration);
        }
    }
    
}
