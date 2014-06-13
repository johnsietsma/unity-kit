using System;

public class Filter
{
    public static float HighPass( float prevFiltered, float currRaw, float prevRaw, float smoothingFactor )
    {
        return smoothingFactor * (prevFiltered + currRaw - prevRaw);
    }

    public static float LowPass( float prevFiltered, float currRaw, float smoothingFactor )
    {
        return prevFiltered + smoothingFactor * (currRaw - prevFiltered);
    }
}

