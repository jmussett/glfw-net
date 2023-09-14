using System.Runtime.InteropServices;

namespace GLFW;

/// <summary>
///     Describes a basic image structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct GlfwImage
{
    /// <summary>
    ///     The height, in pixels, of this image.
    /// </summary>
    public readonly int Width;

    /// <summary>
    ///     The width, in pixels, of this image.
    /// </summary>
    public readonly int Height;

    /// <summary>
    ///     Pointer to the RGBA pixel data of this image, arranged left-to-right, top-to-bottom.
    /// </summary>
    public readonly nint Pixels;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GlfwImage" /> struct.
    /// </summary>
    /// <param name="width">The height, in pixels, of this image.</param>
    /// <param name="height">The width, in pixels, of this image..</param>
    /// <param name="pixels">Pointer to the RGBA pixel data of this image, arranged left-to-right, top-to-bottom.</param>
    public GlfwImage(int width, int height, nint pixels)
    {
        Width = width;
        Height = height;
        Pixels = pixels;
    }

    // TODO: Implement manual load of bmp
}