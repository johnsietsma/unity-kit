using UnityEngine;

/**
 * Basic information about an input event.
 */
public struct InputInfo
{
    public Vector3 pos;
    public float time;
    public RaycastHit hit;

    public override string ToString()
    {
        if( hit.transform != null )
            return string.Format( "Input pos:{0} time:{1} object:{2}", pos, time, hit.transform.name );
        else
            return string.Format( "Input pos:{0} time:{1} object:{2}", pos, time, null );
    }
}
