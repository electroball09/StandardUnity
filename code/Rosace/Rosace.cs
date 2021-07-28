using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.Profiling;

public interface IRosaceUpdate
{
    bool IsValidUpdater();
    void RosaceUpdate(Rosace.RosaceUpdateContext context);
    void PostRosaceUpdate(Rosace.RosaceUpdateContext context);
}

public class Rosace
{
    public static float delta = 0.00694f;
    public static float updateScale = 1f;
    public static float timeScale = 1f;
    public static float time { get; private set; }
    public static float realTime { get; private set; }
    public static float numUpdates { get; private set; }

    static PlayerLoopSystem.UpdateFunction updateFunction;
    static List<IRosaceUpdate> rosaceUpdaters = new List<IRosaceUpdate>();
    static List<IRosaceUpdate> pendingUpdaters = new List<IRosaceUpdate>();

    public struct RosaceUpdateContext
    {

        public static void Init()
        {
            if (updateFunction != null) return;

            updateFunction = RosaceUpdate.Update;

            Physics.autoSimulation = false;
            Physics.autoSyncTransforms = false;

            PlayerLoopInjector.InjectSubsystem<Rosace, RosaceUpdate, FixedUpdate>(updateFunction);
        }

        struct RosaceUpdate
        {
            static RosaceUpdateContext ctParams;
            static float lastUpdateTime = 0f;
            static float lastRosaceUpdateTime = 0f;
            //static float trackedRealTime = 0f;

            public static void Update()
            {
                if (!Application.isPlaying) return;

                if (updateScale < 1f)
                {
                    Debug.LogWarning("Rosace does not support update scales < 1.  Scale will be reset to 1");
                    updateScale = 1f;
                }

                realTime += Time.unscaledDeltaTime;

                rosaceUpdaters.AddRange(pendingUpdaters);
                pendingUpdaters.Clear();

                numUpdates = Mathf.FloorToInt((realTime - lastUpdateTime) / (delta / updateScale));

                ctParams = new RosaceUpdateContext()
                {
                    deltaTime = delta * timeScale,
                    constantMultiplier = (delta * timeScale) / 0.02f
                };

                for (int i = 0; i < numUpdates; i++)
                {
                    //Rosace.time = lastRosaceUpdateTime + (ctParams.deltaTime * i + ctParams.deltaTime);
                    Rosace.time += ctParams.deltaTime;

                    Physics.SyncTransforms();

                    Profiler.BeginSample("Rosace Update");
                    UpdateList(rosaceUpdaters);
                    Profiler.EndSample();

                    Physics.Simulate(ctParams.deltaTime);

                    Profiler.BeginSample("Rosace Post Update");
                    PostUpdateList(rosaceUpdaters);
                    Profiler.EndSample();
                }

                if (numUpdates > 0)
                {
                    lastUpdateTime = realTime;
                    lastRosaceUpdateTime = Rosace.time;
                }
            }

            public static void UpdateList(List<IRosaceUpdate> list)
            {
                foreach (var upd in list)
                    if (upd.IsValidUpdater())
                        upd.RosaceUpdate(ctParams);
            }

            public static void PostUpdateList(List<IRosaceUpdate> list)
            {
                foreach (var upd in list)
                    if (upd.IsValidUpdater())
                        upd.PostRosaceUpdate(ctParams);
            }
        }

        public float deltaTime { get; private set; }
        public float constantMultiplier { get; private set; }
        //public float time { get; private set; }

        public void UpdateGameObject(GameObject obj)
        {
            RosaceUpdate.UpdateList(obj.FastGetComponents<IRosaceUpdate>());
        }

        public void UpdateGameObject(IRosaceUpdate source, GameObject obj)
        {
            var l = obj.FastGetComponents<IRosaceUpdate>();
            l.Remove(source);
            RosaceUpdate.UpdateList(l);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void InitRosace()
    {
        RosaceUpdateContext.Init();

        ComReg.AddCom("rosace.updatescale", (float val) => Rosace.updateScale = val);
        ComReg.AddCom("rosace.timescale", (float val) => Rosace.timeScale = val);
        ComReg.AddCom("rosace.delta", (float val) => Rosace.delta = val);

        Debug.Log("ROSACE INJECTED");
    }

    public static void RegisterUpdater(IRosaceUpdate update)
    {
        if (rosaceUpdaters.Contains(update))
        {
            Debug.LogError($"Trying to register a rosace updater twice!");
            return;
        }

        pendingUpdaters.Add(update);
    }

    public static void DeregisterUpdater(IRosaceUpdate update)
    {
        rosaceUpdaters.Remove(update);
    }
}
