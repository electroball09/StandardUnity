using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public interface IDebugViz
{
    void DrawDebugViz();
}

public class DearImguiDebugViz : MonoBehaviour
{
    static List<DearImguiDebugViz> vizs = new List<DearImguiDebugViz>();
    static bool p_open = true;
    static bool doDebugViz = true;

    static DearImguiDebugViz()
    {
        ImGuiUn.Layout += ImGuiUn_Layout;

        ComReg.AddCom("debugviz", () => doDebugViz = !doDebugViz, "toggles debug viz window");
    }

    private static void ImGuiUn_Layout()
    {
        if (!doDebugViz) return;

        ImGuiWindowFlags flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.HorizontalScrollbar;
        ImGui.SetNextWindowBgAlpha(0.35f);
        ImGui.SetNextWindowSize(new Vector2(300, 400));
        if (ImGui.Begin("debug viz", ref p_open, flags))
        {
            ImGuiTabBarFlags f = ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton;
            ImGui.BeginTabBar("debug_viz_tab", f);
            foreach (var v in vizs)
            {
                if (ImGui.BeginTabItem(v.gameObject.name))
                {
                    v.DrawDebugViz();
                    ImGui.EndTabItem();
                }
            }
            ImGui.EndTabBar();
        }
    }

    void Awake()
    {
        vizs.Add(this);
    }

    void OnDestroy()
    {
        vizs.Remove(this);
    }

    public void DrawDebugViz()
    {
        bool _active = gameObject.activeInHierarchy;
        ImGui.Checkbox("gameobject active", ref _active);
        if (_active != gameObject.activeInHierarchy)
            gameObject.SetActive(_active);

        ImGui.Spacing();

        ImGuiTabBarFlags f = ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton;
        if (ImGui.BeginTabBar(gameObject.name, f))
        {
            gameObject.SendMessage<IDebugViz>
                ((cmp) =>
                {
                    if (ImGui.BeginTabItem(cmp.GetType().Name))
                    {
                        cmp.DrawDebugViz();
                        ImGui.EndTabItem();
                    }
                });
            ImGui.EndTabBar();
        }
    }
}
