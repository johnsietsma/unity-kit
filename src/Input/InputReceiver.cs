using UnityEngine;
using System.Collections;

public class InputReceiver : MonoBehaviour
{
    public int level;  // higher level receivers get messages first

    [HideInInspector]
    public Rect[] ignoreAreas = new Rect[0];
}