using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public interface IRosaceUpdate
{
    bool IsValidUpdater();
    void RosaceUpdate(Rosace.RosaceUpdateContext context);
    void PostRosaceUpdate(Rosace.RosaceUpdateContext context);
}

public class Rosace
{
    static PlayerLoopSystem.UpdateFunction updateFunction;
    static List<IRosaceUpdate> rosaceUpdaters = new List<IRosaceUpdate>();

    public struct RosaceUpdateContext
    {
        public static void Init()
        {
            if (updateFunction != null) return;

            updateFunction = RosaceUpdate.Update;

            Physics.autoSimulation = false;

            PlayerLoopInjector.InjectSubsystem<Rosace, RosaceUpdate, FixedUpdate>(updateFunction);
        }

        struct RosaceUpdate
        {
            static RosaceUpdateContext ctParams;
            static float lastUpdateTime = 0f;

            public static void Update()
            {
                if (Time.time - lastUpdateTime < 0.02f)
                    return;

                lastUpdateTime = Time.time;

                ctParams = new RosaceUpdateContext()
                {
                    deltaTime = 0.02f,
                    constantMultiplier = 0.02f / 0.02f
                };

                UpdateList(rosaceUpdaters);

                Physics.Simulate(0.02f);

                PostUpdateList(rosaceUpdaters);
            }

            public static void UpdateList(IEnumerable<IRosaceUpdate> list)
            {
                foreach (var upd in list)
                    if (upd.IsValidUpdater())
                        upd.RosaceUpdate(ctParams);
            }

            public static void PostUpdateList(IEnumerable<IRosaceUpdate> list)
            {
                foreach (var upd in list)
                    if (upd.IsValidUpdater())
                        upd.PostRosaceUpdate(ctParams);
            }
        }

        public float deltaTime { get; private set; }
        public float constantMultiplier { get; private set; }

        public void UpdateGameObject(GameObject obj)
        {
            RosaceUpdate.UpdateList(obj.FastGetComponents<IRosaceUpdate>());
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void InitRosace()
    {
        RosaceUpdateContext.Init();

        Debug.Log("ROSACE INJECTED");
    }

    public static void RegisterUpdater(IRosaceUpdate update)
    {
        if (rosaceUpdaters.Contains(update))
        {
            Debug.LogError($"Trying to register a rosace updater twice!");
            return;
        }

        rosaceUpdaters.Add(update);
    }

    public static void DeregisterUpdater(IRosaceUpdate update)
    {
        rosaceUpdaters.Remove(update);
    }
}
