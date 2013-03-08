using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Pose
{
    public static readonly Pose zero = new Pose( Vector3.zero, Quaternion.identity, Vector3.zero );
    
    public Vector3 position;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;
    
    public Pose( Vector3 position, Quaternion rotation, Vector3 scale )
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    
    public static Pose FromTransform( Transform t )
    {
        return new Pose( t.localPosition, t.localRotation, t.localScale );
    }
    
    public void ApplyToTransform( Transform t )
    {
        t.localPosition = position;
        t.localRotation = rotation;
        t.localScale = scale;
    }

    public static Pose Lerp( Pose p1, Pose p2, float t ) {
        return new Pose( 
                        Vector3.Lerp( p1.position, p2.position, t ),
                        Quaternion.Lerp( p1.rotation, p2.rotation, t ),
                        Vector3.Lerp( p1.scale, p2.scale, t )
                        );
    }
    
    public override string ToString()
    {
        return string.Format( position + " " + rotation + " " + scale );
    }
}
