using UnityEngine;
using System.Collections;

public static class Vector3Extensions
{
    public static Vector3 RotateAroundPoint( this Vector3 pivot, Vector3 point, Quaternion angle )
    {
        return angle * (point - pivot) + pivot;
    }
    
    public static bool Approx( this Vector3 v1, Vector3 v2, float tolerance=0.01f )
    {
        return (v1 - v2).magnitude < tolerance;
    }
 
    public static Vector3 ClampToBounds( this Vector3 v, Bounds bounds )
    {
        Vector3 clamp = v;
        Ray clampRay = new Ray( Vector2.zero, v );
        float dist;
        if( bounds.IntersectRay( clampRay, out dist ) ) {
            clamp = Vector3.ClampMagnitude( v, dist );
        }
        return clamp;
    }
 
    public static Vector2 xy( this Vector3 v )
    {
        return new Vector2( v.x, v.y );
    }
 
    public static Vector2 xz( this Vector3 v )
    {
        return new Vector2( v.x, v.z );
    }

    public static Vector2 yz( this Vector3 v )
    {
        return new Vector2( v.y, v.z );
    }
 
    public static Vector3 ToGround( this Vector3 v )
    {
        return new Vector3( v.x, 0, v.z );
    }
 
    public static float[] ToArray( this Vector3 v )
    {
        return new float[] { v.x, v.y, v.z };
    }
 
    public static string ToStringf( this Vector3 v )
    {
        return v.ToString( "0.0,0.0,0.0" );
    }
    
    public static Vector3 Abs( this Vector3 v )
    {
        return new Vector3( Mathf.Abs( v.x ), Mathf.Abs( v.y ), Mathf.Abs( v.z ) );
    }

    public static Vector3 LowPassFilter( this Vector3 v, Vector3 prevVector, float smoothFactor )
    {
        return new Vector3(
            Filter.LowPass( prevVector.x, v.x, smoothFactor ),
            Filter.LowPass( prevVector.y, v.y, smoothFactor ),
            Filter.LowPass( prevVector.z, v.z, smoothFactor )
            );
    }
}
