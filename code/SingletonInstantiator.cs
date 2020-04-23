using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonInstantiator : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void LoadConsole()
    {
        GameObject console = Resources.Load("IngameDebugConsole") as GameObject;

        Instantiate(console);
        DontDestroyOnLoad(console);
    }
}
