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

        }
    }
}
