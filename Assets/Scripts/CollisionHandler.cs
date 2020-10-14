using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;


[RequireComponent(typeof(Playercontroler))]
[RequireComponent(typeof(Collider2D))]
public class CollisionHandler : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right,
        Bottom,
        Top,
        Count
    }

    public struct Contacts
    {
        private bool[] contacts;

        public Contacts(int sideCount)
        {
            contacts = new bool[sideCount];

            for (int i = 0; i < contacts.Length; ++i)
            {
                contacts[i] = false;
            }
        }

        public override string ToString()
        {
            string text = "Contacts: ";

            foreach (bool contact in contacts)
            {
                text += contact + " ";
            }
            return text;
        }

        // Error prone
        public bool this[Side side]
        {
            get { return contacts[(int)side]; }
            set { contacts[(int)side] = value; }
        }
    }

    [SerializeField] LayerMask platformLayer;
    [SerializeField] int rayCastsPerSide;

    [Header("Contacts distances")]
    [SerializeField] float contactDistance;

    Playercontroler player;
    Collider2D collider;

    Vector2[] unitVectors2D = { Vector2.left, Vector2.right, Vector2.down, Vector2.up };
    bool[] contacts = { false, false, false, false };

    void Start()
    {
        player = GetComponent<Playercontroler>();
        collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        
    }

    bool Cast(Vector3 direction)
    {
        bool result = false;
        int i = 0;

        while (result == false && i < rayCastsPerSide)
        {
            result = Physics2D.Raycast(collider.bounds.center - Vector3.Dot(direction, collider.bounds.extents) * direction + direction * i * Vector3.Dot(direction, collider.bounds.extents) / (i + 1), direction, contactDistance, platformLayer).collider != null;
            ++i;
        }

        return result;
    }
}
