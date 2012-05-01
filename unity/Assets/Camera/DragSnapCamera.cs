using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(DragSnapCamera))]
public class DragSnapCamera : MonoBehaviour
{
    #region Fields
    public Camera dragSnapCamera;
    private float defaultHeight;
    private float defaultLookDownAngle;
    private Transform defaultSnapPoint;
    private ICollection<Transform> snapPoints;
    private Plane groundPlane;
    #endregion

    #region Properties
    public ICollection<Transform> SnapPoints {
        get { return snapPoints; }
        set { snapPoints = value; }
    }

    public Transform DefaultSnapPoint {
        get { return defaultSnapPoint; }
        set {
            Check.NotNull( value, "Setting a null default snap point" );
            defaultSnapPoint = value;
        }
    }
    #endregion

    #region Unity events
    void Awake()
    {
        dragSnapCamera = dragSnapCamera != null ? dragSnapCamera : Camera.main;
        Check.NotNull( dragSnapCamera, "No camera has been set" );

        defaultHeight = dragSnapCamera.transform.position.y;
        defaultLookDownAngle = dragSnapCamera.transform.localRotation.eulerAngles.x;
        groundPlane = new Plane( Vector3.down, 0 );
    }

    void Update()
    {

        if( Input.GetMouseButtonDown( 0 ) ) {
            Vector3 clickPos = Input.mousePosition;
            clickPos.z = dragSnapCamera.nearClipPlane;
            Vector3 p = dragSnapCamera.ScreenToWorldPoint( clickPos );
            print( "Wold pos: " + p + " click:" + clickPos );
        }
    }
    #endregion

    #region Input events
    void OnSingleTap( InputEvent inputEvent )
    {
        Transform hitTransform = inputEvent.hit.transform;

        if( hitTransform == null || !snapPoints.Contains( hitTransform ) )
            return;

        LookAt( hitTransform );
    }

    void OnDrag( InputEvent inputEvent )
    {
        Vector2 deltaScreenPos = (inputEvent.pos - inputEvent.lastPos).xy();

        // Get the delta in near clip space
        Vector3 nearClipPos = dragSnapCamera.ScreenToWorldPoint( deltaScreenPos.AddZ( dragSnapCamera.nearClipPlane ) );
        Vector3 nearClipZeroPos = dragSnapCamera.ScreenToWorldPoint( new Vector3( 0, 0, dragSnapCamera.nearClipPlane ) );
        Vector2 deltaNearClipPos = (nearClipPos - nearClipZeroPos).xy();

        // Use this ratio to find the world space delta
        float r = dragSnapCamera.transform.position.y / dragSnapCamera.nearClipPlane;
        Vector2 deltaPos = deltaNearClipPos * r;

        // Move horizontally along rotation of camera
        Vector3 eulerRot = dragSnapCamera.transform.localRotation.eulerAngles;
        Quaternion horzRot = Quaternion.Euler( 0, eulerRot.y, eulerRot.z );
        dragSnapCamera.transform.Translate( horzRot * -deltaPos.AddY( 0 ), Space.World );
    }

    private Vector3 ScreenPointToGroundPlanePoint( Vector3 screenPoint )
    {
        Ray ray = dragSnapCamera.ScreenPointToRay( screenPoint );
        float d;
        if( groundPlane.Raycast( ray, out d ) ) {
            return dragSnapCamera.transform.forward * d;
        }
        return Vector3.zero;
    }
    #endregion

    #region Camera control
    public void MoveToDefaultSnapPoint()
    {
        Check.NotNull( defaultSnapPoint );
        LookAt( defaultSnapPoint );
    }
    #endregion

    #region Private helpers
    private void LookAt( Transform t )
    {
        Check.NotNull( t, "Moving to a null transform" );
        print( "Looking at: " + t );

        // Get the distance between the object and the camera
        float hypDist = defaultHeight / Mathf.Sin( defaultLookDownAngle * Mathf.Deg2Rad );

        // Move camera away the required distance
        dragSnapCamera.transform.position = t.position - dragSnapCamera.transform.forward * hypDist;
    }
    #endregion
}
