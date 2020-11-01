using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] List<Door> doors;

    SpriteRenderer spriteRenderer;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        IsActive = !IsActive;

        foreach (Door door in doors)
        {
            door.SetDoorOpen(IsActive);
        }
    }
}
