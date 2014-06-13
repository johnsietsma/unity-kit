using NLog;
using NLog.Targets;
using UnityEngine;

namespace UnityKit
{
[Target("UnityConsole")]
public class UnityConsoleNLogTarget : TargetWithLayout
{
    protected override void Write( LogEventInfo logEvent )
    {
        string logMsg = this.Layout.Render( logEvent );
        if( logEvent.Level == LogLevel.Error ) {
            Debug.LogError( logMsg );
        }
        else if( logEvent.Level == LogLevel.Warn ) {
            Debug.LogWarning( logMsg );
        }
        else {
            Debug.Log( logMsg );
        }
    }
}
}

