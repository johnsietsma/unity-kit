using System;
using NUnit.Core;
using UnityEngine;

public class ConsoleListener : EventListener
{
    public int numTests;
 
    public void RunStarted( string name, int testCount )
    {
        numTests = testCount;
        Debug.Log( "Running " + testCount + " tests in " + name );
    }

    public void RunFinished( TestResult result )
    {
        //Debug.Log( "Run finsished, success:" + result.IsSuccess);
    }

    public void RunFinished( Exception exception )
    {
        Debug.LogError( "Run exception:" + exception.Message );
    }

    public void TestStarted( TestName testName )
    {
        //Debug.Log( "Running test " + testName.Name );
    }

    public void TestFinished( TestResult result )
    {
        if( result.IsFailure ) {
            Debug.LogError( String.Format( "Failure in test {0}: {1}\n{2}", result.Name, result.Message, result.StackTrace ) );
        }
        else {
            //Debug.Log( "Test finished: " + result.Message );
        }
    }

    public void SuiteStarted( TestName testName )
    {
        //Debug.Log( "Suite started:" + testName.Name );
    }

    public void SuiteFinished( TestResult result )
    {
        //Debug.Log( "Suite finished, success:" + result.IsSuccess );
    }

    public void UnhandledException( Exception exception )
    {
        Debug.LogError( "Unhandled Exception: " + exception.Message );
    }

    public void TestOutput( TestOutput testOutput )
    {
        //log.Debug( "Test output: " + testOutput.Text );
    }
}
