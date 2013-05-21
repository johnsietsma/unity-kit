using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class SerializedPropertyExtensions
{
    /// <summary>
    /// Use reflection to find the object that s wrapped by this SerializedProperty.
    /// This is the equivalent of property.objectReferenceValue, but it works for types that don't derive from UnityEngine.Object.
    /// </summary>
    public static System.Object GetObjectValue( this SerializedProperty property )
    {
        string[] paths = property.propertyPath.Split( new char[] { '.' } );
        if( paths.Length==0 ) { return null; }

        // Get the UnityEngine.Object that contains this property.
        System.Object propertyObject = property.serializedObject.targetObject;

        // Drill down each component in the path to get the property
        foreach( string path in paths ) {
            FieldInfo fi = propertyObject.GetType().GetField( path );
            propertyObject = fi.GetValue( propertyObject );
        }

        return propertyObject;
    }
}
