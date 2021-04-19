using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDefault : MonoBehaviour
{
    public delegate void CursorToggledEvent(bool toggledOn);
    public static event CursorToggledEvent CursorToggled;

    public static bool consoleIsOpen = false;
    public delegate void ConsoleToggledEvent(bool toggledOn);
    public static event ConsoleToggledEvent ConsoleToggled;

    public static CursorDefault inst;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnLoad()
    {
        GameObject obj = new GameObject("CURSOR_DEFAULT");
        DontDestroyOnLoad(obj);
        inst = obj.AddComponent<CursorDefault>();

        ComReg.AddCom(inst, "tglcursor", ToggleCursor, "Toggles cursor active");

        CursorToggled += (on) => Debug.Log("Cursor toggled: " + on);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !consoleIsOpen)
        {
            ToggleCursor();
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleIsOpen = !consoleIsOpen;
            ConsoleToggled?.Invoke(consoleIsOpen);
        }
    }

    private static void ToggleCursor()
    {
        bool isOn = CursorUtil.ToggleCursor();
        CursorToggled?.Invoke(isOn);
    }
}
