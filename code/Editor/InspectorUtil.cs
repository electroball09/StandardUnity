using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InspectorUtil
{
    const int btnWidth = 20;
    const int btnHeight = 20;
    const int txtWidth = 250;

    public static void DrawReorderableList<T>(List<T> list, Func<T, T> drawer, Action<T> removeFunc = null,
        Action<int, int> switchFunc = null, string name = "List", bool showAddButton = true, bool showReorderables = true)
    {
        GUILayout.Label(name + ":");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();

        if (list == null || list.Count == 0)
            GUILayout.Label("No items");

        int indexToRemove = -1;
        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (i != 0 && showReorderables)
            {
                if (GUILayout.Button("↑", GUILayout.Width(btnWidth), GUILayout.Height(btnHeight)))
                {
                    if (switchFunc != null)
                        switchFunc.Invoke(i, i - 1);
                    else
                        list.Switch(i, i - 1);
                }
            }
            else
            {
                GUILayout.Space(28);
            }

            if (i != list.Count - 1 && showReorderables)
            {
                if (GUILayout.Button("↓", GUILayout.Width(btnWidth), GUILayout.Height(btnHeight)))
                {
                    if (switchFunc != null)
                        switchFunc.Invoke(i, i + 1);
                    else
                        list.Switch(i, i + 1);
                }
            }
            else
            {
                GUILayout.Space(24);
            }

            if (GUILayout.Button("X", GUILayout.Width(btnWidth), GUILayout.Height(btnHeight)))
            {
                indexToRemove = i;
            }

            list[i] = drawer.Invoke(list[i]);

            EditorGUILayout.EndHorizontal();
        }

        if (indexToRemove != -1)
        {
            if (removeFunc == null)
                list.RemoveAt(indexToRemove);
            else
                removeFunc.Invoke(list[indexToRemove]);
        }

        if (showAddButton && GUILayout.Button("Add +", GUILayout.Height(btnHeight), GUILayout.Width(80)))
            list.Add(default(T));

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}