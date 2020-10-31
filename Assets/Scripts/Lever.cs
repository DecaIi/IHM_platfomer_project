using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.flipY = !spriteRenderer.flipY;
        IsActive = !IsActive;
    }
}
