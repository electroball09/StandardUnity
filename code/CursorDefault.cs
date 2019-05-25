using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDefault : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnLoad()
    {
        GameObject obj = new GameObject("CURSOR_DEFAULT");
        DontDestroyOnLoad(obj);
        obj.AddComponent<CursorDefault>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            CursorUtil.ToggleCursor();
    }
}
