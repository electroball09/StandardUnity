using UnityEngine;
using System.Collections;

public static class MathUtil
{
    public static float FarthestFromZero(float a, float b)
    {
        if (Mathf.Abs(a) > Mathf.Abs(b))
            return a;
        return b;
    }
}