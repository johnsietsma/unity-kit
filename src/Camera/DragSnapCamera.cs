using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 * The drag, snap camera moves around on the horizontal plane, and snapso to points of interest.
 */
public class DragSnapCamera : MonoBehaviour
{
    #region Fields
    public Camera dragSnapCamera; // The camera to move, defaults to main.
    public float snapDist = 0.4f;
    public float tweenTime = 1;
    private float cameraHeight;
    private ICollection<Transform> snapPoints;  // The collection of points of interest to snap to.
    private Plane groundPlane; // This is used to find the world distance from screen space

    private Vector3 dragPos; // The finger drag pos when we're unlocked.
    private Transform currLockTarget; // The transform we are locked to
    private Vector3 currLockPosition; // The last known pos of the lock target

    private Tween cameraTween;
    #endregion

    #region Properties
    public ICollection<Transform> SnapPoints {
        get { return snapPoints; }
        set { snapPoints = value; }
    }
    #endregion

    #region Unity events
    void Awake()
    {
        dragSnapCamera = dragSnapCamera != null ? dragSnapCamera : Camera.main;
        Check.NotNull( dragSnapCamera, "No camera has been set" );

        cameraHeight = dragSnapCamera.transform.position.y;
        groundPlane = new Plane( Vector3.down, Vector3.zero );
    }

    void Update()
    {

        if( dragPos == Vector3.zero ) {
            if( currLockTarget != null && cameraTween.state != TweenState.Running ) {
                // Lock target has moved
                if( currLockTarget.position.Approx( currLockPosition ) )
                    Lock( currLockTarget );
            }
            return;
        }

        // We're dragging, so stop tweening
        cameraTween.pause();

        Vector3 adjustedDragPos = dragPos;

        /*Vector3 cameraGroundPos = CameraPosToGroundPlanePoint( dragPos );
        Transform targetSnapTransform = snapPoints.FirstOrDefault( t => t.position.Approx( cameraGroundPos, snapDist ) );
        if( targetSnapTransform != null ) {
            Vector3 snapPos = PointToCameraPos( targetSnapTransform.position );
            Vector3 deltaPos = dragPos - snapPos;
            float deltaDistNorm = 1 - deltaPos.magnitude / snapDist;
            deltaDistNorm = Mathf.Max( 0, deltaDistNorm );
            adjustedDragPos = Vector3.Lerp( dragPos, snapPos, Mathf.Pow( deltaDistNorm, 0.2f ) );//deltaDistNorm*deltaDistNorm*deltaDistNorm );
            //print( string.Format( "Target:{0} Snap:{1} Delta:{2} Norm:{3} Adj:{4}", dragPos, snapPos, deltaPos, deltaDistNorm, adjustedDragPos ) );
        }*/
        adjustedDragPos.y = cameraHeight;

        dragSnapCamera.transform.position = adjustedDragPos;
    }
    #endregion

    #region Input events
    void OnDoubleTap( InputEvent inputEvent )
    {
        RaycastHit hit = InputManager.Pick( Camera.main, inputEvent.pos );
        Transform hitTransform = hit.transform;

        if( hitTransform == null || !snapPoints.Contains( hitTransform ) )
            return;

        Lock( hitTransform );
    }

    void OnTapUp( InputEvent inputEvent )
    {
        if( dragPos != Vector3.zero ) {
            dragPos = Vector3.zero;
            Lock( FindClosestSnapPoint( dragSnapCamera.transform.position ) );
        }
    }

    void OnDrag( InputEvent inputEvent )
    {
        if( dragPos == Vector3.zero ) {
            dragPos = dragSnapCamera.transform.position;
        }
        Vector3 deltaPos = ScreenPointToGroundPlanePoint( inputEvent.pos ) - ScreenPointToGroundPlanePoint( inputEvent.lastPos );
        dragPos = dragPos - deltaPos.xz().AddY( 0 );
    }

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
    #endregion

    #region Private helpers
    private Transform FindClosestSnapPoint( Vector3 pos )
    {
        Transform closest = null;
        float closestDist = float.MaxValue;
        foreach( Transform t in snapPoints ) {
            float dist = (t.position - pos).magnitude;
            if( dist < closestDist ) {
                closest = t;
                closestDist = dist;
            }
        }
        return closest;
    }
    #endregion

    #region Camera control
    public void Lock( Transform t )
    {
        Check.NotNull( t, "Moving to a null transform" );
        Vector3 dest = PointToCameraPos( t.position );
        cameraTween = dragSnapCamera.transform.positionTo( tweenTime, dest, false );
        currLockTarget = t;
        currLockPosition = t.position;
    }
    #endregion
}
