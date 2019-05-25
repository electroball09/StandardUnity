using UnityEngine;
using System.Collections;
using System;

public static class StringUtil
{
    public static string GetEvenlySpacedTimeString()
    {
        string str = "";

        DateTime now = DateTime.Now;

        str += PaddedIntString(now.Hour, 2) + ":";
        str += PaddedIntString(now.Minute, 2) + ":";
        str += PaddedIntString(now.Second, 2) + ":";
        str += PaddedIntString(now.Millisecond, 3);

        return str;
    }

    public static string PaddedIntString(this int val, int length, string insert = "0")
    {
        string str = "";
        string vstr = val.ToString();
        if (vstr.Length < length)
        {
            for (int i = 0; i < length - vstr.Length; i++)
            {
                str += insert;
            }
        }
        str += vstr;
        return str;
    }

    public static string Concatenate(object[] data)
    {
        if (data == null)
            return string.Empty;

        string ret = "";
        for (int i = 0; i < data.Length; i++)
        {
            if (data != null)
                ret += data[i].ToString() + " ";
        }
        return ret;
    }
}

public static class StringExtension
{
    public static string GetFirst(this string source, int head_length)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        return source.Substring(0, Math.Min(source.Length, head_length));
    }

    public static string GetLast(this string source, int tail_length)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        if (tail_length >= source.Length)
            return source;
        return source.Substring(source.Length - tail_length);
    }

    public static string MinusFirst(this string source, int head_length)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        if (head_length >= source.Length)
            return source;
        return source.Substring(head_length);
    }

    public static string MinusLast(this string source, int tail_length)
    {
        if (string.IsNullOrEmpty(source))
            return source;
        if (tail_length >= source.Length)
            return source;
        return source.Remove(source.Length - tail_length);
    }
}
