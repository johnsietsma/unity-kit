using UnityEngine;
using System.Collections;

public class InputWorld : MonoBehaviour
{
    public void OnSingleTap( InputEvent inputEvent )
    {
        print( "World tapped " + inputEvent );
    }

    public void OnDrag( InputEvent inputEvent )
    {
        print( "Dragging " + inputEvent );
    }
}
