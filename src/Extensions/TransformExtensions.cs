using UnityEngine;
using System.Collections.Generic;

public static class TransformExtensions
{
    public static IEnumerable<Transform> Parents( this Transform t )
    {
        while( t.parent != null ) {
            yield return t.parent;
            t = t.parent;
        }
    }

    public static Transform EnsureChild( this Transform t, string childName )
    {
        return Ensure.Child( t, childName );
    }

    public static Pose ToPose( this Transform t )
    {
        return Pose.FromTransform( t );
    }
}

