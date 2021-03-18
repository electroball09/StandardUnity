using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class PlayerLoopInjector
{
    static PlayerLoopSystem playerLoop;

    public static bool InjectSubsystem<TSystemType, TUpdateType, TInjectBeforeType>(PlayerLoopSystem.UpdateFunction updateFunction)
    {
        Debug.Log($"Injecting subsystem {typeof(TSystemType).Name}.{typeof(TUpdateType).Name} before subsystem {typeof(TInjectBeforeType).Name}");

        playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        PlayerLoopSystem injectSystem = new PlayerLoopSystem()
        {
            subSystemList = new PlayerLoopSystem[1]
            {
                new PlayerLoopSystem()
                {
                    updateDelegate = updateFunction,
                    type = typeof(TUpdateType)
                }
            },
            type = typeof(TSystemType)
        };

        PlayerLoopSystem[] subsystems = new PlayerLoopSystem[playerLoop.subSystemList.Length + 1];

        int injectIndex = -1;
        for (int i = 0; i < subsystems.Length; i++)
        {
            if (playerLoop.subSystemList[i].type == typeof(TSystemType))
            {
                Debug.LogError($"Already injected system of type {typeof(TSystemType).Name}");
                return false;
            }

            if (playerLoop.subSystemList[i].type == typeof(TInjectBeforeType))
            {
                injectIndex = i;
                subsystems[i] = injectSystem;
                break;
            }

            subsystems[i] = playerLoop.subSystemList[i];
        }

        if (injectIndex == -1)
        {
            Debug.LogError($"Could not inject because a system of type {typeof(TInjectBeforeType).Name} was not found!");
            return false;
        }

        Array.Copy(playerLoop.subSystemList, injectIndex, subsystems, injectIndex + 1, playerLoop.subSystemList.Length - injectIndex);

        playerLoop.subSystemList = subsystems;

        PlayerLoop.SetPlayerLoop(playerLoop);

        return true;
    }
}
