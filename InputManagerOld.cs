using UnityEngine;
using System;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private Camera[] cameras;

    public IEnumerable<RaycastHit> Hits {
        get { return hits; }
    }

    private RaycastHit[] hits = new RaycastHit[0];

    void Awake()
    {
        cameras = FindObjectsOfType( typeof(Camera) ) as Camera[];
        // Test input on cameras by camera depth
        Array.Sort<Camera>( cameras, CompareCameraDepths );
    }

    void Update()
    {
        if( Input.GetMouseButtonDown( 0 ) ) {
            foreach( Camera cam in cameras ) {
                if( TestCamera( cam ) ) {
                    break;
                }
            }
        }
    }

    private static int CompareCameraDepths( Camera c1, Camera c2 ) {
        return (int)(c2.depth-c1.depth);
    }

    private bool TestCamera( Camera cam )
    {
        Ray ray = cam.ScreenPointToRay( Input.mousePosition );
        Debug.DrawRay( ray.origin, ray.direction * 10, Color.yellow, 2 );
        RaycastHit hit;
        if( Physics.Raycast( ray, out hit, cam.farClipPlane, cam.cullingMask ) ) {
            hit.transform.SendMessageUpwards( "OnHit", hit, SendMessageOptions.DontRequireReceiver );
            return true;
        }
        return false;
    }
}
