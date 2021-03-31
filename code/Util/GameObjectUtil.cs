using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasSubcomponents
{
    void AppendComponents<T>(List<T> list);
}

public static class GameObjectUtil
{
    static Dictionary<Type, IList> componentListCache = new Dictionary<Type, IList>();
    static List<IHasSubcomponents> subCmpList = new List<IHasSubcomponents>();

    /// <summary>
    /// <i>DO NOT KEEP A REFERENCE TO THE LIST RETURNED BY THIS METHOD</i>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="inChildren"></param>
    /// <returns></returns>
    public static List<T> FastGetComponents<T>(this GameObject obj, bool inChildren = true)
    {
        List<T> list;
        Type subType = typeof(T);
        if (componentListCache.ContainsKey(subType))
            list = (List<T>)componentListCache[subType];
        else
        {
            list = new List<T>();
            componentListCache.Add(subType, list);
        }

        if (inChildren)
        {
            obj.GetComponentsInChildren(true, list);
            obj.GetComponentsInChildren(true, subCmpList);
        }
        else
        {
            obj.GetComponents(list);
            obj.GetComponents(subCmpList);
        }

        foreach (var subCmp in subCmpList)
            subCmp.AppendComponents(list);

        return list;
    }

    public static void SendMessage<T>(this GameObject obj, Action<T> message)
    {
        foreach (T cmp in obj.FastGetComponents<T>())
        {
            message(cmp);
        }
    }
}
