using UnityEngine;

public static class Vector2Extensions
{
    /*public static bool Approx( this Vector2 v1, Vector2 v2, float tolerance=0.01f )
    {
        return (v1 - v2).magnitude < tolerance;
    }*/

    public static Vector3 AddY( this Vector2 v, float y )
    {
        return new Vector3( v.x, y, v.y );
    }

    public static Vector3 AddZ( this Vector2 v, float z )
    {
        return new Vector3( v.x, v.y, z );
    }

    public static string ToStringf( this Vector2 v )
    {
        return v.ToString( "0.0,0.0" );
    }
}