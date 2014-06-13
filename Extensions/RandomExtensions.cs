using System;
using System.Collections.Generic;

public static class RandomExtensions
{
    public static T Next<T> (this Random r, List<T> list)
    {
        if (r==null || list==null || list.Count == 0)
            return default(T);
        int randIndex = r.Next (list.Count);
        return list [randIndex];
    }
}

#if UNIT_TESTS
using NUnit.Framework;

[TestFixture]
public class TestRandomExtensions
{
    [Test]
    public void TestEmptyNext ()
    {
        List<int> emptyList = new List<int> ();
        Random r = new Random (1);
        int i = r.Next (emptyList);
        Assert.AreEqual (0, i);
    }
}
#endif
