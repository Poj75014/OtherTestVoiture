using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsTools
{
    public static Vector2 DegreesToVector2(float angle_in_degrees)
    {
        float angle_in_radians = Mathf.Deg2Rad * angle_in_degrees;
        return new Vector2((float)Mathf.Cos(angle_in_radians),
                           -(float)Mathf.Sin(angle_in_radians));
    }

    public static float Vector2ToDegrees(Vector2 angle_vector)
    {
        float angle_in_radians = Mathf.Atan2(angle_vector.x, -angle_vector.y);
        return Mathf.Rad2Deg * angle_in_radians;
    }
}
