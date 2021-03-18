using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using System.Threading;

public class FrameLimiter
{
    public const float MIN_LIMIT_MS = 2f;
    public const float MAX_LIMIT_MS = 1000f;

    struct FrameLimiterUpdate
    {
        public static float limitMs = 16.6667f;
        public static bool enable = false;

        static float lastUpdateTime;

        public static void Update()
        {
            if (enable)
            {
                float diff = limitMs - (Time.deltaTime * 1000);
                Debug.Log($"{diff}   {limitMs}   {Time.deltaTime * 1000}");
                if (diff > 0)
                {
                    Thread.Sleep((int)diff);
                }
            }
            lastUpdateTime = Time.time;
        }
    }

    public static bool Enabled
    {
        get { return FrameLimiterUpdate.enable; }
        set { FrameLimiterUpdate.enable = value; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void InitFrameLimiter()
    {
        PlayerLoopInjector.InjectSubsystem<FrameLimiter, FrameLimiterUpdate, Initialization>(FrameLimiterUpdate.Update);
    }

    public static void SetLimitMilliseconds(float ms)
    {
        if (ms < 0)
        {
            Debug.LogError($"Cannot set framerate limiter to less than 0 ms! ({ms} ms)");
            return;
        }

        if (ms > MAX_LIMIT_MS)
        {
            Debug.LogError($"Cannot set framerate limiter to more than {MAX_LIMIT_MS} ms! ({ms} ms)");
            return;
        }

        FrameLimiterUpdate.limitMs = ms;
    }

    public static void SetLimitFPS(int fps)
    {
        SetLimitMilliseconds((1f / fps) * 1000);
    }
}
