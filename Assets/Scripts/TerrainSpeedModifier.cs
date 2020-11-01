using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpeedModifier : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] float speedModificationFactor;

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
        if (playerCollider.IsTouchingLayers(terrainLayer))
        {
            player.SlowDown(speedModificationFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerCollider.IsTouchingLayers(terrainLayer))
        {
            player.SlowDown(1.0f);
        }
    }
}
