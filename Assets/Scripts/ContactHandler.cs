using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public struct Contacts
{
    private readonly bool leftContact;
    private readonly bool rightContact;
    private readonly bool bottomContact;
    private readonly bool topContact;

    public Contacts(bool left, bool right, bool bottom, bool top)
    {
        leftContact = left;
        rightContact = right;
        bottomContact = bottom;
        topContact = top;
    }

    public bool Left
    {
        get { return leftContact; }
    }

    public bool Right
    {
        get { return rightContact; }
    }

    public bool Bottom
    {
        get { return bottomContact; }
    }

    public bool Top
    {
        get { return topContact; }
    }

    public override string ToString()
    {
        return "Contacts: (Left: " + leftContact + " Right: " + rightContact + " Bottom: " + bottomContact + " Top: " + topContact + ")";
    }

    public static Contacts None()
    {
        return new Contacts(false, false, false, false);
    }
}

[RequireComponent(typeof(Collider2D))]
public class ContactHandler : MonoBehaviour
{
    [SerializeField] LayerMask platformLayer;
    [SerializeField] int rayCastsPerSide;
    [SerializeField] float contactDistance;

    new Collider2D collider;

    public Contacts Contacts { get; private set; } = Contacts.None();

    private Vector2 ColliderCenter
    {
        get { return collider.bounds.center; }
    }

    private Vector2 ColliderSize
    {
        get { return collider.bounds.size; }
    }
    private Vector2 ColliderExtents
    {
        get { return collider.bounds.extents; }
    }

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        bool leftContact = Cast(Vector2.left);
        bool rightContact = Cast(Vector2.right);
        bool bottomContact = Cast(Vector2.down);
        bool topContact = Cast(Vector2.up);

        Contacts = new Contacts(leftContact, rightContact, bottomContact, topContact);
    }

    /*
     * Check for collisions in the given direction. Returns true if a collision is detected.
     * Uses member parameter contactDistance as cast distance and rayCastsPerSide for the cast resolution.
     */
    bool Cast(Vector2 direction)
    {
        bool result = false;
        int i = 0;

        while (result == false && i < rayCastsPerSide)
        {
            // Raycasts originate from a line perpendicular to direction passing by the center of the collider. They are evenly distributed along this line.
            result = Physics2D.Raycast(ColliderCenter + Vector2.Perpendicular(direction) * ColliderSize * ((i + 1) / rayCastsPerSide - 0.5f), direction, Mathf.Abs(Vector2.Dot(ColliderExtents, direction)) + contactDistance, platformLayer).collider != null;
            ++i;
        }
        return result;
    }
}
