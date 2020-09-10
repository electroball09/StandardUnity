using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using UnityEditor;

public class PlayerLoopInspector : EditorWindow
{
    static Vector2 scrollPos = Vector2.zero;

    [MenuItem("Tools/Player Loop Inspector")]
    static void ShowWindow()
    {
        GetWindow<PlayerLoopInspector>();
    }

    static void DrawSystem(ref PlayerLoopSystem loop)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(loop.type != null ? loop.type.ToString() : "null" );
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(8);
        EditorGUILayout.BeginVertical();
        if (loop.subSystemList != null)
            for (int i = 0; i < loop.subSystemList.Length; i++)
                DrawSystem(ref loop.subSystemList[i]);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    void OnGUI()
    {
        PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        DrawSystem(ref loop);
        EditorGUILayout.EndScrollView();
    }
}
