using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

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

    private List<InputReceiver> inputReceivers = new List<InputReceiver>();

    private enum TouchState
    {
        TapUp,
        Dragging,
        Pinching,
        TapDown
    };

    private InputInfo currTouch = new InputInfo();
    private InputInfo lastTouch = new InputInfo();
    private TouchState touchState = TouchState.TapUp;
    private Logger log;

    public void AddInputReceiver( InputReceiver inputReceiver )
    {
        log.Trace( "Adding input receiver: " + inputReceiver.name + " with cam " + inputReceiver.inputCamera );
        inputReceivers.Add( inputReceiver );
        inputReceivers.Sort( CompareInputReceivers );
    }

    public void RemoveInputReceiver( InputReceiver inputReceiver )
    {
        inputReceivers.Remove( inputReceiver );
    }

    #region Unity events
    void Awake()
    {
        log = LogManager.GetLogger( "Input" );
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
			UpdateUpState();
		} else if( touchState == TouchState.TapDown ) {
			UpdateDownState();
		} else if( touchState == TouchState.Dragging ) {
			UpdateDragState();
		} else if( touchState == TouchState.Pinching ) {
			UpdatePinchState();
		}
    }

    private void UpdateUpState()
	{
		// Clear touch of tap is up for too long
		if( currTouch.time != 0 && currTouch.time - lastTouch.time < doubleTapTimeDelta ) {
			EndTouchInput();
        }

        Vector2 tapStart;
        if( TapStart( out tapStart ) ) {
	        touchState = TouchState.TapDown;
	        currTouch.pos = tapStart;
	        currTouch.time = Time.time;
	  
	        StartTouchInput( tapStart );
	        SendTapDownMessage( MakeHitInputEvent() );
        }
    }

    private void UpdateDownState()
    {
        Vector2 tapDown;
		bool isDown = TapDown( out tapDown );
        currTouch.time = Time.time;
        if( isDown ) {
        	if ( ( tapDown - currTouch.pos ).magnitude > deadZone ) {
		        touchState = TouchState.Dragging;
		        lastTouch = currTouch;
            }
        } else {
            // tap up
            if( lastTouch.time != 0 && currTouch.time - lastTouch.time < doubleTapTimeDelta ) {
                SendDoubleTapMessage( MakeHitInputEvent() );
            } else {
                SendSingleTapMessage( MakeHitInputEvent() );
            }
            SendTapUpMessage( MakeHitInputEvent() );
            lastTouch = currTouch;

            touchState = TouchState.TapUp;
            currTouch.pos = Vector3.zero;
            SendTapMessage( MakeInputEvent() );
        }
	}

    private void UpdateDragState()
    {
        Vector2 tapDown;
        if( TapDown( out tapDown ) == false ) {
			touchState = TouchState.TapUp;
			SendTapUpMessage( MakeHitInputEvent() );
			lastTouch = currTouch;
        } else if( (tapDown - currTouch.pos).magnitude > deadZone ) {
            currTouch.pos = tapDown;
            SendDragMessage( MakeInputEvent() );  // No raycast for drag
            lastTouch = currTouch;
        }
    }
    
    private void UpdatePinchState()
    {
    	
    }
    #endregion

    #region Public static helpers


    #endregion

    #region Input helpers
    private bool TapStart( out Vector2 position )
    {
        if( Input.GetMouseButtonDown( 0 ) ) {
            position = Input.mousePosition;
            return true;
        }
        if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ) {
            position = Input.touches[0].position;
            return true;
        }
        position = Vector2.zero;
        return false;
    }

    private bool TapDown( out Vector2 position )
    {
        if( Input.GetMouseButton( 0 ) ) {
            position = Input.mousePosition;
            return true;
        }

        if( Input.touchCount > 0 && (
                Input.touches[0].phase == TouchPhase.Moved ||
                Input.touches[0].phase == TouchPhase.Stationary ) ) {
        	position = Input.touches[0].position;
        	return true;
        }
        position = Vector2.zero;
        return false;
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

    private void SendTapDownMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnTapDown", inputEvent );
    }

    private void SendTapUpMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnTapUp", inputEvent );
    }

    private void SendTapMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnTap", inputEvent );
    }

    private void SendDragMessage( InputEvent inputEvent )
    {
        DoSendMessage( "OnDrag", inputEvent );
    }

    private void DoSendMessage( string msgName, InputEvent inputEvent )
    {
        log.Trace( "Attempting to send message: " + msgName + " cam: " + currTouch.cam );
        inputEvent.hit = currTouch.hit;

        if( currTouch.receiver != null ) {
            log.Trace( "Sending message " + msgName + " to " + currTouch.receiver.name );
            currTouch.receiver.SendMessage( msgName, inputEvent, SendMessageOptions.DontRequireReceiver );
        }

        IEnumerable<InputReceiver> globalReceivers = inputReceivers.FindAll( ir => ir != null && ir.receiveAllInput == true );
        globalReceivers.ForEach( ir => {
            log.Trace( "Sending message " + msgName + " to " + ir.name );
            ir.SendMessage( msgName, inputEvent, SendMessageOptions.DontRequireReceiver );
        } );
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

    private void StartTouchInput( Vector3 pos )
    {
        IEnumerable<InputReceiver> validInputReceviers = inputReceivers.FindAll( ir => !ScreenPointInRects( pos, ir.ignoreAreas ) );
        log.Trace( "Hit test start touch against " + validInputReceviers.Count().ToString() + " receivers." );
        foreach( InputReceiver inputReceiver in validInputReceviers ) {
            RaycastHit hit = Picker.Pick( inputReceiver.inputCamera, pos );
            currTouch.hit = hit.transform;
            if( hit.transform != null && FindChild( inputReceiver.transform, hit.transform ) ) {
                currTouch.cam = inputReceiver.inputCamera;
                currTouch.receiver = inputReceiver.transform;
                log.Trace( "Start input chain. Pos: " + pos + " Hit: " + currTouch.hit + " receiver: " + currTouch.receiver );
                return;
            }
        }
        log.Trace( "Didn't find any hit targets." );
    }

    private void EndTouchInput()
    {
        log.Trace( "End input chain" );
        currTouch.Reset();
    }

    private bool FindChild( Transform parentTransform, Transform targetTransform )
    {
        if( parentTransform == targetTransform ) {
            return true;
        }
        foreach( Transform childTransform in parentTransform ) {
            if( childTransform == targetTransform )
                return true;
            if( FindChild( childTransform, targetTransform ) ) {
                return true;
            }
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
        public Vector2 pos;
        public float time;
        public Transform hit;
        public Transform receiver;
        public Camera cam;

        public void Reset()
        {
            pos = Vector2.zero;
            time = 0;
            hit = null;
            receiver = null;
            cam = null;
        }

        public override string ToString()
        {
            return string.Format( "Pos:{0} Time:{1}", pos, time );
        }
    }

}
