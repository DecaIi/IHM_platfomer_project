using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

[RequireComponent(typeof(Playercontroler))]
[RequireComponent(typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] LayerMask platformLayer;

    Playercontroler player;
    Collider2D collider;

    void Start()
    {
        player = GetComponent<Playercontroler>();
        collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        ContactFilter2D filter = new ContactFilter2D()
        {
            layerMask = platformLayer
        };

        RaycastHit2D[] hits = new RaycastHit2D[4];
        int numHits = collider.Cast(player.Velocity, hits, player.Velocity.magnitude * Time.deltaTime);

        Debug.Log(numHits);

        if (numHits > 0)
        {
            //player.IsGrounded = true;
            player.transform.position = player.Velocity.normalized * hits[0].distance + new Vector2(collider.bounds.center.x, collider.bounds.center.y);
        }
        else
        {
            //player.IsGrounded = false;
        }
    }
}
