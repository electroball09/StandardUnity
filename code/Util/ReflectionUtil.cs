using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel;

public static class ReflectionUtil
{
    public static object CreateInstanceFromString(string _type)
    {
        Type type = Type.GetType(_type);
        if (type == null)
            return null;

        return Activator.CreateInstance(type);
    }

    public static bool InheritsFrom<T>(this Type type)
    {
        bool isCorrectType = false;
        Type lastType = type;
        while (type != null)
        {
            if (typeof(T) == type)
            {
                isCorrectType = true;
                break;
            }
            lastType = type.BaseType;
        }
        return isCorrectType;
    }
}
