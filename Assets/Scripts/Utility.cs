using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Utility
{
    private static Vector2[] unitVectors2D = { Vector2.left, Vector2.right, Vector2.down, Vector2.up };

    public Vector2[] UnitVectors2D
    {
        get { return unitVectors2D; }
    }

    public static Vector3 Vector2ToVector3(Vector2 vector)
    {
        return vector;
    }

    public static Vector2 Vector3ToVector2(Vector3 vector)
    {
        return vector;
    }
}
