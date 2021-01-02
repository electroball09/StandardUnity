using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorUtil
{
    public static bool ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return true;
        }
    }
}
