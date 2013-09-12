using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject EnsureChild( this GameObject go, string childName ) {
        return Ensure.Child( go, childName );
    }

    public static T EnsureComponent<T>( this GameObject go ) where T:Component {
        return Ensure.Component<T>( go );
    }

    public static bool HasComponent<T>( this GameObject go ) where T:Component {
        return go.GetComponent<T>()!=null;
    }

    public static T EnsureChildHasComponent<T>( this GameObject go ) where T:Component {
        GameObject goComp = go.Descendants().FirstOrDefault( c=>c.HasComponent<T>() ) as GameObject;
        Check.NotNull( goComp, go.name + " has no child with component " + typeof(T).Name );
        return goComp.GetComponent<T>();
    }

    public static IEnumerable<GameObject> Descendants( this GameObject go ) {
        return GameObjectHelpers.Descendants( go );
    }

    public static IEnumerable<GameObject> Parents( this GameObject go ) {
        return go.transform.Parents().Select( t => t.gameObject );
    }

    public static T GetComponentInParents<T>( this GameObject go ) where T : Component {
        GameObject goParent = go.Parents().FirstOrDefault( g=>g.HasComponent<T>() );
        return goParent==null ? null : goParent.GetComponent<T>();
    }

    public static void SetChildrenActive( this GameObject go, bool active ) {
        foreach( Transform childTransform in go.transform ) {
            childTransform.gameObject.SetActive( active );
        }
    }
}

