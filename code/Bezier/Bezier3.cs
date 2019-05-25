using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bezier3
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;

    public Bezier3(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    public Vector3 Sample(float t)
    {
        Vector3 c1 = Mathf.Pow(1 - t, 2) * p0;
        Vector3 c2 = 2 * (1 - t) * t * p1;
        Vector3 c3 = Mathf.Pow(t, 2) * p2;
        return c1 + c2 + c3;
    }
}