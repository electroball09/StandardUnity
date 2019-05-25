using UnityEngine;
using System.Collections;

public class Vector3Util
{
    public static Vector3 Average(params Vector3[] vectors)
    {
        Vector3 average = Vector3.zero;
        for (int i = 0; i < vectors.Length; i++)
        {
            average += vectors[i];
        }
        return (average / vectors.Length);
    }

    public static float Angle(Vector3 a, Vector3 b)
    {
        float angle = Vector3.Angle(a, b);
        float y = Vector3.Cross(a, b).y;
        if (y < 0) angle = -angle;
        return angle;
    }
}

public static class Vector3UtilExtension
{
    public static Vector3 Lerp(this Vector3 origin, Vector3 target, float alpha)
    {
        return Vector3.Lerp(origin, target, alpha);
    }

    public static Vector3 LerpOffset(this Vector3 origin, Vector3 target, float alpha)
    {
        return Vector3.Lerp(origin, target, alpha) - origin;
    }

    public static Vector3 NormalizeXZ(this Vector3 vector)
    {
        vector.y = 0;
        return vector.normalized;
    }

    public static float Length(this Vector3 vector)
    {
        return Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y) + (vector.z * vector.z));
    }
}
