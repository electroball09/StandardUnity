using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using System;

public class DearImguiStats : MonoBehaviour
{
    static bool debugStats = false;
    static bool p_open = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void init()
    {
        ImGuiUn.Layout += DoImgui;

        ComReg.AddCom("debugstats", () => debugStats = !debugStats, "toggles debug stats");
    }

    static void DoImgui()
    {
        if (!debugStats) return;

        ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDecoration
            | ImGuiWindowFlags.NoSavedSettings;
        ImGui.SetNextWindowBgAlpha(0.2f);
        ImGui.SetNextWindowPos(new Vector2(Screen.width - 300, 0));
        ImGui.SetNextWindowSize(new Vector2(300, 150));
        if (ImGui.Begin("debugstats", ref p_open, flags))
        {
            ImGui.Text($"Unity {Application.unityVersion}");
            ImGui.Text($"{Application.productName} - {Application.platform} - {SystemInfo.graphicsDeviceType}");
            ImGui.Text($"{SystemInfo.processorType}");
            ImGui.Text($"{SystemInfo.graphicsDeviceName} - {SystemInfo.graphicsMemorySize}MB");
            ImGui.Text($"{Environment.OSVersion}");
            ImGui.Text($"{Environment.MachineName} - {Environment.UserName}");
            ImGui.Text($"{1 / Time.deltaTime}fps  {Time.deltaTime}ms");

            ImGui.End();
        }
    }
}
