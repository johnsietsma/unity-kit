using System;
using System.Collections.Generic;

public static class IEnumerableExtensions
{
    /*public static int Count<T>( this IEnumerable<T> e )
    {
        return new List<T>( e ).Count;
    }*/

    public static string ToStringJoin<T>( this IEnumerable<T> e, string joinString="," )
    {
        List<T> data = new List<T>( e );
        List<string> stringData = data.ConvertAll<string>( d => d.ToString() );
        return String.Join( joinString, stringData.ToArray() );
    }

    public static void ForEach<T>( this IEnumerable<T> enumeration, Action<T> action )
    {
        foreach( T item in enumeration ) {
            action( item );
        }
    }

    public static int Sum<T>( this IEnumerable<T> items, Func<T,int> p ) {
        int ret = 0;
        items.ForEach( c=>ret+=p(c));
        return ret;
    }
}

