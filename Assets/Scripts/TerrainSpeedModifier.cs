using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpeedModifier : MonoBehaviour
{
    [SerializeField] LayerMask slowDownLayer;
    [SerializeField] float slowDownFactor;

    Playercontroler player;
    Collider2D playerCollider;

    private void Awake()
    {
        player = GetComponent<Playercontroler>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerCollider.IsTouchingLayers(slowDownLayer))
        {
            player.SlowDown(slowDownFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerCollider.IsTouchingLayers(slowDownLayer))
        {
            player.SlowDown(1.0f);
        }
    }
}
