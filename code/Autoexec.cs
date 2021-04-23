using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Autoexec
{
    static string[] defaultAutoExec = new string[]
    {
        "rosace.update true",
        "debugstats"
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        Debug.Log(File.Exists("autoexec.cfg"));
        if (File.Exists("autoexec.cfg"))
        {
            ProcessLines(File.ReadAllLines("autoexec.cfg"));
        }
#if UNITY_WINRT && !UNITY_EDITOR
        ProcessLines(defaultAutoExec);
#endif
    }

    static void ProcessLines(string[] lines)
    {
        foreach (string str in lines)
            Debug.Log($"autoexec - {str}");
        foreach (string str in lines)
            ComReg.RunCom(str);
    }
}
