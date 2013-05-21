using UnityEngine;
using System.Collections;

public class InputReceiver : MonoBehaviour
{
    // Optionally assign a camera for input ordering and object picking. Defaults to the main camera.
    public Camera inputCamera;
    public string cameraName;
    public bool receiveAllInput = false;

    [HideInInspector]
    public Rect[] ignoreAreas = new Rect[0];

    private InputManager inputManager;

    // Add ourselves to the input manager, do this in Start to give InputManager to do stuff in Awake.
    void Start()
    {
        if( inputCamera == null ) {
            if( !string.IsNullOrEmpty( cameraName ) ) {
                inputCamera = GameObject.Find( cameraName ).camera;
            }
            if( inputCamera == null ) {
                inputCamera = camera != null ? camera : Camera.main;
            }
        }

        inputManager = Ensure.SceneObject<InputManager>();
        inputManager.AddInputReceiver( this );
    }

    void OnDestroy() {
        inputManager.RemoveInputReceiver( this );
    }
}