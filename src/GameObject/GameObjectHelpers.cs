using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectHelpers
{
    /**
     * Return IEnumerable<GameObject> with all descendants including itself.
     * */
    public static IEnumerable<GameObject> Descendants( GameObject go ) {
        yield return go;
        foreach( Transform childTransform in go.transform ) {
            yield return childTransform.gameObject;
        }
    }

    public static IEnumerable<GameObject> Parents( GameObject go ) {
        while( go.transform.parent!=null ) {
            yield return go.transform.parent.gameObject;
            go = go.transform.parent.gameObject;
        }
    }
}

