using Godot;
using System;

public partial class BitmaskExtensions : Node
{
    public static bool IsSet<T>(T flags, T flag) where T : struct
    {
        Int64 flagsValue = (Int64)(object)flags;
        Int64 flagValue = (Int64)(object)flag;

        return (flagsValue & flagValue) != 0;
    }

    public static void Set<T>(ref T flags, T flag) where T : struct
    {
        Int64 flagsValue = (Int64)(object)flags;
        Int64 flagValue = (Int64)(object)flag;

        flags = (T)(object)(flagsValue | flagValue);
    }

    public static void Unset<T>(ref T flags, T flag) where T : struct
    {
        Int64 flagsValue = (Int64)(object)flags;
        Int64 flagValue = (Int64)(object)flag;

        flags = (T)(object)(flagsValue & (~flagValue));
    }
}
