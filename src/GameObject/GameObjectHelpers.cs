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
}

