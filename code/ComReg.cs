using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;

public static class ComReg
{
    class Com
    {
        public object sourceObject { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public MethodInfo method { get; set; }
        public ParameterInfo[] parameters { get; set; }

        public string GetParamsTypeStr()
        {
            if (parameters.Length == 0)
                return "[no params]";
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < parameters.Length - 1; i++)
                str.Append(parameters[i].ParameterType.Name + ", ");
            str.Append(parameters[parameters.Length - 1].ParameterType.Name);
            return str.ToString();
        }
    }

    static readonly Dictionary<string, Com> registeredCommands = new Dictionary<string, Com>();

    public static void AddCom(object sourceObj, string name, MethodInfo methodInfo, string desc)
    {
        if (name.IndexOf(' ') != -1)
        {
            Debug.LogWarning($"Cannot have spaces in command! ({name})");
            return;
        }

        foreach (var p in methodInfo.GetParameters())
        {
            if (ConvertUtil.GetConverter(p.ParameterType, true) == default(Func<string, object>))
            {
                Debug.LogError($"Command {name} tried to register invalid parameter type {p.ParameterType}");
                return;
            }
        }

        Com c = new Com()
        {
            sourceObject = sourceObj != null ? sourceObj : Activator.CreateInstance(methodInfo.DeclaringType),
            name = name,
            desc = desc,
            method = methodInfo,
            parameters = methodInfo.GetParameters() != null ? methodInfo.GetParameters() : new ParameterInfo[0]
        };

        if (registeredCommands.ContainsKey(name))
        {
            Debug.LogWarning($"Double registration of command {name}");
            registeredCommands[name] = c;
        }
        else
        {
            registeredCommands.Add(name, c);
        }
    }

    public static void AddCom(string name, Action act, string desc = "no description")
    {
        AddCom(null, name, act, desc);
    }

    public static void AddCom<T1>(string name, Action<T1> act, string desc = "no description")
    {
        AddCom(null, name, act, desc);
    }

    public static void AddCom<T1, T2>(string name, Action<T1, T2> act, string desc = "no description")
    {
        AddCom(null, name, act, desc);
    }

    public static void AddCom<T1, T2, T3>(string name, Action<T1, T2, T3> act, string desc = "no description")
    {
        AddCom(null, name, act, desc);
    }

    public static void AddCom<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> act, string desc = "no description")
    {
        AddCom(null, name, act, desc);
    }

    public static void AddCom(this object sourceObj, string name, Action act, string desc = "no description")
    {
        AddCom(sourceObj, name, act.Method, desc);
    }

    public static void AddCom<T1>(this object sourceObj, string name, Action<T1> act, string desc = "no description")
    {
        AddCom(sourceObj, name, act.Method, desc);
    }

    public static void AddCom<T1, T2>(this object sourceObj, string name, Action<T1, T2> act, string desc = "no description")
    {
        AddCom(sourceObj, name, act.Method, desc);
    }

    public static void AddCom<T1, T2, T3>(this object sourceObj, string name, Action<T1, T2, T3> act, string desc = "no description")
    {
        AddCom(sourceObj, name, act.Method, desc);
    }

    public static void AddCom<T1, T2, T3, T4>(this object sourceObj, string name, Action<T1, T2, T3, T4> act, string desc = "no description")
    {
        AddCom(sourceObj, name, act.Method, desc);
    }

    public static void RunCom(string commandString)
    {
        string[] vars = commandString.Split(' ');
        string command = vars[0];
       
        if (!registeredCommands.ContainsKey(command))
        {
            Debug.LogError($"Command {command} is not registered!");
            return;
        }

        Com c = registeredCommands[command];

        if (vars.Length - 1 != c.parameters.Length)
        {
            Debug.LogError($"Parameter mismatch, params should be:  {c.GetParamsTypeStr()}");
            return;
        }

        object[] objParams = new object[c.parameters.Length];
        for (int i = 0; i < objParams.Length; i++)
        {
            objParams[i] = ConvertUtil.ConvertFromStr(c.parameters[i].ParameterType, vars[i + 1]);
        }

        c.method.Invoke(c.sourceObject, objParams);
    }
}
