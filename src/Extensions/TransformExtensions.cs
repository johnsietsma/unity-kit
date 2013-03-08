using UnityEngine;

public static class TransformExtensions
{
    public static Transform EnsureChild( this Transform t, string childName ) {
        return Ensure.Child( t, childName );
    }

    public static Pose ToPose( this Transform t )
    {
        return Pose.FromTransform( t );
    }
}

