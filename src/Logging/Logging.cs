#define TRACE
using System;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using UnityEngine;

namespace UnityKit
{
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

public class Logging : MonoBehaviour
{
    public string[] logTargets = new string[] {
            "UnityKit.UnityConsoleNLogTarget",
            "UnityKit.TestFlightNLogTarget" };
    public LogSource[] logSources;
 
    void Awake()
    {
        LoggingConfiguration config = new LoggingConfiguration();

        List<TargetWithLayout> targets = new List<TargetWithLayout>();
        for( int i=0; i<logTargets.Length; i++ ) {
            Type t = Type.GetType( logTargets[i] );
            if( t == null ) {
                Debug.LogWarning( "Could not get logger target of type " + logTargets[i] );
                continue;
            }
            TargetWithLayout target = System.Activator.CreateInstance( t ) as TargetWithLayout;
            if( target == null ) {
                Debug.LogError( "Couldn't instantiate logger: " + logTargets[i] );
                continue;
            }
            target.Layout = new SimpleLayout( "[${logger}] ${message}" );
            targets.Add( target );
        }
        SplitGroupTarget splitTarget = new SplitGroupTarget( targets.ToArray() );


        config.AddTarget( "Split Target", splitTarget );
        LogManager.Configuration = config;

        for( int i=0; i<logSources.Length; i++ ) {
            string logName = logSources[i].name;
            LogLevel logLevel = ToLogLevel( logSources[i].logLevel );
            AddRule( config, logName, logLevel, splitTarget );
        }
     
        LogManager.Configuration = config;
    }

    void OnDestroy()
    {
        LogManager.Configuration = null;
    }

    private void AddRule( LoggingConfiguration config, string logName, LogLevel level, Target splitTarget )
    {
        LoggingRule rule = new LoggingRule( logName, level, splitTarget );
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
}