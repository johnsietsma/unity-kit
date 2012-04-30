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
    }
    #endregion

    #region Input events
    void OnSingleTap( InputInfo inputInfo )
    {
        if( inputInfo.hit.transform == null )
            return;

        LookAt( inputInfo.hit.transform );
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

        // Get the distance between the object and the camera
        float hypDist = defaultHeight / Mathf.Sin( defaultLookDownAngle * Mathf.Deg2Rad );

        // Move camera away the required distance
        dragSnapCamera.transform.position = t.position - dragSnapCamera.transform.forward * hypDist;
    }
    #endregion
}
