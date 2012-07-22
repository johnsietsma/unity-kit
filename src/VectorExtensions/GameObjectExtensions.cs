using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject EnsureChild( this GameObject go, string childName ) {
        return Ensure.Child( go, childName );
    }

    public static T EnsureComponent<T>( this GameObject go ) where T:Component {
        return Ensure.Component<T>( go );
    }
}

