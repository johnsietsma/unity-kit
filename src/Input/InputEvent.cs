using UnityEngine;

/**
 * Basic information about an input event.
 */
public struct InputEvent
{
    public Vector3 lastPos;
    public Vector3 pos;
    public float time;
    public Transform hit;

    public override string ToString()
    {
        return string.Format( "Input last pos:{0} pos:{1} time:{2} object:{3}", lastPos, pos, time, hit );
    }
}
