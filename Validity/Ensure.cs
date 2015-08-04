using UnityEngine;

public static class Ensure
{
    public static T SceneObject<T>() where T : Object
    {
        T t = Find.SceneObject<T>();
        Check.NotNull( t, "Object of type " + typeof(T).Name + " not found" );
        return t;
    }

    // Just like GetComponent(), but will log an error if the Component is not found.
    public static T GetComponentChecked<T>( this GameObject go ) where T : Component
    {
        T t = go.GetComponent<T>( );
        Check.NotNull( t, "Component of type " + typeof(T).Name + " not found not found in GameObject " + go.name );
        return t;
    }

    // Just like GetComponent(), but will log an error if the Component is not found.
    public static T GetComponentChecked<T>(this MonoBehaviour mb) where T : Component
    {
        T t = mb.GetComponent<T>();
        Check.NotNull( t, "Component of type " + typeof(T).Name + " not found in Monobehaviour " + mb.name );
        return t;
    }

    // Just like GetComponentInChildren(), but will log an error if the Component is not found.
    public static T GetComponentInChildrenChecked<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponentInChildren<T>();
        Check.NotNull(t, "Component of type " + typeof(T).Name + " not found not found in children of GameObject " + go.name);
        return t;
    }

    // Just like GetComponentInChildren(), but will log an error if the Component is not found.
    public static T GetComponentInChildrenChecked<T>(this MonoBehaviour mb) where T : Component
    {
        T t = mb.GetComponentInChildren<T>();
        Check.NotNull(t, "Component of type " + typeof(T).Name + " not found in children of Monobehaviour " + mb.name);
        return t;
    }

    // Just like GetComponentInParent(), but will log an error if the Component is not found.
    public static T GetComponentInParentChecked<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponentInParent<T>();
        Check.NotNull(t, "Component of type " + typeof(T).Name + " not found in parents of GameObject " + go.name);
        return t;
    }

    // Just like GetComponentInParent(), but will log an error if the Component is not found.
    public static T GetComponentInParentChecked<T>(this MonoBehaviour mb) where T : Component
    {
        T t = mb.GetComponentInParent<T>();
        Check.NotNull(t, "Component of type " + typeof(T).Name + " not found in parents of Monobehaviour " + mb.name);
        return t;
    }


    // Just like GetComponent(), but will add the Component if it is not found.
    public static T GetComponentEnsured<T>(this MonoBehaviour mb) where T : Component
    {
        T t = mb.GetComponent<T>();
        if( t==null ) {
            t = mb.gameObject.AddComponent<T>();
        }
        return t;
    }

    public static T InstantiateChecked<T>( T prefab ) where T : Object
    {
        return InstantiateChecked<T>(prefab, Vector3.zero, Quaternion.identity);
    }

    public static T InstantiateChecked<T>(T prefab, Vector3 pos) where T : Object
    {
        return InstantiateChecked<T>(prefab, pos, Quaternion.identity);
    }

    public static T InstantiateChecked<T>(T prefab, Vector3 pos, Quaternion rot) where T : Object
    {
        Check.NotNull( prefab, "The " + typeof(T).Name + " prefab hasn't been set" );
        T obj = Object.Instantiate( prefab, pos, rot ) as T;
        Check.NotNull( prefab, "The " + typeof(T).Name + " prefab can't be instantiated." );
        return obj;
    }

    public static Transform GetChildChecked( this Transform transform, string name )
    {
        Transform child = transform.Find( name );
        Check.NotNull( child, "Child \"" + name + "\" of " + transform.name + " doesn't exist" );
        return child;
    }

    public static GameObject GetChildChecked(GameObject gameObject, string name)
    {
        return GetChildChecked(gameObject.transform, name).gameObject;
    }

    public static Transform GetParentChecked( Transform transform, string parentName )
    {
        Transform t = transform;
        while( t!=null ) {
            if( t.name==parentName )
                return t;
            t = t.parent;
        }
        Check.Error( "Parent " + parentName + " of " + transform.name + " doesn't exist" );
        return null;;
    }
}