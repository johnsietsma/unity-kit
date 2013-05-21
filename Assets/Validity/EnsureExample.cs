using UnityEngine;
using System;


/**
 * Ensure classes are about failing early. It is doesn't exist we want to fail then, not when we first use it.
 * The calls are also a little more terse then the Unity equivalents.
 */
public class EnsureExample : MonoBehaviour
{
    public GameObject examplePrefabAssigned;
    public GameObject examplePrefabNotAssigned;

    void Start()
    {
        DoSceneObjects();
        DoComponents();
        DoInstantiate();
        DoChild();
    }

    void DoSceneObjects()
    {
        // This works
        Ensure.SceneObject<ExampleMonoBehaviour1>();
        print( "Found ExampleMonoBehaviour1 in scene" );

        // This returns null because it's not in the scene
        Find.SceneObject<ExampleMonoBehaviour2>();
        print( "Didn't find ExampleMonoBehaviour2 in scene" );

        // This doesn't exist in the scene and will now, rather then when first referenced.
        try {
            Ensure.SceneObject<ExampleMonoBehaviour2>();
        }
        catch( Exception ) {
            print( "Didn't ensure ExampleMonoBehaviour2 in scene" );
        }
    }

    void DoComponents()
    {
        GameObject exampleParent = GameObject.Find( "ExampleParent" );

        // This works because it exists
        Ensure.Component<ExampleMonoBehaviour1>( exampleParent );
        print( "Found ExampleMonoBehaviour1 component" );

        // This returns null
        Find.Component<ExampleMonoBehaviour2>( exampleParent );
        print( "Didn't find ExampleMonoBehaviour2 component" );

        // This throws and exception
        try {
            Ensure.Component<ExampleMonoBehaviour2>( exampleParent );
        }
        catch( Exception ) {
            print( "Didn't ensure ExampleMonoBehaviour2 component" );
        }
    }

    void DoInstantiate()
    {
        // This works
        Ensure.Instantiate( examplePrefabAssigned );
        print( "Successfully instantiated" );

        // This throws because it's not assigned
        try {
            Ensure.Instantiate( examplePrefabNotAssigned );
        }
        catch( Exception ) {
            print( "Not instantiated" );
        }
    }

    void DoChild()
    {
        GameObject exampleParent = GameObject.Find( "ExampleParent" );
        Ensure.Child( exampleParent.transform, "ExampleChild" );
        print( "Found the child" );
    }
}
