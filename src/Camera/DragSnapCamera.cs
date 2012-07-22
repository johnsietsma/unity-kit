using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NLog;

/**
 * The drag, snap camera moves around on the horizontal plane, and snapso to points of interest.
 */
public class DragSnapCamera : MonoBehaviour
{
    #region Fields
    public Camera dragSnapCamera; // The camera to move, defaults to main.
    public float tweenTime = 1;
    public float speedSmoothingFactor = 0.9f;
    public float frictionFactor = 0.75f;
    private float cameraHeight;
    private Plane groundPlane; // This is used to find the world distance from screen space
    private Logger log;

    private enum State
    {
        None,
        Dragging,
        Drifting,
        Locked,
        Tweening
    };
    private State state = State.None;
    private Vector3 speed = Vector3.zero;
    private Transform currLockTarget; // The transform we are locked to
    private Vector3 currLockPosition; // The last known pos of the lock target
    private Tween cameraTween;
    #endregion

    #region Unity events
    void Awake()
    {
        log = LogManager.GetLogger( "Camera" );
        dragSnapCamera = dragSnapCamera != null ? dragSnapCamera : Camera.main;
        Check.NotNull( dragSnapCamera, "No camera has been set" );

        cameraHeight = dragSnapCamera.transform.position.y;
        groundPlane = new Plane( Vector3.down, Vector3.zero );
    }

    void Update()
    {
        switch( state ) {
        case State.Locked:
            UpdateLockTarget();
            break;
        case State.Drifting:
            UpdateDrift();
            break;
        }
    }
    #endregion

    #region Input events
    void OnDoubleTap( InputEvent inputEvent )
    {
        if( inputEvent.hit==null ) return;
        Transform hitTransform = inputEvent.hit.transform;

        Lock( hitTransform );
    }

    void OnTapUp( InputEvent inputEvent )
    {
        if( state == State.Dragging ) {
            state = State.Drifting;
        }
    }

    void OnDrag( InputEvent inputEvent )
    {
        if( state != State.Drifting ) {
            state = State.Dragging;
            if( cameraTween != null )
                cameraTween.pause();
        }
        Vector3 deltaPos = ScreenPointToGroundPlanePoint( inputEvent.pos ) - ScreenPointToGroundPlanePoint( inputEvent.lastPos );
        deltaPos = -deltaPos.xz().AddY( 0 );
        speed = speed.LowPassFilter( deltaPos / Time.deltaTime, speedSmoothingFactor );
        speed.y = 0;
        log.Trace( "Drag by " + deltaPos );
        dragSnapCamera.transform.Translate( deltaPos, Space.World );
    }
    #endregion

    #region Private helpers
    private Vector3 CameraPosToGroundPlanePoint( Vector3 cameraPos )
    {
        Ray ray = new Ray( cameraPos, dragSnapCamera.transform.forward );
        return RayToGroundPlanePoint( ray );
    }

    private Vector3 ScreenPointToGroundPlanePoint( Vector3 screenPoint )
    {
        Ray ray = dragSnapCamera.ScreenPointToRay( screenPoint );
        return RayToGroundPlanePoint( ray );
    }

    private Vector3 RayToGroundPlanePoint( Ray ray )
    {
        float d;
        if( groundPlane.Raycast( ray, out d ) ) {
            return ray.GetPoint( d );
        }
        return Vector3.zero;
    }

    /**
     * Find the camera postion that would centre on the given point.
     */
    private Vector3 PointToCameraPos( Vector3 point )
    {
        // Get the distance between the object and the camera
        float hypDist = dragSnapCamera.transform.position.y / Mathf.Sin( dragSnapCamera.transform.rotation.eulerAngles.x * Mathf.Deg2Rad );
        Check.True( hypDist != float.NaN, "The camera is not pointing down" );
        Check.True( hypDist != float.NegativeInfinity, "The camera is not pointing down" );
        Check.True( hypDist != float.PositiveInfinity, "The camera is not pointing down" );

        // Move point away the required distance
        Vector3 camPoint = point - dragSnapCamera.transform.forward * hypDist;
        camPoint.y = cameraHeight;
        return camPoint;
    }

    private void UpdateLockTarget()
    {
        // Lock target has moved
        if( !currLockTarget.position.Approx( currLockPosition ) ) {
            //print( "Target moved - target:" + currLockTarget.position.ToStringf() + " curr pos:" + currLockPosition.ToStringf() );
            Lock( currLockTarget );
        }

    }

    private void UpdateDrift()
    {
        speed *= frictionFactor;
        dragSnapCamera.transform.Translate( speed * Time.deltaTime, Space.World );
    }
    #endregion

    #region Camera control
    public void Lock( Transform t )
    {
        Check.NotNull( t, "Moving to a null transform" );
        log.Trace( "Locking on target " + t.name );
        Vector3 dest = PointToCameraPos( t.position );
        //print( "Cam rot: " + dragSnapCamera.transform.rotation.eulerAngles.ToStringf() );
        cameraTween = dragSnapCamera.transform.positionTo( tweenTime, dest, false );
        //dragSnapCamera.transform.position = dest;
        currLockTarget = t;
        currLockPosition = t.position;
        state = State.Locked;
    }
    #endregion
}
