using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonInstantiator : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void LoadConsole()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            GameObject console = Resources.Load("IngameDebugConsole") as GameObject;

            Instantiate(console);
            DontDestroyOnLoad(console);
        }
    }
}
