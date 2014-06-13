using System.Collections.Generic;

public static class ListExtensions
{
    /*public static string ToStringJoin<T> (this List<T> list)
    {
        return list.ToStringJoin<T>();
    }*/

    public static void Remove<T> (this List<T> list, IEnumerable<T> toRemove)
    {
        if (list == null || toRemove == null)
            return;
        
        foreach (T item in toRemove) {
            list.Remove (item);
        }
    }    
}
