using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using NUnit.Core;

public class NUnitRunner : MonoBehaviour
{

    [MenuItem( "Tests/Run Tests %t" )]
    static void RunTests()
    {

        string assemFile = GetAssembly();
        ConsoleListener l = new ConsoleListener();

        TestResult result = RunTests( assemFile, l );
        if( result.IsSuccess ) {
            Debug.Log( String.Format( "{0} tests successful", l.numTests ) );
        }
        else {
            Debug.LogError( "Test failed" );
        }
    }

    private static string GetAssembly()
    {
        // The calling Assembly is in memory, so get the path and add the known file name back on.
        Assembly assem = Assembly.GetCallingAssembly();
        string assemFile = NUnit.Core.AssemblyHelper.GetAssemblyPath( assem );
        string dir = Path.GetDirectoryName( assemFile );
        return Path.Combine( dir, "Assembly-CSharp.dll" );
    }

    private static TestResult RunTests( string assemFile, ConsoleListener l )
    {
        TestPackage testPackage = new TestPackage( assemFile );
        RemoteTestRunner remoteTestRunner = new RemoteTestRunner();
        remoteTestRunner.Load( testPackage );
        return remoteTestRunner.Run( l, TestFilter.Empty, false, LoggingThreshold.Debug );
    }
}
