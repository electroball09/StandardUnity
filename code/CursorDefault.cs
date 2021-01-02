using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDefault : MonoBehaviour
{
    public delegate void CursorToggledEvent(bool toggledOn);
    public static event CursorToggledEvent CursorToggled;

    private bool consoleIsOpen = false;
    public delegate void ConsoleToggledEvent(bool toggledOn);
    public static event ConsoleToggledEvent ConsoleToggled;
    
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
        {
            bool isOn = CursorUtil.ToggleCursor();
            CursorToggled?.Invoke(isOn);
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleIsOpen = !consoleIsOpen;
            ConsoleToggled?.Invoke(consoleIsOpen);
        }
    }
}
