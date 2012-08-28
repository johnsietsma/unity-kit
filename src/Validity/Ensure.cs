using UnityEngine;

public static class Ensure
{
    public static T SceneObject<T>() where T : Object
    {
        T t = Find.SceneObject<T>();
        Check.NotNull( t, "Object of type " + typeof(T).Name + " not found" );
        return t;
    }

    public static T Component<T>( Component c ) where T : Component
    {
        T t = Find.Component<T>( c );
        Check.NotNull( t, "Component of type " + typeof(T).Name + " not found" );
        return t;
    }

    public static T Component<T>( GameObject go ) where T : Component
    {
        T t = Find.Component<T>( go );
        Check.NotNull( t, "Component of type " + typeof(T).Name + " not found" );
        return t;
    }

    public static T HasComponent<T>( GameObject go ) where T : Component
    {
        T t = Find.Component<T>( go );
        if( t==null ) {
            t = go.AddComponent<T>();
        }
        return t;
    }

    public static T Instantiate<T>( T prefab ) where T : Object
    {
        return Instantiate<T>( prefab, Vector3.zero, Quaternion.identity );
    }

    public static T Instantiate<T>( T prefab, Vector3 pos ) where T : Object
    {
        return Instantiate<T>( prefab, pos, Quaternion.identity );
    }

    public static T Instantiate<T>( T prefab, Vector3 pos, Quaternion rot ) where T : Object
    {
        Check.NotNull( prefab, "The " + typeof(T).Name + " prefab hasn't been set" );
        T obj = Object.Instantiate( prefab, pos, rot ) as T;
        Check.NotNull( prefab, "The " + typeof(T).Name + " prefab can't be instantiated." );
        return obj;
    }

    public static Transform Child( Transform transform, string name )
    {
        Transform child = transform.Find( name );
        Check.NotNull( child, "Child \"" + name + "\" of " + transform.name + " doesn't exist" );
        return child;
    }

    public static GameObject Child( GameObject gameObject, string name )
    {
        return Child( gameObject.transform, name ).gameObject;
    }

    public static Transform Parent( Transform transform, string parentName )
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