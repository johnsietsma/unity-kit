using UnityEngine;
using System.Collections;

public static class Find
{
    public static T SceneObject<T>() where T : Object
    {
        return Object.FindObjectOfType( typeof( T ) ) as T;
    }

    public static T[] SceneObjects<T>() where T : Object
    {
        return Object.FindObjectsOfType( typeof( T ) ) as T[];
    }

    public static T Component<T>( Component c ) where T : Component
    {
        return c.GetComponent<T>();
    }

    public static T Component<T>( GameObject go ) where T : Component
    {
        return go.GetComponent<T>();
    }
}

