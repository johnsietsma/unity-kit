using UnityEngine;
using System.Collections.Generic;

public static class Picker
{
    public static readonly RaycastHit DefaultRaycastHit = new RaycastHit();

    public static RaycastHit Pick( Camera camera, Vector3 point )
    {
        Ray ray = camera.ScreenPointToRay( point );
        RaycastHit hit;
        if( Physics.Raycast( ray, out hit ) ) {
            return hit;
        }
        return DefaultRaycastHit;
    }

    public static IEnumerable<RaycastHit> Pick( IEnumerable<Camera> cameras, Vector3 point )
    {
        foreach( Camera cam in cameras ) {
            RaycastHit hit = Pick( cam, point );
            if( hit.transform==null )
                yield return hit;
        }
    }
}

