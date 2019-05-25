using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class EnumUtil
{
    public static TEnum Parse<TEnum>(string value, TEnum defaultEnum) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enumerated type");
        if (string.IsNullOrEmpty(value)) return defaultEnum;

        foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
        {
            if (item.ToString().ToLower().Equals(value.Trim().ToLower())) return item;
        }
        return defaultEnum;
    }
}

//[StructLayout(LayoutKind.Explicit)]
//public struct FastEnumConverter<T> where T : IConvertible
//{
//    [FieldOffset(0)]
//    public T Raw;
//    [FieldOffset(0)]
//    public sbyte SByte;
//    [FieldOffset(0)]
//    public byte Byte;
//    [FieldOffset(0)]
//    public short Short;
//    [FieldOffset(0)]
//    public ushort UShort;
//    [FieldOffset(0)]
//    public int Int;
//    [FieldOffset(0)]
//    public uint UInt;
//    [FieldOffset(0)]
//    public long Long;
//    [FieldOffset(0)]
//    public ulong ULong;
//    [FieldOffset(0)]
//    public float Float;
//}

//public static class FastEnumConvert
//{
//    public static sbyte ToSByte<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.SByte; }
//    public static byte ToByte<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.Byte; }
//    public static short ToShort<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.Short; }
//    public static ushort ToUShort<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.UShort; }
//    public static int ToInt32<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.Int; }
//    public static uint ToUInt32<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.UInt; }
//    public static long ToLong<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.Long; }
//    public static ulong ToULong<T>(this T value) where T : IConvertible { return new FastEnumConverter<T> { Raw = value }.ULong; }

//    public static T ToEnum<T>(this sbyte value) where T : IConvertible { return new FastEnumConverter<T> { SByte = value }.Raw; }
//    public static T ToEnum<T>(this byte value) where T : IConvertible { return new FastEnumConverter<T> { Byte = value }.Raw; }
//    public static T ToEnum<T>(this short value) where T : IConvertible { return new FastEnumConverter<T> { Short = value }.Raw; }
//    public static T ToEnum<T>(this ushort value) where T : IConvertible { return new FastEnumConverter<T> { UShort = value }.Raw; }
//    public static T ToEnum<T>(this int value) where T : IConvertible { return new FastEnumConverter<T> { Int = value }.Raw; }
//    public static T ToEnum<T>(this uint value) where T : IConvertible { return new FastEnumConverter<T> { UInt = value }.Raw; }
//    public static T ToEnum<T>(this long value) where T : IConvertible { return new FastEnumConverter<T> { Long = value }.Raw; }
//    public static T ToEnum<T>(this ulong value) where T : IConvertible { return new FastEnumConverter<T> { ULong = value }.Raw; }
//}