using UnityEngine;
using System.Collections;

public class InputReceiver : MonoBehaviour
{
    // Optionally assign a camera for input ordering and object picking. Defaults to the main camera.
    public Camera inputCamera;
    public string cameraName;

    [HideInInspector]
    public Rect[] ignoreAreas = new Rect[0];

    void Awake()
    {
        if( inputCamera == null ) {
            if( !string.IsNullOrEmpty( cameraName ) ) {
                inputCamera = GameObject.Find( cameraName ).camera;
            }
            if( inputCamera == null ) {
                inputCamera = camera != null ? camera : Camera.main;
            }
        }

        InputManager inputManager = Ensure.SceneObject<InputManager>();
        inputManager.AddInputReceiver( this );
    }
}