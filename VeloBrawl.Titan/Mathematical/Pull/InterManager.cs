﻿using System.Runtime.CompilerServices;

namespace VeloBrawl.Titan.Mathematical.Pull;

public static class InterManager
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateRight(uint value, int count)
    {
        return (value >> count) | (value << (32 - count));
    }
}