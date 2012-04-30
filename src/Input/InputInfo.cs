using UnityEngine;

/**
 * Basic information about an input event.
 */
public struct InputInfo
{
    public Vector3 startPos;
    public Vector3 pos;
    public float time;
    public RaycastHit hit;

    public void Reset()
    {
        startPos = Vector3.zero;
        pos = Vector3.zero;
        time = 0;
        hit = new RaycastHit();
    }

    public override string ToString()
    {
        if( hit.transform != null )
            return string.Format( "Input start pos:{0} pos:{1} time:{2} object:{3}", startPos, pos, time, hit.transform.name );
        else
            return string.Format( "Input start pos:{0} pos:{1} time:{2} object:{3}", startPos, pos, time, null );
    }
}
