using UnityEngine;
using System.Collections;

public class InputGUI : MonoBehaviour
{
    private InputReceiver inputRecv;
    private Rect guiArea1 = new Rect( 10, 10, 200, 100 );

    void Awake()
    {
        inputRecv = Ensure.Component<InputReceiver>( this );
        inputRecv.ignoreAreas = new Rect[] { guiArea1 };
    }

    void OnGUI()
    {
        GUI.BeginGroup( guiArea1 );
        if( GUI.Button( new Rect( 0, 0, 200, 100 ), "Press Me!" ) ) {
            print( "Pressed" );
        }
        GUI.EndGroup();
    }
}
