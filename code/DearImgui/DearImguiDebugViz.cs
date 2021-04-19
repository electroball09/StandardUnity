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
    static List<DearImguiDebugViz> vizs;
    static bool p_open = true;
    static bool doDebugViz = true;

    static DearImguiDebugViz()
    {
        if (vizs != null) return;
        vizs = new List<DearImguiDebugViz>();

        ImGuiUn.Layout += ImGuiUn_Layout;

        ComReg.AddCom("debugviz", () => doDebugViz = !doDebugViz, "toggles debug viz window");
    }

    private static void ImGuiUn_Layout()
    {
        if (!doDebugViz) return;

        ImGuiWindowFlags flags = ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoFocusOnAppearing
            | ImGuiWindowFlags.HorizontalScrollbar;
        ImGui.SetNextWindowBgAlpha(0.35f);
        ImGui.SetNextWindowSize(new Vector2(400, 300), ImGuiCond.FirstUseEver);
        if (ImGui.Begin("debug viz", ref p_open, flags))
        {
            ImGuiTabBarFlags f = ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton;
            if (ImGui.BeginTabBar("debug_viz_tab", f))
            {
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

            ImGui.End();
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
        if (ImGui.Checkbox("gameobject active", ref _active))
            gameObject.SetActive(_active);

        ImGui.Spacing();

        ImGuiTabBarFlags f = ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton 
            | ImGuiTabBarFlags.FittingPolicyDefault | ImGuiTabBarFlags.Reorderable;
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
