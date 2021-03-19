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
        public string CommandSource { get; set; }

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

    static ComReg()
    {
        AddCom("help", () => LogComs(), "Prints all commands");
        AddCom("helpcom", (string s) => LogComDesc(s), "Prints help about a command");
        AddCom("log", (string s) => Debug.Log(s), "Prints to the log");
        AddCom("warn", (string s) => Debug.LogWarning(s), "Prints a warning");
        AddCom("error", (string s) => Debug.LogError(s), "Prints an error");
    }

    public static void AddCom(object sourceObj, string name, MethodInfo methodInfo, string desc)
    {
        if (name.IndexOf(' ') != -1)
        {
            Debug.LogWarning($"Cannot have spaces in command! ({name})");
            return;
        }

        foreach (var p in methodInfo.GetParameters())
        {
            if (ConvertUtil.GetConverter(p.ParameterType, true) == default)
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
            parameters = methodInfo.GetParameters() != null ? methodInfo.GetParameters() : new ParameterInfo[0],
            CommandSource = Environment.StackTrace
        };

        if (registeredCommands.ContainsKey(name))
        {
            Debug.LogWarning($"Double registration of command {name}\nCommand 1: {registeredCommands[name].CommandSource}\nCommand 2: {c.CommandSource}");
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

        if (c.parameters.Length == 1 && c.parameters[0].ParameterType == typeof(string))
        {
            if (vars.Length == 1)
            {
                Debug.LogError("Needs to have a string!");
                return;
            }
            Debug.Log($"runnning com: {commandString}");
            c.method.Invoke(c.sourceObject, new object[] { commandString.MinusFirst(command.Length + 1) });
            return;
        }

        if (vars.Length - 1 != c.parameters.Length)
        {
            Debug.LogError($"Parameter mismatch, params should be:  {c.GetParamsTypeStr()}");
            return;
        }

        Debug.Log($"runnning com: {commandString}");

        object[] objParams = new object[c.parameters.Length];
        for (int i = 0; i < objParams.Length; i++)
        {
            objParams[i] = ConvertUtil.ConvertFromStr(c.parameters[i].ParameterType, vars[i + 1]);
        }

        c.method.Invoke(c.sourceObject, objParams);
    }

    public static void LogComs()
    {
        Debug.Log("Type \"helpcom [command]\" to see more info");
        Debug.Log($"Registered commands [{registeredCommands.Count}]:");
        foreach (var com in registeredCommands.Values)
        {
            Debug.Log($" {com.name} - {com.GetParamsTypeStr()}");
        }
    }

    public static void LogComDesc(string cmd)
    {
        string[] vars = cmd.Split(' ');

        if (!registeredCommands.ContainsKey(vars[0]))
        {
            Debug.LogError("Please input a command to help with");
            return;
        }

        Com c = registeredCommands[vars[0]];

        Debug.Log($"{c.name} - {c.GetParamsTypeStr()}\n  -{c.desc}");
    }
}
