using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite openDoor;
    [SerializeField] Sprite closedDoor;
    [SerializeField] bool invertedBehaviour;

    public bool IsOpen { get; private set; }
 
    SpriteRenderer spriteRenderer;
    Collider2D doorCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        SetDoorOpen(false);
    }

    public void SetDoorOpen(bool open)
    {
        if ((open && !invertedBehaviour) || (!open && invertedBehaviour))
        {
            IsOpen = true;
            spriteRenderer.sprite = openDoor;
            doorCollider.enabled = false;
        }
        else
        {
            IsOpen = false;
            spriteRenderer.sprite = closedDoor;
            doorCollider.enabled = true;
        }
    }
}
