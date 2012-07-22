using NLog;
using NLog.Targets;
using TestFlightUnity;

namespace TestFlightUnity
{
[Target("TestFlight")]
public class TestFlightNLogTarget : TargetWithLayout
{
    protected override void Write( LogEventInfo logEvent )
    {
        //string logMsg = this.Layout.Render( logEvent );
            // TODO: This isn't working and should be in the TestFlight project.
        //TestFlight.Log( logMsg );
    }
}
}
