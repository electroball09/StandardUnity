using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using System;

public class DearImguiStats : MonoBehaviour
{
    static bool debugStats = false;
    static bool p_open = true;
    static Rect guiRect;
    static GUIStyle style;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void init()
    {
        ImGuiUn.Layout += DoImgui;
        style = new GUIStyle();
        style.padding = new RectOffset(4, 0, 0, 0);

        ComReg.AddCom("debug.stats", () => debugStats = !debugStats, "toggles debug stats");
    }

    static void DoImgui()
    {
#if !UNITY_WINRT
        DrawGui();
#endif
    }

#if UNITY_WINRT
    private void OnGUI()
    {
        Color old = GUI.contentColor;
        GUI.contentColor = Color.white;
        DrawGui();
        GUI.contentColor = old;
    }
#endif

    static void DrawGui()
    {
        if (!debugStats) return;

        guiRect = new Rect(0, 0, 0, 0);
        if (BeginWindow("debugstats", Screen.width - 400, 0, 400, 150))
        {
            DrawDebugStats(0);
            EndWindow();
        }
    }

    static void DrawDebugStats(int id)
    {
        Text($"Unity {Application.unityVersion}");
        Text($"{Application.productName} - {Application.platform} - {SystemInfo.graphicsDeviceType}");
        Text($"{SystemInfo.processorType}");
        Text($"{SystemInfo.graphicsDeviceName} - {SystemInfo.graphicsMemorySize}MB");
        Text($"{Environment.OSVersion}");
        Text($"{Environment.MachineName} - {Environment.UserName}");
        Text($"{1 / Time.deltaTime}fps  {Time.deltaTime}ms  time {Time.time}");
        Text($"rosace time {Rosace.time} delta {Rosace.delta} upd {Rosace.numUpdates} real {Rosace.realTime}");
    }

    static bool BeginWindow(string name, float x, float y, float width, float height)
    {
#if !UNITY_WINRT
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDecoration
            | ImGuiWindowFlags.NoSavedSettings;
        ImGui.SetNextWindowBgAlpha(0.2f);
        ImGui.SetNextWindowPos(new Vector2(x, y));
        ImGui.SetNextWindowSize(new Vector2(width, height));
        return ImGui.Begin(name, ref p_open, flags);
#else
        GUI.Window(420, new Rect(x, y, width, height), DrawDebugStats, name);
        return false;
#endif
    }

    static void Text(string str)
    {
#if UNITY_WINRT
        guiRect.y += 15;
        guiRect.width = 400;
        guiRect.height = 15;
        GUI.Label(guiRect, str, style);
#else
        ImGui.Text(str);
#endif
    }

    static void EndWindow()
    {
#if !UNITY_WINRT
        ImGui.End();
#endif
    }
}
