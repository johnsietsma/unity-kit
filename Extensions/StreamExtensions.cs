using System.IO;

public static class StreamExtensions
{

    public static void Write (this Stream stream, byte[] buff)
    {
        if (stream == null || buff == null)
            return;
        stream.Write (buff, 0, buff.Length); 
    }
 
    public static byte[] Read (this Stream stream, int len)
    {
        if (stream == null)
            return null;
        byte[] buff = new byte[len];
        stream.Read (buff, 0, len);
        return buff;
    }
}
