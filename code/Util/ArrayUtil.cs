using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtil
{
    /// <summary>
    /// Executes a specified routine on an array
    /// </summary>
    /// <typeparam name="T">Array type</typeparam>
    /// <param name="_array">Array to manipulate</param>
    /// <param name="_action">Function to run on each object in the array</param>
    public static void DoAction<T>(this T[] _array, Action<T> _action)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _action.Invoke(_array[i]);
        }
    }

    /// <summary>
    /// Sets each object in an array to the value returned from the specified routine
    /// </summary>
    /// <typeparam name="T">Array type</typeparam>
    /// <param name="_array">Array to manipulate</param>
    /// <param name="_func">Function that returns the new value for each object in the array</param>
    public static void DoAction<T>(this T[] _array, Func<T, T> _func)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = _func.Invoke(_array[i]);
        }
    }

    /// <summary>
    /// Executes a specified routine on a list
    /// </summary>
    /// <typeparam name="T">List type</typeparam>
    /// <param name="_array">List to manipulate</param>
    /// <param name="_action">Function to run on each object in the List</param>
    public static void DoAction<T>(this List<T> _list, Action<T> _action)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _action.Invoke(_list[i]);
        }
    }

    /// <summary>
    /// Sets each object in a list to the value returned from the specified routine
    /// </summary>
    /// <typeparam name="T">List type</typeparam>
    /// <param name="_list">List to manipulate</param>
    /// <param name="_func">Function that returns the new value for each object in the array</param>
    public static void DoAction<T>(this List<T> _list, Func<T, T> _func)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i] = _func.Invoke(_list[i]);
        }
    }

    public static void Switch<T>(this List<T> _list, int ind1, int ind2)
    {
        T obj = _list[ind1];
        _list[ind1] = _list[ind2];
        _list[ind2] = obj;
    }
}
