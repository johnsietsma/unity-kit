using UnityEngine;

/**
 * Basic information about an input event.
 */
public struct InputEvent
{
    public Vector3 lastPos;
    public Vector3 pos;
    public float time;
    public RaycastHit hit;

    public override string ToString()
    {
        if( hit.transform != null )
            return string.Format( "Input last pos:{0} pos:{1} time:{2} object:{3}", lastPos, pos, time, hit.transform.name );
        else
            return string.Format( "Input last pos:{0} pos:{1} time:{2} object:{3}", lastPos, pos, time, null );
    }
}
