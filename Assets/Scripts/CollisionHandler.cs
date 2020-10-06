using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    // Reference to object's BoxCollider2D
    Collider2D collider2D;
    
    // Properties of the collision handler
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int sideRaycastCount;

    // Properties

    private Vector2 BoxColliderCenter
    {
        get { return boxCollider2D.bounds.center; }
    }

    private Vector2 BoxColliderExtents
    {
        get { return boxCollider2D.bounds.extents; }
    }

    private Vector2 BoxColliderSize
    {
        get { return boxCollider2D.size; }
    }

    // Enumarations and structs

    public enum Side : int
    {
        Left = -1,
        Right = 1
    }

    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Debug.Log(GetColliderBottom(1f));   
    }

    bool CastCollider(Vector2 direction, out RaycastHit2D[] raycasts, float distance)
    {
        return collider2D.Cast(direction, raycasts, distance);
    }

    Collider2D[] GetCollidersSide(Side side, float distance)
    {
        Collider2D[] sideColliders = new Collider2D[sideRaycastCount];
        Vector2 direction = side == Side.Left ? Vector2.left : Vector2.right;

        for (int i = 0; i < sideRaycastCount; ++i)
        {
            sideColliders[i] = Physics2D.Raycast(new Vector2(BoxColliderCenter.x, i * BoxColliderSize.y / (sideRaycastCount - 1)), direction, distance, layerMask).collider;
        }

        return sideColliders;
    }

    Collider2D GetColliderBottom(float distance)
    {
        RaycastHit2D hit = Physics2D.BoxCast(BoxColliderCenter, BoxColliderSize, 0f, Vector2.down, distance, layerMask);
        return hit.collider;
    }

    Collider2D GetColliderTop(float distance)
    {
        RaycastHit2D hit = Physics2D.BoxCast(BoxColliderCenter, BoxColliderSize, 0f, Vector2.up, distance, layerMask);
        return hit.collider;
    }
}
