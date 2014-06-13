using UnityEngine;
using Diag = System.Diagnostics;

public class UnityConsoleTraceListener : Diag.TraceListener
{

    public override void Write( string message )
    {
        Debug.Log( message );
    }

    public override void Write( object o )
    {
        Debug.Log( o.ToString() );
    }

    public override void Write( string message, string category )
    {
        Debug.Log( category + ": " + message );
    }

    public override void Write( object o, string category )
    {
        Debug.Log( category + ": " + o.ToString() );
    }

    public override void WriteLine( string message )
    {
        Debug.Log( message );
    }

    public override void WriteLine( object o )
    {
        Debug.Log( o.ToString() );
    }

    public override void WriteLine( string message, string category )
    {
        Debug.Log( category + ": " + message );
    }

    public override void WriteLine( object o, string category )
    {
        Debug.Log( category + ": " + o.ToString() );
    }
}
