using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FrameLimitTester : MonoBehaviour
{
    public int limit = 60;
    public bool enable = false;

    void Update()
    {
        limit = Mathf.Clamp(limit, 2, 500);
        FrameLimiter.SetLimitFPS(limit);
        FrameLimiter.Enabled = enable;
    }
}
