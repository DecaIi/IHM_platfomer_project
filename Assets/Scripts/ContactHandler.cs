using UnityEngine;

/*
 * Struct to hold contact data.
 */
public struct Contacts
{
    public Contacts(bool left, bool right, bool bottom, bool top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public bool Left { get; }

    public bool Right { get; }

    public bool Bottom { get; }

    public bool Top { get; }

    public override string ToString()
    {
        return "Contacts: (Left: " + Left + " Right: " + Right + " Bottom: " + Bottom + " Top: " + Top + ")";
    }

    /*
     * Returns a Contacts struct with every side at false.
     */
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
