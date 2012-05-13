using UnityEngine;
using System.Collections;

public class InputObject : MonoBehaviour
{
    public void OnSingleTap( InputEvent inputEvent )
    {
        print( "Object tapped " + inputEvent );
    }

    public void OnDrag( InputEvent inputEvent )
    {
        print( "Dragging " + inputEvent );
    }
}
