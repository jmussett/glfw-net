using System;
using System.Runtime.InteropServices;

namespace GLFW;

/// <summary>
///     Wrapper around a handle for a window cursor object.
/// </summary>
/// <seealso cref="GlfwCursor" />
[StructLayout(LayoutKind.Sequential)]
public readonly struct GlfwCursor : IEquatable<GlfwCursor>
{
    /// <summary>
    ///     Represents a <c>null</c> value for a <see cref="GlfwCursor" /> object.
    /// </summary>
    public static readonly GlfwCursor None;

    /// <summary>
    ///     Internal pointer.
    /// </summary>
    private readonly nint cursor;

    /// <summary>
    ///     Determines whether the specified <see cref="GlfwCursor" />, is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="GlfwCursor" /> to compare with this instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="GlfwCursor" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(GlfwCursor other) { return cursor.Equals(other.cursor); }

    /// <summary>
    ///     Determines whether the specified <see cref="object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is GlfwCursor cur)
            return Equals(cur);
        return false;
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode() { return cursor.GetHashCode(); }

    /// <summary>
    ///     Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    ///     The result of the operator.
    /// </returns>
    public static bool operator ==(GlfwCursor left, GlfwCursor right) { return left.Equals(right); }

    /// <summary>
    ///     Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    ///     The result of the operator.
    /// </returns>
    public static bool operator !=(GlfwCursor left, GlfwCursor right) { return !left.Equals(right); }
}