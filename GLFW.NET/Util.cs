﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GLFW;

internal static class Util
{
    /// <summary>
    ///     Reads memory from the pointer until the first null byte is encountered and decodes the bytes from UTF-8 into a
    ///     managed <see cref="string" />.
    /// </summary>
    /// <param name="ptr">Pointer to the start of the string.</param>
    /// <returns>Managed string created from read UTF-8 bytes.</returns>
    public static string PtrToStringUTF8(nint ptr)
    {
        if (ptr != IntPtr.Zero)
        {
            var length = 0;
            while (Marshal.ReadByte(ptr, length) != 0)
                length++;
            var buffer = new byte[length];
            Marshal.Copy(ptr, buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }

        return "";
    }
}