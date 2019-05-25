using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class CommandLine
{
    public const string ARG_BEGIN = @"/";
#if UNITY_EDITOR
    public const string DEBUG_ARGS = @"";
#endif

    private static ReadOnlyCollection<string> m_args = null;
    public static ReadOnlyCollection<string> args { get { return m_args; } }

#if UNITY_EDITOR
    static CommandLine()
    {
        List<string> tempArgs = new List<string>();
        foreach (string str in DEBUG_ARGS.Split(' '))
        {
            if (str.StartsWith(ARG_BEGIN))
            {
                tempArgs.Add(str);
            }
        }
        m_args = tempArgs.AsReadOnly();
    }
#else
    static CommandLine()
    {
        List<string> tempArgs = new List<string>();
        foreach (string str in System.Environment.GetCommandLineArgs())
        {
            if (str.StartsWith(ARG_BEGIN))
                tempArgs.Add(str);
        }
        m_args = tempArgs.AsReadOnly();
    }
#endif

    public static bool HasArg(string arg)
    {
        if (arg.StartsWith(ARG_BEGIN))
            return m_args.Contains(arg);
        return m_args.Contains(ARG_BEGIN + arg);
    }
}