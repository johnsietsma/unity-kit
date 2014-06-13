// Adapted from: https://gist.github.com/onevcat/4036890

#define DEBUG_LEVEL_LOG
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR


using UnityEngine;
using System.Collections;


// setting the conditional to the platform of choice will only compile the method for that platform
// alternatively, use the #defines at the top of this file
public class D
{
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
    public static void Log( object format, params object[] paramList )
    {
        Debug.Log( MakeString( format, paramList ) );
    }


    [System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
    public static void Warn( object format, params object[] paramList )
    {
        Debug.LogWarning( MakeString( format, paramList ) );
    }


    [System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
    public static void Error( object format, params object[] paramList )
    {
        Debug.LogError( MakeString( format, paramList ) );
    }


    [System.Diagnostics.Conditional( "UNITY_EDITOR" )]
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
    public static void Assert( bool condition )
    {
        Assert( condition, string.Empty );
    }


    [System.Diagnostics.Conditional( "UNITY_EDITOR" )]
    [System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
    public static void Assert( bool condition, object format, params object[] paramList )
    {
        if ( !condition ) {
            Debug.LogError( "assert failed! " + MakeString( format, paramList ) );
        }
    }

    private static string MakeString( object format, params object[] paramList )
    {
        if ( format is string ) {
            return string.Format( format as string, paramList );
        }
        else {
            return format.ToString();
        }
    }
}
