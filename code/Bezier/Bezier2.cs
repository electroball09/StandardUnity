using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bezier2
{
    public Vector2 p0;
    public Vector2 p1;
    public Vector2 p2;

    public Bezier2(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    public Vector2 Sample(float t)
    {
        Vector2 c1 = Mathf.Pow(1 - t, 2) * p0;
        Vector2 c2 = 2 * (1 - t) * t * p1;
        Vector2 c3 = Mathf.Pow(t, 2) * p2;
        return c1 + c2 + c3;
    }
}