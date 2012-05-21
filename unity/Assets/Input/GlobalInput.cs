using UnityEngine;
using System.Collections;

public class GlobalInput : MonoBehaviour
{

    public void OnSingleTap( InputEvent inputEvent )
    {
        print( "Single tap: " + inputEvent );
    }

    public void OnDoubleTap( InputEvent inputEvent )
    {
        print( "Double tap: " + inputEvent );
    }
    
}
