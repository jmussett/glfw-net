using System;
using System.Runtime.InteropServices;

namespace GLFW;

/// <summary>
///     Wrapper around a GLFW window pointer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct GlfwWindow : IEquatable<GlfwWindow>
{
    /// <summary>
    ///     Describes a default/null instance.
    /// </summary>
    public static readonly GlfwWindow None;

    /// <summary>
    ///     Internal pointer.
    /// </summary>
    private readonly nint handle;

    /// <summary>
    ///     Performs an implicit conversion from <see cref="GlfwWindow" /> to <see cref="nint" />.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <returns>
    ///     The result of the conversion.
    /// </returns>
    public static implicit operator nint(GlfwWindow window) { return window.handle; }
    
    /// <summary>
    ///     Performs an explicit conversion from <see cref="nint"/> to <see cref="GlfwWindow"/>.
    /// </summary>
    /// <param name="handle">A pointer representing the window handle.</param>
    /// <returns>The result of the conversion.</returns>
    public static explicit operator GlfwWindow(nint handle) => new(handle);

    /// <summary>
    /// Creates a new instance of the <see cref="GlfwWindow"/> struct.
    /// </summary>
    /// <param name="handle">A pointer representing the window handle.</param>
    public GlfwWindow(nint handle)
    {
        this.handle = handle;
    }

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString() { return handle.ToString(); }

    /// <summary>
    ///     Determines whether the specified <see cref="GlfwWindow" />, is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="GlfwWindow" /> to compare with this instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="GlfwWindow" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(GlfwWindow other) { return handle.Equals(other.handle); }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is GlfwWindow window)
            return Equals(window);
        return false;
    }

    /// <summary>
    ///     Gets or sets the opacity of the window in the range of <c>0.0</c> and <c>1.0</c> inclusive.
    /// </summary>
    public float Opacity
    {
        get => Glfw.GetWindowOpacity(handle);
        set => Glfw.SetWindowOpacity(handle, Math.Min(1.0f, Math.Max(0.0f, value)));
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode() { return handle.GetHashCode(); }

    /// <summary>
    ///     Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    ///     The result of the operator.
    /// </returns>
    public static bool operator ==(GlfwWindow left, GlfwWindow right) { return left.Equals(right); }

    /// <summary>
    ///     Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    ///     The result of the operator.
    /// </returns>
    public static bool operator !=(GlfwWindow left, GlfwWindow right) { return !left.Equals(right); }
}