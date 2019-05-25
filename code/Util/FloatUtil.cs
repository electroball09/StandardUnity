using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension
{
    public static bool Between(this float a, float x, float y)
    {
        if (x > y)
        {
            float t = x;
            x = y;
            y = t;
        }
        return a > x && a <= y;
    }
}
