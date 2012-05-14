#define TRACE
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

// LogLevel is not serializable and can't be used in the Unity Editor.
public enum LogLevelEnum
{
    Off,
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal
};

[Serializable]
public class LogSource
{
    public string name;
    public LogLevelEnum logLevel;
}

[Target("UnityConsole")]
public class UnityConsoleTarget : TargetWithLayout
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

public class Logging : MonoBehaviour
{
 
    public LogSource[] logSources;
 
    void Awake()
    {
        UnityConsoleTarget unityTarget;
        LoggingConfiguration config = new LoggingConfiguration();
        unityTarget = new UnityConsoleTarget();
        unityTarget.Layout = new SimpleLayout( "[${logger}] ${message}" );
        config.AddTarget( "UnityConsole", unityTarget ); // LogManager.Configuration.AddTarget() doesn't work
        LogManager.Configuration = config;

        for( int i=0; i<logSources.Length; i++ ) {
            string logName = logSources[i].name;
            LogLevel logLevel = ToLogLevel( logSources[i].logLevel );
            AddRule( config, logName, logLevel, unityTarget );
        }
     
        LogManager.Configuration = config;
    }
 
    private void AddRule( LoggingConfiguration config, string logName, LogLevel level, UnityConsoleTarget target )
    {
        LoggingRule rule = new LoggingRule( logName, level, target );
        config.LoggingRules.Add( rule );
    }
 
    private LogLevel ToLogLevel( LogLevelEnum ll )
    {
        switch( ll ) {
        case LogLevelEnum.Debug: 
            return LogLevel.Debug;
        case LogLevelEnum.Error: 
            return LogLevel.Error;
        case LogLevelEnum.Fatal: 
            return LogLevel.Fatal;
        case LogLevelEnum.Info: 
            return LogLevel.Info;
        case LogLevelEnum.Off: 
            return LogLevel.Off;
        case LogLevelEnum.Trace: 
            return LogLevel.Trace;
        case LogLevelEnum.Warn: 
            return LogLevel.Warn;
        default:
            return LogLevel.Off;
        }
    }
}
