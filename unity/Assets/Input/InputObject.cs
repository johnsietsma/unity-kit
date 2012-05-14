using UnityEngine;
using System.Collections;

public class InputObject : MonoBehaviour
{
    public void OnSingleTap( InputEvent inputEvent )
    {
        print( "Single tap " + inputEvent );
    }

    public void OnDoubleTap( InputEvent inputEvent )
    {
        print( "Double tap " + inputEvent );
    }

    public void OnTapDown( InputEvent inputEvent )
    {
        print( "Tap down " + inputEvent );
    }

    public void OnTapUp( InputEvent inputEvent )
    {
        print( "Tap up " + inputEvent );
    }

    public void OnDrag( InputEvent inputEvent )
    {
        print( "Dragging " + inputEvent );
    }
}
