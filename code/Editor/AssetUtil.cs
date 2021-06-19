using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetUtil
{
    public static T CreateAsset<T>(string path = "Assets/") where T : ScriptableObject
    {
        return CreateAsset(typeof(T), path) as T;
    }

    public static ScriptableObject CreateAsset(Type type, string path = "Assets/")
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

        return asset;
    }
}