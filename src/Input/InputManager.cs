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

                // Fill in touch info
                currTouch.startPos = tapStart;
                currTouch.pos = tapStart;
                currTouch.time = Time.time;
                currTouch.hit = Raycast( tapStart );

                // Send messages
                if( currTouch.time - lastTouch.time < doubleTapTimeDelta ) {
                    SendDoubleTapMessage( currTouch );

                }
                else {
                    SendSingleTapMessage( currTouch );  // Sends both a single and double tap for reponsiveness
                }

                lastTouch = currTouch;
            }
        }

        if( touchState == TouchState.TapDown ) {
            Vector3 tapDown = TapDown();
            if( tapDown != Vector3.zero ) {
                currTouch.pos = tapDown;
                currTouch.time = Time.time;
                // no raycast for drag, leave initial hit
                SendDragMessage( currTouch );
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
    private void SendSingleTapMessage( InputInfo touchInfo )
    {
        DoSendMessage( "OnSingleTap", touchInfo );
    }

    private void SendDoubleTapMessage( InputInfo touchInfo )
    {
        DoSendMessage( "OnDoubleTap", touchInfo );
    }

    private void SendDragMessage( InputInfo touchInfo )
    {
        DoSendMessage( "OnDrag", touchInfo );
    }

    private void DoSendMessage( string msgName, InputInfo touchInfo )
    {
        foreach( InputReceiver recv in inputReceivers ) {
            recv.SendMessage( msgName, touchInfo, SendMessageOptions.DontRequireReceiver );
            if( ScreenPointInRects( touchInfo.pos, recv.ignoreAreas ) )
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

    private static int CompareInputReceivers( InputReceiver recv1, InputReceiver recv2 )
    {
        return recv2.level - recv1.level;  // highest goes first
    }
    #endregion
}
