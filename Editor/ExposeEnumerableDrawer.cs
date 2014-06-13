using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

/// <summary>
/// Draws a IEnumerable as a property.
/// The field must be public, convertable to IEnumerable and marked as serializable.
/// </summary>
[CustomPropertyDrawer( typeof( ExposeEnumerableAttribute ) )]
public class ExposeEnumerableDrawer : PropertyDrawer
{
    private const int LINE_HEIGHT = 16;

    private bool enumIsOut = false;

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        float height = LINE_HEIGHT;
        if ( enumIsOut ) {
            var e = property.GetObjectValue() as IEnumerable;
            int count = 0;
            foreach ( object x in e ) { count++; }
            height += count * LINE_HEIGHT;
        }
        return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        var log = property.GetObjectValue() as IEnumerable;
        if ( log == null ) { return; }

        EditorGUI.BeginProperty ( position, label, property );

        enumIsOut = EditorGUI.Foldout( position, enumIsOut, property.name );

        if ( enumIsOut ) {
            EditorGUI.indentLevel++;
            Rect rect = new Rect( position );
            foreach ( object mr in log ) {
                rect.y += LINE_HEIGHT;
                EditorGUI.LabelField( rect, mr.ToString() );
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty ();
    }

    private static System.Object FindProperty( SerializedProperty property )
    {
        string propertyPath = property.propertyPath;
        string[] paths = propertyPath.Split( new char[] { '.' } );
        if ( paths.Length == 0 ) { return null; }

        System.Object propertyObject = property.serializedObject.targetObject;
        foreach ( string path in paths ) {
            FieldInfo fi = propertyObject.GetType().GetField( path );
            if ( fi != null ) {
                propertyObject = fi.GetValue( propertyObject );
            }
        }

        return propertyObject;
    }
}
