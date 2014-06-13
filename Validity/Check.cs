using System.Collections;

public static class Check
{
 
    public static void Error( string msg )
    {
        throw new System.Exception( msg );
    }
 
    public static void False( bool test, string message="Not false" )
    {
        True( !test, message );
    }
     
    public static void True( bool test, string message="Not true" )
    {
        if( !test )
            Error( message );
    }

    public static void Equal( int a, int b, string message="" )
    {
        if( a != b ) {
            message = message != "" ? message : string.Format( "{0} is not equal to {1}", a, b );
            Error( message );
        }
    }

    public static void NotEqual( int a, int b, string message="" )
    {
        if( a == b ) {
            message = message != "" ? message : string.Format( "{0} is equal to {1}", a, b );
            Error( message );
        }
    }

    public static void GreaterThen( int a, int b, string message="" )
    {
        if( a <= b ) {
            message = message != "" ? message : string.Format( "{0} is not greater then {1}", a, b );
            Error( message );
        }
    }
 
    public static void GreaterThenOrEqual( int a, int b, string message="" )
    {
        if( a < b ) {
            message = message != "" ? message : string.Format( "{0} is not greater then or equal to {1}", a, b );
            Error( message );
        }
    }
 
    public static void LessThen( int a, int b, string message="" )
    {
        if( a >= b ) {
            message = message != "" ? message : string.Format( "{0} is not less then {1}", a, b );
            Error( message );
        }
    }

    public static void Null( object obj, string message="Object isn't null" )
    {
        True( obj == null, message );
    }

    public static void NotNull( object obj, string message="null object" )
    {
        True( obj != null, message );
    }
 
    public static void NotEmpty<T>( T[] array, string message="Empty array" )
    {
        True( array == null || array.Length != 0, message );
    }
 
    public static void NotNullOrEmpty<T>( T[] array, string message="Null or empty array" )
    {
        NotNull( array, message );
        NotEmpty( array, message );
    }
}
