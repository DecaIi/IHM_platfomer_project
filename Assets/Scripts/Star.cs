using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Collider2D starCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        starCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        starCollider.enabled = false;
        GameManager.Instance.LevelStars += 1;
        spriteRenderer.enabled = false;
    }
}
