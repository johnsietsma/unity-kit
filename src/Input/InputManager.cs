using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Central manager of all input in a scene.
 * Sends input messages to all recievers, allows ordering and swallowing of messages.
 * Raycasts to provide hit info.
 * Uses both touch and mouse input.
 */
public class InputManager : MonoBehaviour
{
    public float doubleTapTimeDelta = 0.2f;
    public float deadZone = 0.01f;
    public List<InputReceiver> inputReceivers = new List<InputReceiver>();
    private enum TouchState
    {
        TapUp,
        TapDown
    };

    private InputInfo currTouch = new InputInfo();
    private InputInfo lastTouch = new InputInfo();
    private TouchState touchState = TouchState.TapUp;
    private static readonly RaycastHit DefaultRaycastHit = new RaycastHit();

    public void AddInputReceiver( InputReceiver inputReceiver )
    {
        inputReceivers.Add( inputReceiver );
        inputReceivers.Sort( CompareInputReceivers );
    }

    #region Unity events
    void Awake()
    {
        Check.Equal( 1, Find.SceneObjects<InputManager>().Length );//, "There should only be one InputManager per scene" );
        //inputReceivers.AddRange( Find.SceneObjects<InputReceiver>() );
    }

    void Start()
    {
        // Camera may be assigned during Awake.
        inputReceivers.Sort( CompareInputReceivers );
    }

    void Update()
    {
        if( touchState == TouchState.TapUp ) {
            // Clear touch of tap is up for too long
            if( currTouch.time != 0 && currTouch.time - lastTouch.time < doubleTapTimeDelta ) {
                currTouch.Reset();
            }

            Vector3 tapStart = TapStart();
            if( tapStart != Vector3.zero ) {
                touchState = TouchState.TapDown;
                currTouch.pos = tapStart;
                currTouch.time = Time.time;

                InputEvent inputEvent = MakeHitInputEvent();

                // Send messages
                if( currTouch.time - lastTouch.time < doubleTapTimeDelta ) {
                    SendDoubleTapMessage( inputEvent );

                }
                else {
                    SendSingleTapMessage( inputEvent );  // Sends both a single and double tap for reponsiveness
                }

                lastTouch = currTouch;
            }
        }
        else if( touchState == TouchState.TapDown ) {
                Vector3 tapDown = TapDown();
                currTouch.time = Time.time;
                if( tapDown != Vector3.zero ) {
                    if( !tapDown.Approx( lastTouch.pos ) ) {
                        currTouch.pos = tapDown;
                        SendDragMessage( MakeInputEvent() );  // No raycast for drag
                        lastTouch = currTouch;
                    }
                }
                else {
                    touchState = TouchState.TapUp;
                    currTouch.pos = Vector3.zero;
                    SendTapUpMessage( MakeInputEvent() );
                }
            }
    }
    #endregion

    #region Public static helpers
    public static RaycastHit Pick( Camera camera, Vector3 point )
    {
        Ray ray = camera.ScreenPointToRay( point );
        RaycastHit hit;
        if( Physics.Raycast( ray, out hit ) ) {
            return hit;
        }
        return DefaultRaycastHit;
    }

    #endregion

    #region Input helpers
    private Vector3 TapStart()
    {
        if( Input.GetMouseButtonDown( 0 ) ) {
            return Input.mousePosition;
        }
        if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) {
            return Input.touches[0].position;
        }
        return Vector3.zero;
    }

    private Vector3 TapDown()
    {
        if( Input.GetMouseButton( 0 ) ) {
            return Input.mousePosition;
        }

        if( Input.touchCount > 0 ) {
            if( Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary )
                return Input.touches[0].position;
        }
        return Vector3.zero;
    }
    #endregion

    #region Private helpers
    private void SendSingleTapMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnSingleTap", inputEvent );
    }

    private void SendDoubleTapMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnDoubleTap", inputEvent );
    }

    private void SendTapUpMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnTapUp", inputEvent );
    }

    private void SendDragMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnDrag", inputEvent, false );
    }

    private void DoSendMessage( string msgName, InputEvent inputEvent, bool pick=true )
    {
        //print( "Sending message: " + inputEvent );
        Transform pickedObject = null;
        Camera currPickCamera = null;
        foreach( InputReceiver recv in inputReceivers ) {
            // Receivers are sorted by camera depth, cycle to new camera if need be
            if( currPickCamera != recv.inputCamera ) {
                currPickCamera = recv.inputCamera;
                pickedObject = null; // reset the picked object for new camera
            }

            if( pickedObject == null && pick ) {
                pickedObject = Pick( currPickCamera, inputEvent.pos ).transform;
            }

            inputEvent.hit = pickedObject;

            if( pickedObject && FindChild( recv.transform, pickedObject.transform ) || !pick ) {
                recv.SendMessage( msgName, inputEvent, SendMessageOptions.DontRequireReceiver );
            }

            if( ScreenPointInRects( inputEvent.pos, recv.ignoreAreas ) )
                break; // areas on higher levels swallow input
        }
    }

    private bool ScreenPointInRects( Vector3 point3, Rect[] rects )
    {
        Vector2 point2 = point3.xy();
        point2.y = Screen.height - point2.y;  // Screen pos start start lower left.

        foreach( Rect r in rects ) {
            if( r.Contains( point2 ) )
                return true;
        }
        return false;
    }

    private InputEvent MakeInputEvent()
    {
        return new InputEvent() {
            lastPos = lastTouch.pos,
            pos = currTouch.pos,
            time = currTouch.time,
        };

    }

    private InputEvent MakeHitInputEvent()
    {
        return new InputEvent() {
            lastPos = lastTouch.pos,
            pos = currTouch.pos,
            time = currTouch.time,
        };
    }

    private bool FindChild( Transform parentTransform, Transform targetTransform )
    {
        if( parentTransform == targetTransform ) {
            return true;
        }
        foreach( Transform childTransform in parentTransform ) {
            if( childTransform == targetTransform )
                return true;
            if( FindChild( childTransform, childTransform ) )
                return true;
        }
        return false;
    }

    private static int CompareInputReceivers( InputReceiver recv1, InputReceiver recv2 )
    {
        return (int)(recv2.inputCamera.depth - recv1.inputCamera.depth);  // highest goes first
    }
    #endregion

    private struct InputInfo
    {
        public Vector3 pos;
        public float time;

        public void Reset()
        {
            pos = Vector3.zero;
            time = 0;
        }

        public override string ToString()
        {
            return string.Format( "Pos:{0} Time:{1}", pos, time );
        }
    }

}
