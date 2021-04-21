using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Autoexec
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        Debug.Log(File.Exists("autoexec.cfg"));
        if (File.Exists("autoexec.cfg"))
        {
            var lines = File.ReadAllLines("autoexec.cfg");
            foreach (string str in lines)
                Debug.Log($"autoexec - {str}");
            foreach (string str in lines)
                ComReg.RunCom(str);
        }
    }
}
