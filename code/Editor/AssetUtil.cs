using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetUtil
{
    public static void CreateAsset<T>(string path = "Assets/") where T : ScriptableObject
    {
        CreateAsset(typeof(T), path);
    }

    public static void CreateAsset(Type type, string path = "Assets/")
    {
        ScriptableObject asset = ScriptableObject.CreateInstance(type);
        if (asset == null)
        {
            throw new Exception("Asset is null, type: " + type.FullName);
        }

        string pathName = path + type.Name + ".asset";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        AssetDatabase.CreateAsset(asset, pathName);
        AssetDatabase.Refresh();
        Selection.activeObject = asset;

        Debug.Log("Created asset: " + pathName);
    }
}