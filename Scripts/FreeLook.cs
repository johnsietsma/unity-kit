using UnityEngine;
using System.Collections;

public class FreeLook : MonoBehaviour
{

    private Camera cam;
    private Quaternion startRot;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        startRot = cam.transform.rotation;
    }


    void Update ()
    {
        Quaternion rot = cam.transform.rotation;
        cam.transform.rotation = startRot;
        var screenPos = Input.mousePosition;
        screenPos.z = cam.nearClipPlane;
        Vector3 p = cam.ScreenToWorldPoint( screenPos );
        cam.transform.rotation = rot;

        cam.transform.LookAt( p );

    }
}
