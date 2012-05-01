using UnityEngine;
using System;

/**
 * Central manager of all input.
 * Sends input messages to all recievers, allows ordering and swallowing of messages.
 * Raycasts to provide hit info.
 * Uses both touch and mouse input.
 *
 *
 */
public class InputManager : MonoBehaviour
{
    public float doubleTapTimeDelta = 0.2f;
    public float deadZone = 0.01f;
    public Camera raycastCamera;
    private enum TouchState
    {
        TapUp,
        TapDown
    };
    private InputReceiver[] inputReceivers;
    private InputInfo currTouch = new InputInfo();
    private InputInfo lastTouch = new InputInfo();
    private TouchState touchState = TouchState.TapUp;
    private static readonly RaycastHit DefaultRaycastHit = new RaycastHit();

    #region Unity events
    void Awake()
    {
        inputReceivers = Find.SceneObjects<InputReceiver>();
        Array.Sort( inputReceivers, CompareInputReceivers );

        if( raycastCamera == null ) {
            raycastCamera = Camera.main;
        }
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
                if( tapDown != Vector3.zero ) {
                    if( !tapDown.Approx( lastTouch.pos ) ) {
                        currTouch.pos = tapDown;
                        currTouch.time = Time.time;
                        SendDragMessage( MakeInputEvent() );  // No raycast for drag
                        lastTouch = currTouch;
                    }
                }
                else {
                    touchState = TouchState.TapUp;
                }
            }
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

    private void SendDragMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnDrag", inputEvent );
    }

    private void DoSendMessage( string msgName, InputEvent inputEvent )
    {
        //print( "Sending message: " + inputEvent );
        foreach( InputReceiver recv in inputReceivers ) {
            recv.SendMessage( msgName, inputEvent, SendMessageOptions.DontRequireReceiver );
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

    private RaycastHit Raycast( Vector3 point )
    {
        Ray ray = raycastCamera.ScreenPointToRay( point );
        RaycastHit hit;
        if( Physics.Raycast( ray, out hit ) ) {
            return hit;
        }
        return DefaultRaycastHit;
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
            hit = Raycast( currTouch.pos )
        };
    }

    private static int CompareInputReceivers( InputReceiver recv1, InputReceiver recv2 )
    {
        return recv2.level - recv1.level;  // highest goes first
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
