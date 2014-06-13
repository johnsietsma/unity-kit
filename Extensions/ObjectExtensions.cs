using System;
using System.Collections.Generic;

public static class ObjectExtensions
{
    public static bool IsDefault<T>( this T obj ) {
        return EqualityComparer<T>.Default.Equals( obj );
    }
}

