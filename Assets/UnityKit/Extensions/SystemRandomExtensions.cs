using System;
using UnityEngine;

public static class SystemRandomExtensions
{
    public static Vector3 NextVector3( this System.Random r, Vector3 maxValue )
    {
        return r.NextVector3( Vector3.zero, maxValue );
    }

    public static Vector3 NextVector3( this System.Random r, Vector3 minValue, Vector3 maxValue )
    {
        return Vector3.Lerp( minValue, maxValue, (float)r.NextDouble() );
    }
}
