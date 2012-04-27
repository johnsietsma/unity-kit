using UnityEngine;
using System.Collections;

public class InputWorld : MonoBehaviour
{
    public void OnSingleTap( InputInfo info )
    {
        print( "World tapped " + info );
    }
}
