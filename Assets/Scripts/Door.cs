using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite openDoor;
    [SerializeField] Sprite closedDoor;
    [SerializeField] bool invertedBehaviour;
 
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDoor(false);
    }

    public void SetDoor(bool open)
    {
        if ((open && !invertedBehaviour) || (!open && invertedBehaviour))
        {
            spriteRenderer.sprite = openDoor;
        }
        else
        {
            spriteRenderer.sprite = closedDoor;
        }
    }
}
