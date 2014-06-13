using System;

public static class StringExtensions
{
    public static T ParseEnum<T>( this string enumValue )
    {
        try {
            return (T)Enum.Parse( typeof(T), enumValue );
        } catch( ArgumentException ex ) {
            //D.LogError(  );
            throw new Exception(
                String.Format ("\"{0}\" is an invalid value. Accepted values are {1}", enumValue, String.Join (",", Enum.GetNames (typeof(T)))),
                ex
            );
        }
    }
}

#if UNIT_TESTS

using NUnit.Framework;

[TestFixture]
public class TestStringExtensions {
    private enum EnumTest { One, Two };

    [Test]
    public void TestParseEnum()
    {
        EnumTest et1 = "One".ParseEnum<EnumTest>();
        Assert.AreEqual( et1, EnumTest.One );
    }

    [Test]
    [ExpectedException( typeof(Exception))]
    public void TestParseEnumException()
    {
        "Three".ParseEnum<EnumTest>( );
    }
}
#endif
