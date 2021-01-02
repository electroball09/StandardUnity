using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class Rosace
{
    static PlayerLoopSystem playerLoop;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void InitRosace()
    {
        Debug.Log("ROSACE INIT");

        playerLoop = PlayerLoop.GetDefaultPlayerLoop();

        PlayerLoopSystem[] newSystems = new PlayerLoopSystem[playerLoop.subSystemList.Length + 1];
        Array.Copy(playerLoop.subSystemList, newSystems, playerLoop.subSystemList.Length);

        PlayerLoopSystem rosaceSystem = new PlayerLoopSystem()
        {
            subSystemList = new PlayerLoopSystem[1]
            {
                new PlayerLoopSystem()
                {
                    type = typeof(RosaceUpdate),
                    updateDelegate = RosaceUpdate.Update
                }
            },
            type = typeof(Rosace)
        };

        for (int i = 0; i < newSystems.Length; i++)
        {
            if (newSystems[i].type == typeof(FixedUpdate))
            {
                for (int j = newSystems.Length - 1; j > i; j--)
                {
                    newSystems[j] = newSystems[j - 1];
                }

                newSystems[i] = rosaceSystem;

                break;
            }
        }

        playerLoop.subSystemList = newSystems;

        PlayerLoop.SetPlayerLoop(playerLoop);

        Debug.Log("ROSACE INJECTED");
    }

    struct RosaceUpdate
    {
        public static void Update()
        {

        }
    }
}
