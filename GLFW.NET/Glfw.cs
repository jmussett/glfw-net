using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#pragma warning disable 0419

namespace GLFW;

/// <summary>
///     The base class the vast majority of the GLFW functions, excluding only Vulkan and native platform specific
///     functions.
/// </summary>
[SuppressUnmanagedCodeSecurity]
public static class Glfw
{
    /// <summary>
    ///     The native library name,
    ///     <para>For Unix users using an installed version of GLFW, this needs refactored to <c>glfw</c>.</para>
    /// </summary>
#if Windows
    public const string LIBRARY = "glfw3";
#elif OSX
    public const string LIBRARY = "libglfw.3"; // mac
#else
    public const string LIBRARY = "glfw";
#endif

    private static readonly GlfwErrorCallback errorCallback = GlfwError;

    static Glfw()
    {
        Init();
        SetErrorCallback(errorCallback);
    }

    /// <summary>
    ///     Returns and clears the error code of the last error that occurred on the calling thread, and optionally
    ///     a description of it.
    ///     <para>
    ///         If no error has occurred since the last call, it returns <see cref="ErrorCode.None" /> and the
    ///         description pointer is set to <c>null</c>.
    ///     </para>
    /// </summary>
    /// <param name="description">The description string, or <c>null</c> if there is no error.</param>
    /// <returns>The error code.</returns>
    public static ErrorCode GetError(out string? description)
    {
        var code = GetErrorPrivate(out var ptr);
        description = code == ErrorCode.None ? null : Util.PtrToStringUTF8(ptr);
        return code;
    }

    /// <summary>
    ///     Retrieves the content scale for the specified monitor. The content scale is the ratio between the
    ///     current DPI and the platform's default DPI.
    ///     <para>
    ///         This is especially important for text and any UI elements. If the pixel dimensions of your UI scaled by
    ///         this look appropriate on your machine then it should appear at a reasonable size on other machines
    ///         regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being
    ///         somewhat correct.
    ///     </para>
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <param name="xScale">The scale on the x-axis.</param>
    /// <param name="yScale">The scale on the y-axis.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetMonitorContentScale(nint monitor, out float xScale, out float yScale);

    /// <summary>
    ///     Returns the current value of the user-defined pointer of the specified <paramref name="monitor" />.
    /// </summary>
    /// <param name="monitor">The monitor whose pointer to return.</param>
    /// <returns>The user-pointer, or <see cref="IntPtr.Zero" /> if none is defined.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetMonitorUserPointer(nint monitor);

    /// <summary>
    ///     This function sets the user-defined pointer of the specified <paramref name="monitor" />.
    ///     <para>The current value is retained until the monitor is disconnected.</para>
    /// </summary>
    /// <param name="monitor">The monitor whose pointer to set.</param>
    /// <param name="pointer">The user-defined pointer value.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetMonitorUserPointer(nint monitor, nint pointer);

    /// <summary>
    ///     Returns the opacity of the window, including any decorations.
    /// </summary>
    /// <param name="window">The window to query.</param>
    /// <returns>The opacity value of the specified window, a value between <c>0.0</c> and <c>1.0</c> inclusive.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetWindowOpacity(nint window);

    /// <summary>
    ///     Sets the opacity of the window, including any decorations.
    ///     <para>
    ///         The opacity (or alpha) value is a positive finite number between zero and one, where zero is fully
    ///         transparent and one is fully opaque.
    ///     </para>
    /// </summary>
    /// <param name="window">The window to set the opacity for.</param>
    /// <param name="opacity">The desired opacity of the specified window.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowOpacity(nint window, float opacity);

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values until
    ///     changed by a call to this function or <see cref="DefaultWindowHints" />, or until the library is terminated.
    ///     <para>
    ///         Some hints are platform specific. These may be set on any platform but they will only affect their
    ///         specific platform. Other platforms will ignore them. Setting these hints requires no platform specific
    ///         headers or functions.
    ///     </para>
    /// </summary>
    /// <param name="hint">The window hit to set.</param>
    /// <param name="value">The new value of the window hint.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwWindowHintString", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WindowHintString(GlfwHint hint, byte[] value);

    /// <summary>
    ///     Helper function to call <see cref="WindowHintString(GlfwHint, byte[])" /> with UTF-8 encoding.
    /// </summary>
    /// <param name="hint">The window hit to set.</param>
    /// <param name="value">The new value of the window hint.</param>
    // ReSharper disable once InconsistentNaming
    public static void WindowHintStringUTF8(GlfwHint hint, string value)
    {
        WindowHintString(hint, Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    ///     Helper function to call <see cref="WindowHintString(GlfwHint, byte[])" /> with ASCII encoding.
    /// </summary>
    /// <param name="hint">The window hit to set.</param>
    /// <param name="value">The new value of the window hint.</param>
    // ReSharper disable once InconsistentNaming
    public static void WindowHintStringASCII(GlfwHint hint, string value)
    {
        WindowHintString(hint, Encoding.ASCII.GetBytes(value));
    }

    /// <summary>
    ///     Retrieves the content scale for the specified window. The content scale is the ratio between the current DPI and
    ///     the platform's default DPI. This is especially important for text and any UI elements. If the pixel dimensions of
    ///     your UI scaled by this look appropriate on your machine then it should appear at a reasonable size on other
    ///     machines regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being
    ///     somewhat correct.
    ///     <para>
    ///         On systems where each monitors can have its own content scale, the window content scale will depend on which
    ///         monitor the system considers the window to be on.
    ///     </para>
    /// </summary>
    /// <param name="window">The window to query.</param>
    /// <param name="xScale">The content scale on the x-axis.</param>
    /// <param name="yScale">The content scale on the y-axis.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowContentScale", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetWindowContentScale(nint window, out float xScale, out float yScale);

    /// <summary>
    ///     Requests user attention to the specified <paramref name="window" />. On platforms where this is not supported,
    ///     attention is
    ///     requested to the application as a whole.
    ///     <para>
    ///         Once the user has given attention, usually by focusing the window or application, the system will end the
    ///         request automatically.
    ///     </para>
    /// </summary>
    /// <param name="window">The window to request user attention to.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwRequestWindowAttention", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RequestWindowAttention(nint window);

    /// <summary>
    ///     This function returns whether raw mouse motion is supported on the current system.
    ///     <para>
    ///         This status does not change after GLFW has been initialized so you only need to check this once. If you
    ///         attempt to enable raw motion on a system that does not support it, an error will be emitted.
    ///     </para>
    /// </summary>
    /// <returns><c>true</c> if raw mouse motion is supported on the current machine, or <c>false</c> otherwise.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RawMouseMotionSupported();

    /// <summary>
    ///     Sets the maximization callback of the specified <paramref name="window," /> which is called when the window is
    ///     maximized or restored.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="cb">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = CallingConvention.Cdecl)]
    public static extern GlfwWindowMaximizedCallback SetWindowMaximizeCallback(nint window,
        GlfwWindowMaximizedCallback cb);

    /// <summary>
    ///     Sets the window content scale callback of the specified window, which is called when the content scale of the
    ///     specified window changes.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="cb">The new callback, or <c>null</c> to remove the currently set callback</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowContentScaleCallback",
        CallingConvention = CallingConvention.Cdecl)]
    public static extern GlfwWindowContentsScaleCallback SetWindowContentScaleCallback(nint window,
        GlfwWindowContentsScaleCallback cb);

    /// <summary>
    ///     Returns the platform-specific scan-code of the specified key.
    ///     <para>If the key is <see cref="Keys.Unknown" /> or does not exist on the keyboard this method will return -1.</para>
    /// </summary>
    /// <param name="key">The named key to query.</param>
    /// <returns>The platform-specific scan-code for the key, or -1 if an error occurred.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetKeyScancode", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetKeyScanCode(Keys key);

    /// <summary>
    ///     Sets the value of an attribute of the specified window.
    /// </summary>
    /// <param name="window">
    ///     The window to set the attribute for
    ///     <para>Valid attributes include:</para>
    ///     <para>
    ///         <see cref="WindowAttribute.Decorated" />
    ///     </para>
    ///     <para>
    ///         <see cref="WindowAttribute.Resizable" />
    ///     </para>
    ///     <para>
    ///         <see cref="WindowAttribute.Floating" />
    ///     </para>
    ///     <para>
    ///         <see cref="WindowAttribute.AutoIconify" />
    ///     </para>
    ///     <para>
    ///         <see cref="WindowAttribute.Focused" />
    ///     </para>
    /// </param>
    /// <param name="attr">A supported window attribute.</param>
    /// <param name="value">The value to set.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowAttribute(nint window, WindowAttribute attr, bool value);

    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickHats", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetJoystickHats(int joystickId, out int count);

    /// <summary>
    ///     Returns the state of all hats of the specified joystick as a bitmask.
    /// </summary>
    /// <param name="joystickId">The joystick to query.</param>
    /// <returns>A bitmask enumeration containing the state of the joystick hats.</returns>
    public static Hat GetJoystickHats(int joystickId)
    {
        var hat = Hat.Centered;
        var ptr = GetJoystickHats(joystickId, out var count);
        for (var i = 0; i < count; i++)
        {
            var value = Marshal.ReadByte(ptr, i);
            hat |= (Hat) value;
        }

        return hat;
    }

    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickGUID", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetJoystickGuidPrivate(int joystickId);

    /// <summary>
    ///     Returns the SDL compatible GUID, as a hexadecimal string, of the specified joystick.
    ///     <para>
    ///         The GUID is what connects a joystick to a gamepad mapping. A connected joystick will always have a GUID even
    ///         if there is no gamepad mapping assigned to it.
    ///     </para>
    /// </summary>
    /// <param name="joystickId">The joystick to query.</param>
    /// <returns>The GUID of the joystick, or <c>null</c> if the joystick is not present or an error occurred.</returns>
    public static string? GetJoystickGuid(int joystickId)
    {
        var ptr = GetJoystickGuidPrivate(joystickId);
        return ptr == IntPtr.Zero ? null : Util.PtrToStringUTF8(ptr);
    }

    /// <summary>
    ///     This function returns the current value of the user-defined pointer of the specified joystick.
    /// </summary>
    /// <param name="joystickId">The joystick whose pointer to return.</param>
    /// <returns>The user-defined pointer, or <see cref="IntPtr.Zero" /> if never defined.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetJoystickUserPointer(int joystickId);

    /// <summary>
    ///     This function sets the user-defined pointer of the specified joystick.
    ///     <para>The current value is retained until the joystick is disconnected.</para>
    /// </summary>
    /// <param name="joystickId">The joystick whose pointer to set.</param>
    /// <param name="pointer">The new value.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetJoystickUserPointer(int joystickId, nint pointer);

    /// <summary>
    ///     Returns whether the specified joystick is both present and has a gamepad mapping.
    /// </summary>
    /// <param name="joystickId">The joystick to query.</param>
    /// <returns><c>true</c> if a joystick is both present and has a gamepad mapping, or <c>false</c> otherwise.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool JoystickIsGamepad(int joystickId);

    [DllImport(LIBRARY, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool UpdateGamepadMappings([NotNull] byte[] mappings);

    /// <summary>
    ///     Parses the specified string and updates the internal list with any gamepad mappings it finds.
    ///     <para>
    ///         This string may contain either a single gamepad mapping or many mappings separated by newlines. The parser
    ///         supports the full format of the SDL <c>gamecontrollerdb.txt</c> source file including empty lines and comments.
    ///     </para>
    /// </summary>
    /// <param name="mappings">The string containing the gamepad mappings.</param>
    /// <returns><c>true</c> if successful, or <c>false</c> if an error occurred.</returns>
    public static bool UpdateGamepadMappings(string mappings)
    {
        return UpdateGamepadMappings(Encoding.ASCII.GetBytes(mappings));
    }

    [DllImport(LIBRARY, EntryPoint = "glfwGetGamepadName", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetGamepadNamePrivate(int gamepadId);

    /// <summary>
    ///     Returns the human-readable name of the gamepad from the gamepad mapping assigned to the specified joystick.
    /// </summary>
    /// <param name="gamepadId">The joystick to query.</param>
    /// <returns>
    ///     The name of the gamepad, or <c>null</c> if the joystick is not present, does not have a mapping or an error
    ///     occurred.
    /// </returns>
    public static string? GetGamepadName(int gamepadId)
    {
        var ptr = GetGamepadNamePrivate(gamepadId);
        return ptr == IntPtr.Zero ? null : Util.PtrToStringUTF8(ptr);
    }

    /// <summary>
    ///     Retrieves the state of the specified joystick remapped to an Xbox-like gamepad.
    /// </summary>
    /// <param name="id">The joystick to query.</param>
    /// <param name="state">The gamepad input state of the joystick.</param>
    /// <returns>
    ///     <c>true</c> if successful, or <c>false</c> if no joystick is connected, it has no gamepad mapping or an error
    ///     occurred.
    /// </returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetGamepadState", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool GetGamepadState(int id, out GlfwGamePadState state);

    /// <summary>
    ///     Gets the window whose OpenGL or OpenGL ES context is current on the calling thread, or <see cref="GlfwWindow.None" />
    ///     if no context is current.
    /// </summary>
    /// <value>
    ///     The current context.
    /// </value>
    public static GlfwWindow CurrentContext => GetCurrentContext();

    /// <summary>
    ///     Gets an array of handles for all currently connected monitors.
    ///     <para>The primary monitor is always first in the array.</para>
    /// </summary>
    /// <value>
    ///     The monitors.
    /// </value>
    public static GlfwMonitor[] Monitors
    {
        get
        {
            var ptr = GetMonitors(out var count);
            var monitors = new GlfwMonitor[count];
            var offset = 0;
            for (var i = 0; i < count; i++, offset += IntPtr.Size)
            {
                monitors[i] = Marshal.PtrToStructure<GlfwMonitor>(ptr + offset);
            }

            return monitors;
        }
    }

    /// <summary>
    ///     Gets the primary monitor. This is usually the monitor where elements like the task bar or global menu bar are
    ///     located.
    /// </summary>
    /// <value>
    ///     The primary monitor, or <see cref="GlfwMonitor.None" /> if no monitors were found or if an error occurred.
    /// </value>
    public static GlfwMonitor PrimaryMonitor => GetPrimaryMonitor();

    /// <summary>
    ///     Gets or sets the value of the GLFW timer.
    ///     <para>
    ///         The resolution of the timer is system dependent, but is usually on the order of a few micro- or nanoseconds.
    ///         It uses the highest-resolution monotonic time source on each supported platform.
    ///     </para>
    /// </summary>
    /// <value>
    ///     The time.
    /// </value>
    public static double Time
    {
        get => GetTime();
        set => SetTime(value);
    }

    /// <summary>
    ///     Gets the frequency, in Hz, of the raw timer.
    /// </summary>
    /// <value>
    ///     The frequency of the timer, in Hz, or zero if an error occurred.
    /// </value>
    public static ulong TimerFrequency => GetTimerFrequency();

    /// <summary>
    ///     Gets the current value of the raw timer, measured in 1 / frequency seconds.
    /// </summary>
    /// <value>
    ///     The timer value.
    /// </value>
    public static ulong TimerValue => GetTimerValue();

    /// <summary>
    ///     Gets the version of the native GLFW library.
    /// </summary>
    /// <value>
    ///     The version.
    /// </value>
    public static Version Version
    {
        get
        {
            GetVersion(out var major, out var minor, out var revision);
            return new Version(major, minor, revision);
        }
    }

    /// <summary>
    ///     Gets the compile-time generated version string of the GLFW library binary.
    ///     <para>It describes the version, platform, compiler and any platform-specific compile-time options.</para>
    /// </summary>
    /// <value>
    ///     The version string.
    /// </value>
    public static string VersionString => Util.PtrToStringUTF8(GetVersionString());

    /// <summary>
    ///     This function sets hints for the next initialization of GLFW.
    ///     <para>
    ///         The values you set hints to are never reset by GLFW, but they only take effect during initialization.
    ///         Once GLFW has been initialized, any values you set will be ignored until the library is terminated and
    ///         initialized again.>.
    ///     </para>
    /// </summary>
    /// <param name="hint">
    ///     The hint, valid values are <see cref="GlfwHint.JoystickHatButtons" />,
    ///     <see cref="GlfwHint.CocoaMenuBar" />, and <see cref="GlfwHint.CocoaChDirResources" />.
    /// </param>
    /// <param name="value">The value of the hint.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwInitHint", CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitHint(GlfwHint hint, bool value);

    /// <summary>
    ///     This function initializes the GLFW library. Before most GLFW functions can be used, GLFW must be initialized, and
    ///     before an application terminates GLFW should be terminated in order to free any resources allocated during or after
    ///     initialization.
    ///     <para>
    ///         If this function fails, it calls <see cref="Terminate" /> before returning. If it succeeds, you should call
    ///         <see cref="Terminate" /> before the application exits
    ///     </para>
    ///     <para>
    ///         Additional calls to this function after successful initialization but before termination will return
    ///         <c>true</c> immediately.
    ///     </para>
    /// </summary>
    /// <returns><c>true</c> if successful, or <c>false</c> if an error occurred.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool Init();

    /// <summary>
    ///     This function destroys all remaining windows and cursors, restores any modified gamma ramps and frees any other
    ///     allocated resources. Once this function is called, you must again call <see cref="Init" /> successfully before you
    ///     will be able to use most GLFW functions.
    ///     If GLFW has been successfully initialized, this function should be called before the application exits. If
    ///     initialization fails, there is no need to call this function, as it is called by <see cref="Init" /> before it
    ///     returns failure.
    /// </summary>
    /// <note type="warning">
    ///     The contexts of any remaining windows must not be current on any other thread when this function
    ///     is called.
    /// </note>
    [DllImport(LIBRARY, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Terminate();

    /// <summary>
    ///     Sets the error callback, which is called with an error code and a human-readable description each
    ///     time a GLFW error occurs.
    /// </summary>
    /// <param name="errorHandler">The callback function, or <c>null</c> to unbind this callback.</param>
    /// <returns>The previously set callback function, or <c>null</c> if no callback was already set.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetErrorCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwErrorCallback))]
    public static extern GlfwErrorCallback SetErrorCallback(GlfwErrorCallback errorHandler);

    [DllImport(LIBRARY, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
    private static extern GlfwWindow CreateWindow(int width, int height, byte[] title, GlfwMonitor monitor, GlfwWindow share);

    /// <summary>
    ///     This function destroys the specified window and its context. On calling this function, no further callbacks will be
    ///     called for that window.
    ///     <para>If the context of the specified window is current on the main thread, it is detached before being destroyed.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void DestroyWindow(GlfwWindow window);

    /// <summary>
    ///     This function makes the specified window visible if it was previously hidden. If the window is already visible or
    ///     is in full screen mode, this function does nothing.
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwShowWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ShowWindow(GlfwWindow window);

    /// <summary>
    ///     This function hides the specified window if it was previously visible. If the window is already hidden or is in
    ///     full screen mode, this function does nothing.
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwHideWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void HideWindow(GlfwWindow window);

    /// <summary>
    ///     This function retrieves the position, in screen coordinates, of the upper-left corner of the client area of the
    ///     specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowPos", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetWindowPosition(GlfwWindow window, out int x, out int y);

    /// <summary>
    ///     Sets the position, in screen coordinates, of the upper-left corner of the client area of the
    ///     specified windowed mode window.
    ///     <para>If the window is a full screen window, this function does nothing.</para>
    /// </summary>
    /// <note type="important">
    ///     Do not use this function to move an already visible window unless you have very good reasons for
    ///     doing so, as it will confuse and annoy the user.
    /// </note>
    /// <param name="window">A window instance.</param>
    /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
    /// <remarks>
    ///     The window manager may put limits on what positions are allowed. GLFW cannot and should not override these
    ///     limits.
    /// </remarks>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowPos", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowPosition(GlfwWindow window, int x, int y);

    /// <summary>
    ///     This function retrieves the size, in screen coordinates, of the client area of the specified window.
    ///     <para>
    ///         If you wish to retrieve the size of the framebuffer of the window in pixels, use
    ///         <see cref="GetFramebufferSize" />.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="width">The width, in screen coordinates.</param>
    /// <param name="height">The height, in screen coordinates.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetWindowSize(GlfwWindow window, out int width, out int height);

    /// <summary>
    ///     Sets the size, in screen coordinates, of the client area of the specified window.
    ///     <para>
    ///         For full screen windows, this function updates the resolution of its desired video mode and switches to the
    ///         video mode closest to it, without affecting the window's context. As the context is unaffected, the bit depths
    ///         of the framebuffer remain unchanged.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="width">The desired width, in screen coordinates, of the window client area.</param>
    /// <param name="height">The desired height, in screen coordinates, of the window client area.</param>
    /// <remarks>The window manager may put limits on what sizes are allowed. GLFW cannot and should not override these limits.</remarks>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowSize(GlfwWindow window, int width, int height);

    /// <summary>
    ///     This function retrieves the size, in pixels, of the framebuffer of the specified window.
    ///     <para>If you wish to retrieve the size of the window in screen coordinates, use <see cref="GetWindowSize" />.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="width">The width, in pixels, of the framebuffer.</param>
    /// <param name="height">The height, in pixels, of the framebuffer.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetFramebufferSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetFramebufferSize(GlfwWindow window, out int width, out int height);

    /// <summary>
    ///     Sets the position callback of the specified window, which is called when the window is moved.
    ///     <para>The callback is provided with the screen position of the upper-left corner of the client area of the window.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="positionCallback">The position callback to be invoked on position changes.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwPositionCallback))]
    public static extern GlfwPositionCallback SetWindowPositionCallback(GlfwWindow window,
        GlfwPositionCallback positionCallback);

    /// <summary>
    ///     Sets the size callback of the specified window, which is called when the window is resized.
    ///     <para>The callback is provided with the size, in screen coordinates, of the client area of the window.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="sizeCallback">The size callback to be invoked on size changes.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwSizeCallback))]
    public static extern GlfwSizeCallback SetWindowSizeCallback(GlfwWindow window, GlfwSizeCallback sizeCallback);

    /// <summary>
    ///     Sets the window title, encoded as UTF-8, of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="title">The title as an array of UTF-8 encoded bytes.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetWindowTitle(GlfwWindow window, byte[] title);

    /// <summary>
    ///     This function brings the specified window to front and sets input focus. The window should already be visible and
    ///     not iconified.
    /// </summary>
    /// <param name="window">The window.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwFocusWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FocusWindow(GlfwWindow window);

    /// <summary>
    ///     Sets the focus callback of the specified window, which is called when the window gains or loses input
    ///     focus.
    ///     <para>
    ///         After the focus callback is called for a window that lost input focus, synthetic key and mouse button release
    ///         events will be generated for all such that had been pressed.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="focusCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwFocusCallback))]
    public static extern GlfwFocusCallback SetWindowFocusCallback(GlfwWindow window, GlfwFocusCallback focusCallback);

    /// <summary>
    ///     This function retrieves the major, minor and revision numbers of the GLFW library.
    ///     <para>
    ///         It is intended for when you are using GLFW as a shared library and want to ensure that you are using the
    ///         minimum required version.
    ///     </para>
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="revision">The revision.</param>
    /// <seealso cref="Version" />
    [DllImport(LIBRARY, EntryPoint = "glfwGetVersion", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetVersion(out int major, out int minor, out int revision);

    /// <summary>
    ///     Gets the compile-time generated version string of the GLFW library binary.
    ///     <para>It describes the version, platform, compiler and any platform-specific compile-time options.</para>
    /// </summary>
    /// <returns>A pointer to the null-terminated UTF-8 encoded version string.</returns>
    /// <seealso cref="VersionString" />
    [DllImport(LIBRARY, EntryPoint = "glfwGetVersionString", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetVersionString();

    [DllImport(LIBRARY, EntryPoint = "glfwGetTime", CallingConvention = CallingConvention.Cdecl)]
    private static extern double GetTime();

    [DllImport(LIBRARY, EntryPoint = "glfwSetTime", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetTime(double time);

    [DllImport(LIBRARY, EntryPoint = "glfwGetTimerFrequency", CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong GetTimerFrequency();

    [DllImport(LIBRARY, EntryPoint = "glfwGetTimerValue", CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong GetTimerValue();

    /// <summary>
    ///     This function retrieves the size, in screen coordinates, of each edge of the frame of the specified window.
    ///     <para>
    ///         This size includes the title bar, if the window has one. The size of the frame may vary depending on the
    ///         window-related hints used to create it.
    ///     </para>
    ///     <para>
    ///         Because this function retrieves the size of each window frame edge and not the offset along a particular
    ///         coordinate axis, the retrieved values will always be zero or positive.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="left">The size, in screen coordinates, of the left edge of the window frame</param>
    /// <param name="top">The size, in screen coordinates, of the top edge of the window frame</param>
    /// <param name="right">The size, in screen coordinates, of the right edge of the window frame.</param>
    /// <param name="bottom">The size, in screen coordinates, of the bottom edge of the window frame</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetWindowFrameSize(GlfwWindow window, out int left, out int top, out int right,
        out int bottom);

    /// <summary>
    ///     This function maximizes the specified window if it was previously not maximized. If the window is already
    ///     maximized, this function does nothing.
    ///     <para>If the specified window is a full screen window, this function does nothing.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwMaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MaximizeWindow(GlfwWindow window);

    /// <summary>
    ///     This function iconifies (minimizes) the specified window if it was previously restored.
    ///     <para>If the window is already iconified, this function does nothing.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwIconifyWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void IconifyWindow(GlfwWindow window);

    /// <summary>
    ///     This function restores the specified window if it was previously iconified (minimized) or maximized.
    ///     <para>If the window is already restored, this function does nothing.</para>
    ///     <para>
    ///         If the specified window is a full screen window, the resolution chosen for the window is restored on the
    ///         selected monitor.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwRestoreWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RestoreWindow(GlfwWindow window);

    /// <summary>
    ///     This function makes the OpenGL or OpenGL ES context of the specified window current on the calling thread.
    ///     <para>
    ///         A context can only be made current on a single thread at a time and each thread can have only a single
    ///         current context at a time.
    ///     </para>
    ///     <para>By default, making a context non-current implicitly forces a pipeline flush.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MakeContextCurrent(GlfwWindow window);

    /// <summary>
    ///     This function swaps the front and back buffers of the specified window when rendering with OpenGL or OpenGL ES.
    ///     <para>
    ///         If the swap interval is greater than zero, the GPU driver waits the specified number of screen updates before
    ///         swapping the buffers.
    ///     </para>
    ///     <para>This function does not apply to Vulkan.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SwapBuffers(GlfwWindow window);

    /// <summary>
    ///     Sets the swap interval for the current OpenGL or OpenGL ES context, i.e. the number of screen updates
    ///     to wait from the time <see cref="SwapBuffers" /> was called before swapping the buffers and returning.
    ///     <para>This is sometimes called vertical synchronization, vertical retrace synchronization or just vsync.</para>
    ///     <para>
    ///         A context must be current on the calling thread. Calling this function without a current context will cause
    ///         an exception.
    ///     </para>
    ///     <para>
    ///         This function does not apply to Vulkan. If you are rendering with Vulkan, see the present mode of your
    ///         swapchain instead.
    ///     </para>
    /// </summary>
    /// <param name="interval">
    ///     The minimum number of screen updates to wait for until the buffers are swapped by
    ///     <see cref="SwapBuffers" />.
    /// </param>
    [DllImport(LIBRARY, EntryPoint = "glfwSwapInterval", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SwapInterval(int interval);

    /// <summary>
    ///     Gets whether the specified API extension is supported by the current OpenGL or OpenGL ES context.
    ///     <para>It searches both for client API extension and context creation API extensions.</para>
    /// </summary>
    /// <param name="extension">The extension name as an array of ASCII encoded bytes.</param>
    /// <returns><c>true</c> if the extension is supported; otherwise <c>false</c>.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool GetExtensionSupported(byte[] extension);

    /// <summary>
    ///     This function resets all window hints to their default values.
    /// </summary>
    [DllImport(LIBRARY, EntryPoint = "glfwDefaultWindowHints", CallingConvention = CallingConvention.Cdecl)]
    public static extern void DefaultWindowHints();

    /// <summary>
    ///     Gets the value of the close flag of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns><c>true</c> if close flag is present; otherwise <c>false</c>.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool WindowShouldClose(GlfwWindow window);

    /// <summary>
    ///     Sets the value of the close flag of the specified window.
    ///     <para>This can be used to override the user's attempt to close the window, or to signal that it should be closed.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="close"><c>true</c> to set close flag, or <c>false</c> to cancel flag.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowShouldClose(GlfwWindow window, bool close);

    /// <summary>
    ///     Sets the icon of the specified window. If passed an array of candidate images, those of or closest to
    ///     the sizes desired by the system are selected. If no images are specified, the window reverts to its default icon.
    ///     <para>
    ///         The desired image sizes varies depending on platform and system settings. The selected images will be
    ///         rescaled as needed. Good sizes include 16x16, 32x32 and 48x48.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="count">The number of images in <paramref name="images" />.</param>
    /// <param name="images">An array of icon images.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowIcon(GlfwWindow window, int count, GlfwImage[] images);

    /// <summary>
    ///     This function puts the calling thread to sleep until at least one event is available in the event queue. Once one
    ///     or more events are available, it behaves exactly like glfwPollEvents, i.e. the events in the queue are processed
    ///     and the function then returns immediately. Processing events will cause the window and input callbacks associated
    ///     with those events to be called.
    ///     <para>
    ///         Since not all events are associated with callbacks, this function may return without a callback having been
    ///         called even if you are monitoring all callbacks.
    ///     </para>
    ///     <para>
    ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
    ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
    ///         contents of your window when necessary during such operations.
    ///     </para>
    /// </summary>
    [DllImport(LIBRARY, EntryPoint = "glfwWaitEvents", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WaitEvents();

    /// <summary>
    ///     This function processes only those events that are already in the event queue and then returns immediately.
    ///     Processing events will cause the window and input callbacks associated with those events to be called.
    ///     <para>
    ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
    ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
    ///         contents of your window when necessary during such operations.
    ///     </para>
    ///     <para>
    ///         On some platforms, certain events are sent directly to the application without going through the event queue,
    ///         causing callbacks to be called outside of a call to one of the event processing functions.
    ///     </para>
    /// </summary>
    [DllImport(LIBRARY, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
    public static extern void PollEvents();

    /// <summary>
    ///     This function posts an empty event from the current thread to the event queue, causing <see cref="WaitEvents" /> or
    ///     <see cref="WaitEventsTimeout " /> to return.
    /// </summary>
    [DllImport(LIBRARY, EntryPoint = "glfwPostEmptyEvent", CallingConvention = CallingConvention.Cdecl)]
    public static extern void PostEmptyEvent();

    /// <summary>
    ///     This function puts the calling thread to sleep until at least one event is available in the event queue, or until
    ///     the specified timeout is reached. If one or more events are available, it behaves exactly like
    ///     <see cref="PollEvents" />, i.e. the events in the queue are processed and the function then returns immediately.
    ///     Processing events will cause the window and input callbacks associated with those events to be called.
    ///     <para>The timeout value must be a positive finite number.</para>
    ///     <para>
    ///         Since not all events are associated with callbacks, this function may return without a callback having been
    ///         called even if you are monitoring all callbacks.
    ///     </para>
    ///     <para>
    ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
    ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
    ///         contents of your window when necessary during such operations.
    ///     </para>
    /// </summary>
    /// <param name="timeout">The maximum amount of time, in seconds, to wait.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WaitEventsTimeout(double timeout);

    /// <summary>
    ///     Sets the close callback of the specified window, which is called when the user attempts to close the
    ///     window, for example by clicking the close widget in the title bar.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="closeCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwWindowCallback))]
    public static extern GlfwWindowCallback SetCloseCallback(GlfwWindow window, GlfwWindowCallback closeCallback);

    [DllImport(LIBRARY, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = CallingConvention.Cdecl)]
    private static extern GlfwMonitor GetPrimaryMonitor();

    [DllImport(LIBRARY, EntryPoint = "glfwGetVideoMode", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetVideoModeInternal(GlfwMonitor monitor);

    [DllImport(LIBRARY, EntryPoint = "glfwGetVideoModes", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetVideoModes(GlfwMonitor monitor, out int count);

    /// <summary>
    ///     Gets the handle of the monitor that the specified window is in full screen on.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The monitor, or <see cref="GlfwMonitor.None" /> if the window is in windowed mode or an error occurred.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
    public static extern GlfwMonitor GetWindowMonitor(GlfwWindow window);

    /// <summary>
    ///     Sets the monitor that the window uses for full screen mode or, if the monitor is
    ///     <see cref="GlfwMonitor.None" />, makes it windowed mode.
    ///     <para>
    ///         When setting a monitor, this function updates the width, height and refresh rate of the desired video mode
    ///         and switches to the video mode closest to it. The window position is ignored when setting a monitor.
    ///     </para>
    ///     <para>
    ///         When the monitor is <see cref="GlfwMonitor.None" />, the position, width and height are used to place the window
    ///         client area. The refresh rate is ignored when no monitor is specified.
    ///     </para>
    ///     <para>
    ///         If you only wish to update the resolution of a full screen window or the size of a windowed mode window, use
    ///         <see cref="SetWindowSize" />.
    ///     </para>
    ///     <para>
    ///         When a window transitions from full screen to windowed mode, this function restores any previous window
    ///         settings such as whether it is decorated, floating, resizable, has size or aspect ratio limits, etc..
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="monitor">The desired monitor, or <see cref="GlfwMonitor.None" /> to set windowed mode.</param>
    /// <param name="x">The desired x-coordinate of the upper-left corner of the client area.</param>
    /// <param name="y">The desired y-coordinate of the upper-left corner of the client area.</param>
    /// <param name="width">The desired width, in screen coordinates, of the client area or video mode.</param>
    /// <param name="height">The desired height, in screen coordinates, of the client area or video mode.</param>
    /// <param name="refreshRate">The desired refresh rate, in Hz, of the video mode, or <see cref="Constants.Default" />.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowMonitor(GlfwWindow window, GlfwMonitor monitor, int x, int y, int width, int height,
        int refreshRate);

    [DllImport(LIBRARY, EntryPoint = "glfwGetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
    internal static extern nint GetGammaRampInternal(GlfwMonitor monitor);

    /// <summary>
    ///     Sets the current gamma ramp for the specified monitor.
    ///     <para>
    ///         The original gamma ramp for that monitor is saved by GLFW the first time this function is called and is
    ///         restored by <see cref="Terminate" />.
    ///     </para>
    ///     <para>WARNING: Gamma ramps with sizes other than 256 are not supported on some platforms (Windows).</para>
    /// </summary>
    /// <param name="monitor">The monitor whose gamma ramp to set.</param>
    /// <param name="gammaRamp">The gamma ramp to use.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetGammaRamp(GlfwMonitor monitor, GlfwGammaRamp gammaRamp);

    /// <summary>
    ///     This function generates a 256-element gamma ramp from the specified exponent and then calls
    ///     <see cref="SetGammaRamp" /> with it.
    ///     <para>The value must be a finite number greater than zero.</para>
    /// </summary>
    /// <param name="monitor">The monitor whose gamma ramp to set.</param>
    /// <param name="gamma">The desired exponent.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetGamma", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetGamma(GlfwMonitor monitor, float gamma);

    [DllImport(LIBRARY, EntryPoint = "glfwGetClipboardString", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetClipboardStringInternal(GlfwWindow window);

    [DllImport(LIBRARY, EntryPoint = "glfwSetClipboardString", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetClipboardString(GlfwWindow window, byte[] bytes);

    /// <summary>
    ///     Sets the file drop callback of the specified window, which is called when one or more dragged files
    ///     are dropped on the window.
    ///     <para>
    ///         Because the path array and its strings may have been generated specifically for that event, they are not
    ///         guaranteed to be valid after the callback has returned. If you wish to use them after the callback returns, you
    ///         need to make a deep copy.
    ///     </para>
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="dropCallback">The new file drop callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetDropCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwFileDropCallback))]
    public static extern GlfwFileDropCallback SetDropCallback(GlfwWindow window, GlfwFileDropCallback dropCallback);

    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorName", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetMonitorNameInternal(GlfwMonitor monitor);

    /// <summary>
    ///     Creates a new custom cursor image that can be set for a window with glfwSetCursor.
    ///     <para>
    ///         The cursor can be destroyed with <see cref="DestroyCursor" />. Any remaining cursors are destroyed by
    ///         <see cref="Terminate" />.
    ///     </para>
    ///     <para>
    ///         The pixels are 32-bit, little-endian, non-premultiplied RGBA, i.e. eight bits per channel. They are arranged
    ///         canonically as packed sequential rows, starting from the top-left corner.
    ///     </para>
    ///     <para>
    ///         The cursor hotspot is specified in pixels, relative to the upper-left corner of the cursor image. Like all
    ///         other coordinate systems in GLFW, the X-axis points to the right and the Y-axis points down.
    ///     </para>
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="xHotspot">The x hotspot.</param>
    /// <param name="yHotspot">The y hotspot.</param>
    /// <returns>The created cursor.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwCreateCursor", CallingConvention = CallingConvention.Cdecl)]
    public static extern GlfwCursor CreateCursor(GlfwImage image, int xHotspot, int yHotspot);

    /// <summary>
    ///     This function destroys a cursor previously created with <see cref="CreateCursor" />. Any remaining cursors will be
    ///     destroyed by <see cref="Terminate" />.
    /// </summary>
    /// <param name="cursor">The cursor object to destroy.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwDestroyCursor", CallingConvention = CallingConvention.Cdecl)]
    public static extern void DestroyCursor(GlfwCursor cursor);

    /// <summary>
    ///     Sets the cursor image to be used when the cursor is over the client area of the specified window.
    ///     <para>The set cursor will only be visible when the cursor mode of the window is <see cref="CursorMode.Normal" />.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="cursor">The cursor to set, or <see cref="GlfwCursor.None" /> to switch back to the default arrow cursor.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCursor", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetCursor(GlfwWindow window, GlfwCursor cursor);

    /// <summary>
    ///     Returns a cursor with a standard shape, that can be set for a window with <see cref="SetCursor" />.
    /// </summary>
    /// <param name="type">The type of cursor to create.</param>
    /// <returns>A new cursor ready to use or <see cref="GlfwCursor.None" /> if an error occurred.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwCreateStandardCursor", CallingConvention = CallingConvention.Cdecl)]
    public static extern GlfwCursor CreateStandardCursor(CursorType type);

    /// <summary>
    ///     Gets the position of the cursor, in screen coordinates, relative to the upper-left corner of the
    ///     client area of the specified window
    ///     <para>
    ///         If the cursor is disabled then the cursor position is unbounded and limited only by the minimum and maximum
    ///         values of a double.
    ///     </para>
    ///     <para>
    ///         The coordinate can be converted to their integer equivalents with the floor function. Casting directly to an
    ///         integer type works for positive coordinates, but fails for negative ones.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="x">The cursor x-coordinate, relative to the left edge of the client area.</param>
    /// <param name="y">The cursor y-coordinate, relative to the left edge of the client area.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetCursorPos", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetCursorPosition(GlfwWindow window, out double x, out double y);

    /// <summary>
    ///     Sets the position, in screen coordinates, of the cursor relative to the upper-left corner of the
    ///     client area of the specified window. The window must have input focus. If the window does not have input focus when
    ///     this function is called, it fails silently.
    ///     <para>
    ///         If the cursor mode is disabled then the cursor position is unconstrained and limited only by the minimum and
    ///         maximum values of a <see cref="double" />.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="x">The desired x-coordinate, relative to the left edge of the client area.</param>
    /// <param name="y">The desired y-coordinate, relative to the left edge of the client area.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCursorPos", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetCursorPosition(GlfwWindow window, double x, double y);

    /// <summary>
    ///     Sets the cursor position callback of the specified window, which is called when the cursor is moved.
    ///     <para>
    ///         The callback is provided with the position, in screen coordinates, relative to the upper-left corner of the
    ///         client area of the window.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or<c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCursorPosCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwMouseCallback))]
    public static extern GlfwMouseCallback SetCursorPositionCallback(GlfwWindow window, GlfwMouseCallback mouseCallback);

    /// <summary>
    ///     Sets the cursor boundary crossing callback of the specified window, which is called when the cursor
    ///     enters or leaves the client area of the window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCursorEnterCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwMouseEnterCallback))]
    public static extern GlfwMouseEnterCallback SetCursorEnterCallback(GlfwWindow window, GlfwMouseEnterCallback mouseCallback);

    /// <summary>
    ///     Sets the mouse button callback of the specified window, which is called when a mouse button is
    ///     pressed or released.
    ///     <para>
    ///         When a window loses input focus, it will generate synthetic mouse button release events for all pressed mouse
    ///         buttons. You can tell these events from user-generated events by the fact that the synthetic ones are generated
    ///         after the focus loss event has been processed, i.e. after the window focus callback has been called.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwMouseButtonCallback))]
    public static extern GlfwMouseButtonCallback SetMouseButtonCallback(GlfwWindow window,
        GlfwMouseButtonCallback mouseCallback);

    /// <summary>
    ///     Sets the scroll callback of the specified window, which is called when a scrolling device is used,
    ///     such as a mouse wheel or scrolling area of a touchpad.
    ///     <para>The scroll callback receives all scrolling input, like that from a mouse wheel or a touchpad scrolling area.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mouseCallback">	The new scroll callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetScrollCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwMouseCallback))]
    public static extern GlfwMouseCallback SetScrollCallback(GlfwWindow window, GlfwMouseCallback mouseCallback);

    /// <summary>
    ///     Gets the last state reported for the specified mouse button to the specified window.
    ///     <para>
    ///         If the <see cref="InputMode.StickyMouseButton" /> input mode is enabled, this function returns
    ///         <see cref="InputState.Press" /> the first time you call it for a mouse button that was pressed, even if that
    ///         mouse button has already been released.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="button">The desired mouse button.</param>
    /// <returns>The input state of the <paramref name="button" />.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMouseButton", CallingConvention = CallingConvention.Cdecl)]
    public static extern InputState GetMouseButton(GlfwWindow window, MouseButton button);

    /// <summary>
    ///     Sets the user-defined pointer of the specified window. The current value is retained until the window
    ///     is destroyed. The initial value is <see cref="IntPtr.Zero" />.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="userPointer">The user pointer value.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowUserPointer(GlfwWindow window, nint userPointer);

    /// <summary>
    ///     Gets the current value of the user-defined pointer of the specified window. The initial value is
    ///     <see cref="IntPtr.Zero" />.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The user-defined pointer.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetWindowUserPointer(GlfwWindow window);

    /// <summary>
    ///     Sets the size limits of the client area of the specified window. If the window is full screen, the
    ///     size limits only take effect once it is made windowed. If the window is not resizable, this function does nothing.
    ///     <para>The size limits are applied immediately to a windowed mode window and may cause it to be resized.</para>
    ///     <para>
    ///         The maximum dimensions must be greater than or equal to the minimum dimensions and all must be greater than
    ///         or equal to zero.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="minWidth">The minimum width of the client area.</param>
    /// <param name="minHeight">The minimum height of the client area.</param>
    /// <param name="maxWidth">The maximum width of the client area.</param>
    /// <param name="maxHeight">The maximum height of the client area.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSizeLimits", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowSizeLimits(GlfwWindow window, int minWidth, int minHeight, int maxWidth,
        int maxHeight);

    /// <summary>
    ///     Sets the required aspect ratio of the client area of the specified window. If the window is full
    ///     screen, the aspect ratio only takes effect once it is made windowed. If the window is not resizable, this function
    ///     does nothing.
    ///     <para>
    ///         The aspect ratio is specified as a numerator and a denominator and both values must be greater than zero. For
    ///         example, the common 16:9 aspect ratio is specified as 16 and 9, respectively.
    ///     </para>
    ///     <para>
    ///         If the numerator and denominator is set to <see cref="Constants.Default" /> then the aspect ratio limit is
    ///         disabled.
    ///     </para>
    ///     <para>The aspect ratio is applied immediately to a windowed mode window and may cause it to be resized.</para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="numerator">The numerator of the desired aspect ratio.</param>
    /// <param name="denominator">The denominator of the desired aspect ratio.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowAspectRatio", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetWindowAspectRatio(GlfwWindow window, int numerator, int denominator);

    [DllImport(LIBRARY, EntryPoint = "glfwGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
    private static extern GlfwWindow GetCurrentContext();

    /// <summary>
    ///     Gets the size, in millimeters, of the display area of the specified monitor.
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <param name="width">The width, in millimeters, of the monitor's display area.</param>
    /// <param name="height">The height, in millimeters, of the monitor's display area.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetMonitorPhysicalSize(GlfwMonitor monitor, out int width, out int height);

    /// <summary>
    ///     Gets the position, in screen coordinates, of the upper-left corner of the specified monitor.
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <param name="x">The monitor x-coordinate.</param>
    /// <param name="y">The monitor y-coordinate.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorPos", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetMonitorPosition(GlfwMonitor monitor, out int x, out int y);

    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitors", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetMonitors(out int count);

    /// <summary>
    ///     Sets the character callback of the specified window, which is called when a Unicode character is
    ///     input.
    ///     <para>
    ///         The character callback is intended for Unicode text input. As it deals with characters, it is keyboard layout
    ///         dependent, whereas the key callback is not. Characters do not map 1:1 to physical keys, as a key may produce
    ///         zero, one or more characters. If you want to know whether a specific physical key was pressed or released, see
    ///         the key callback instead.
    ///     </para>
    ///     <para>
    ///         The character callback behaves as system text input normally does and will not be called if modifier keys are
    ///         held down that would prevent normal text input on that platform, for example a Super (Command) key on OS X or
    ///         Alt key on Windows. There is a character with modifiers callback that receives these events.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="charCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCharCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwCharCallback))]
    public static extern GlfwCharCallback SetCharCallback(GlfwWindow window, GlfwCharCallback charCallback);

    /// <summary>
    ///     Sets the character with modifiers callback of the specified window, which is called when a Unicode
    ///     character is input regardless of what modifier keys are used.
    ///     <para>
    ///         The character with modifiers callback is intended for implementing custom Unicode character input. For
    ///         regular Unicode text input, see the character callback. Like the character callback, the character with
    ///         modifiers callback deals with characters and is keyboard layout dependent. Characters do not map 1:1 to
    ///         physical keys, as a key may produce zero, one or more characters. If you want to know whether a specific
    ///         physical key was pressed or released, see the key callback instead.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="charCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or an error occurred.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetCharModsCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwCharModsCallback))]
    public static extern GlfwCharModsCallback SetCharModsCallback(GlfwWindow window, GlfwCharModsCallback charCallback);

    /// <summary>
    ///     Gets the last state reported for the specified key to the specified window.
    ///     <para>The higher-level action <see cref="InputState.Repeat" /> is only reported to the key callback.</para>
    ///     <para>
    ///         If the sticky keys input mode is enabled, this function returns <see cref="InputState.Press" /> the first
    ///         time you call it for a key that was pressed, even if that key has already been released.
    ///     </para>
    ///     <para>
    ///         The key functions deal with physical keys, with key tokens named after their use on the standard US keyboard
    ///         layout. If you want to input text, use the Unicode character callback instead.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="key">The key to query.</param>
    /// <returns>Either <see cref="InputState.Press" /> or <see cref="InputState.Release" />.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetKey", CallingConvention = CallingConvention.Cdecl)]
    public static extern InputState GetKey(GlfwWindow window, Keys key);

    [DllImport(LIBRARY, EntryPoint = "glfwGetKeyName", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetKeyNameInternal(Keys key, int scanCode);

    /// <summary>
    ///     Sets the framebuffer resize callback of the specified window, which is called when the framebuffer of
    ///     the specified window is resized.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="sizeCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwSizeCallback))]
    public static extern GlfwSizeCallback SetFramebufferSizeCallback(GlfwWindow window, GlfwSizeCallback sizeCallback);

    /// <summary>
    ///     Sets the refresh callback of the specified window, which is called when the client area of the window
    ///     needs to be redrawn, for example if the window has been exposed after having been covered by another window.
    ///     <para>
    ///         On compositing window systems such as Aero, Compiz or Aqua, where the window contents are saved off-screen,
    ///         this callback may be called only very infrequently or never at all.
    ///     </para>
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwWindowCallback))]
    public static extern GlfwWindowCallback SetWindowRefreshCallback(GlfwWindow window, GlfwWindowCallback callback);

    /// <summary>
    ///     Sets the key callback of the specified window, which is called when a key is pressed, repeated or
    ///     released.
    ///     <para>
    ///         The key functions deal with physical keys, with layout independent key tokens named after their values in the
    ///         standard US keyboard layout. If you want to input text, use the character callback instead.
    ///     </para>
    ///     <para>
    ///         When a window loses input focus, it will generate synthetic key release events for all pressed keys. You can
    ///         tell these events from user-generated events by the fact that the synthetic ones are generated after the focus
    ///         loss event has been processed, i.e. after the window focus callback has been called.
    ///     </para>
    ///     <para>
    ///         The scancode of a key is specific to that platform or sometimes even to that machine. Scancodes are intended
    ///         to allow users to bind keys that don't have a GLFW key token. Such keys have key set to
    ///         <see cref="Keys.Unknown" />, their state is not saved and so it cannot be queried with <see cref="GetKey" />.
    ///     </para>
    ///     <para>Sometimes GLFW needs to generate synthetic key events, in which case the scancode may be zero.</para>
    /// </summary>
    /// <param name="window">The new key callback, or <c>null</c> to remove the currently set callback.</param>
    /// <param name="keyCallback">The key callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetKeyCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwKeyCallback))]
    public static extern GlfwKeyCallback SetKeyCallback(GlfwWindow window, GlfwKeyCallback keyCallback);

    /// <summary>
    ///     Gets whether the specified joystick is present.
    /// </summary>
    /// <param name="joystick">The joystick to query.</param>
    /// <returns><c>true</c> if the joystick is present, or <c>false</c> otherwise.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwJoystickPresent", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool JoystickPresent(Joystick joystick);

    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickName", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetJoystickNameInternal(Joystick joystick);

    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickAxes", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetJoystickAxes(Joystick joystic, out int count);

    [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickButtons", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetJoystickButtons(Joystick joystick, out int count);

    /// <summary>
    ///     Sets the joystick configuration callback, or removes the currently set callback.
    ///     <para>This is called when a joystick is connected to or disconnected from the system.</para>
    /// </summary>
    /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetJoystickCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwJoystickCallback))]
    public static extern GlfwJoystickCallback SetJoystickCallback(GlfwJoystickCallback callback);

    /// <summary>
    ///     Sets the monitor configuration callback, or removes the currently set callback. This is called when a
    ///     monitor is connected to or disconnected from the system.
    /// </summary>
    /// <param name="monitorCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetMonitorCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwMonitorCallback))]
    public static extern GlfwMonitorCallback SetMonitorCallback(GlfwMonitorCallback monitorCallback);

    /// <summary>
    ///     Sets the iconification callback of the specified window, which is called when the window is iconified
    ///     or restored.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
    /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(GlfwIconifyCallback))]
    public static extern GlfwIconifyCallback SetWindowIconifyCallback(GlfwWindow window, GlfwIconifyCallback callback);

    /// <summary>
    ///     Sets an input mode option for the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mode">The mode to set a new value for.</param>
    /// <param name="value">The new value of the specified input mode.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwSetInputMode", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetInputMode(GlfwWindow window, InputMode mode, int value);

    /// <summary>
    ///     Gets the value of an input option for the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="mode">The mode to query.</param>
    /// <returns>Dependent on mode being queried.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetInputMode", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetInputMode(GlfwWindow window, InputMode mode);

    /// <summary>
    ///     Returns the position, in screen coordinates, of the upper-left corner of the work area of the specified
    ///     monitor along with the work area size in screen coordinates.
    ///     <para>
    ///         The work area is defined as the area of the monitor not occluded by the operating system task bar
    ///         where present. If no task bar exists then the work area is the monitor resolution in screen
    ///         coordinates.
    ///     </para>
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="width">The monitor width.</param>
    /// <param name="height">The monitor height.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetMonitorWorkArea(nint monitor, out int x, out int y, out int width,
        out int height);

    [DllImport(LIBRARY, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint GetProcAddress(byte[] procName);

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    [DllImport(LIBRARY, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WindowHint(GlfwHint hint, int value);

    /// <summary>
    ///     Gets the value of the specified window attribute.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="attribute">The attribute to retrieve.</param>
    /// <returns>The value of the <paramref name="attribute" />.</returns>
    [DllImport(LIBRARY, EntryPoint = "glfwGetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetWindowAttribute(GlfwWindow window, int attribute);

    [DllImport(LIBRARY, EntryPoint = "glfwGetError", CallingConvention = CallingConvention.Cdecl)]
    private static extern ErrorCode GetErrorPrivate(out nint description);

    /// <summary>
    ///     This function creates a window and its associated OpenGL or OpenGL ES context. Most of the options controlling how
    ///     the window and its context should be created are specified with window hints.
    /// </summary>
    /// <param name="width">The desired width, in screen coordinates, of the window. This must be greater than zero.</param>
    /// <param name="height">The desired height, in screen coordinates, of the window. This must be greater than zero.</param>
    /// <param name="title">The initial window title.</param>
    /// <param name="monitor">The monitor to use for full screen mode, or <see cref="GlfwMonitor.None" /> for windowed mode.</param>
    /// <param name="share">
    ///     A window instance whose context to share resources with, or <see cref="GlfwWindow.None" /> to not share
    ///     resources..
    /// </param>
    /// <returns>The created window, or <see cref="GlfwWindow.None" /> if an error occurred.</returns>
    public static GlfwWindow CreateWindow(int width, int height, [NotNull] string title, GlfwMonitor monitor, GlfwWindow share)
    {
        return CreateWindow(width, height, Encoding.UTF8.GetBytes(title), monitor, share);
    }

    /// <summary>
    ///     Gets the client API.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The client API.</returns>
    public static ClientApi GetClientApi(GlfwWindow window)
    {
        return (ClientApi) GetWindowAttribute(window, (int) ContextAttributes.ClientApi);
    }

    /// <summary>
    ///     Gets the contents of the system clipboard, if it contains or is convertible to a UTF-8 encoded
    ///     string.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The contents of the clipboard as a UTF-8 encoded string, or <c>null</c> if an error occurred.</returns>
    public static string GetClipboardString(GlfwWindow window)
    {
        return Util.PtrToStringUTF8(GetClipboardStringInternal(window));
    }

    /// <summary>
    ///     Gets the API used to create the context of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The API used to create the context.</returns>
    public static ContextApi GetContextCreationApi(GlfwWindow window)
    {
        return (ContextApi) GetWindowAttribute(window, (int) ContextAttributes.ContextCreationApi);
    }

    /// <summary>
    ///     Gets the context version of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>The context version.</returns>
    public static Version GetContextVersion(GlfwWindow window)
    {
        GetContextVersion(window, out var major, out var minor, out var revision);
        return new Version(major, minor, revision);
    }

    /// <summary>
    ///     Gets whether the specified API extension is supported by the current OpenGL or OpenGL ES context.
    ///     <para>It searches both for client API extension and context creation API extensions.</para>
    /// </summary>
    /// <param name="extension">The extension name.</param>
    /// <returns><c>true</c> if the extension is supported; otherwise <c>false</c>.</returns>
    public static bool GetExtensionSupported(string extension)
    {
        return GetExtensionSupported(Encoding.ASCII.GetBytes(extension));
    }

    /// <summary>
    ///     Gets the current gamma ramp of the specified monitor.
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>The current gamma ramp, or empty structure if an error occurred.</returns>
    public static GlfwGammaRamp GetGammaRamp(GlfwMonitor monitor)
    {
        return (GlfwGammaRamp) Marshal.PtrToStructure<GlfwGammaRampInternal>(GetGammaRampInternal(monitor));
    }

    /// <summary>
    ///     Gets value indicating if specified window is using a debug context.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns><c>true</c> if window context is debug context, otherwise <c>false</c>.</returns>
    public static bool GetIsDebugContext(GlfwWindow window)
    {
        return GetWindowAttribute(window, (int) ContextAttributes.OpenglDebugContext) == (int) Constants.True;
    }

    /// <summary>
    ///     Gets value indicating if specified window is using a forward compatible context.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns><c>true</c> if window context is forward compatible, otherwise <c>false</c>.</returns>
    public static bool GetIsForwardCompatible(GlfwWindow window)
    {
        return GetWindowAttribute(window, (int) ContextAttributes.OpenglForwardCompat) == (int) Constants.True;
    }

    /// <summary>
    ///     Gets the values of all axes of the specified joystick. Each element in the array is a value
    ///     between -1.0 and 1.0.
    ///     <para>
    ///         Querying a joystick slot with no device present is not an error, but will return an empty array. Call
    ///         <see cref="JoystickPresent" /> to check device presence.
    ///     </para>
    /// </summary>
    /// <param name="joystick">The joystick to query.</param>
    /// <returns>An array of axes values.</returns>
    public static float[] GetJoystickAxes(Joystick joystick)
    {
        var ptr = GetJoystickAxes(joystick, out var count);
        var axes = new float[count];
        if (count > 0)
            Marshal.Copy(ptr, axes, 0, count);
        return axes;
    }

    /// <summary>
    ///     Gets the state of all buttons of the specified joystick.
    /// </summary>
    /// <param name="joystick">The joystick to query.</param>
    /// <returns>An array of values, either <see cref="InputState.Press" /> and <see cref="InputState.Release" />.</returns>
    public static InputState[] GetJoystickButtons(Joystick joystick)
    {
        var ptr = GetJoystickButtons(joystick, out var count);
        var states = new InputState[count];
        for (var i = 0; i < count; i++)
            states[i] = (InputState) Marshal.ReadByte(ptr, i);
        return states;
    }

    /// <summary>
    ///     Gets the name of the specified joystick.
    ///     <para>
    ///         Querying a joystick slot with no device present is not an error. <see cref="JoystickPresent" /> to check
    ///         device presence.
    ///     </para>
    /// </summary>
    /// <param name="joystick">The joystick to query.</param>
    /// <returns>The name of the joystick, or <c>null</c> if the joystick is not present or an error occurred.</returns>
    public static string GetJoystickName(Joystick joystick)
    {
        return Util.PtrToStringUTF8(GetJoystickNameInternal(joystick));
    }

    /// <summary>
    ///     Gets the localized name of the specified printable key. This is intended for displaying key
    ///     bindings to the user.
    ///     <para>
    ///         If the key is <see cref="Keys.Unknown" />, the scancode is used instead, otherwise the scancode is ignored.
    ///         If a non-printable key or (if the key is <see cref="Keys.Unknown" />) a scancode that maps to a non-printable
    ///         key is specified, this function returns NULL.
    ///     </para>
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <param name="scanCode">The scancode of the key to query.</param>
    /// <returns>The localized name of the key.</returns>
    public static string GetKeyName(Keys key, int scanCode)
    {
        return Util.PtrToStringUTF8(GetKeyNameInternal(key, scanCode));
    }

    /// <summary>
    ///     Gets a human-readable name, encoded as UTF-8, of the specified monitor.
    ///     <para>
    ///         The name typically reflects the make and model of the monitor and is not guaranteed to be unique among the
    ///         connected monitors.
    ///     </para>
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>The name of the monitor, or <c>null</c> if an error occurred.</returns>
    public static string GetMonitorName(GlfwMonitor monitor)
    {
        return Util.PtrToStringUTF8(GetMonitorNameInternal(monitor));
    }

    /// <summary>
    ///     Gets the address of the specified OpenGL or OpenGL ES core or extension function, if it is
    ///     supported by the current context.
    ///     This function does not apply to Vulkan. If you are rendering with Vulkan, use
    ///     <see cref="Vulkan.GetInstanceProcAddress" /> instead.
    /// </summary>
    /// <param name="procName">Name of the function.</param>
    /// <returns>The address of the function, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
    public static nint GetProcAddress(string procName)
    {
        return GetProcAddress(Encoding.ASCII.GetBytes(procName));
    }

    /// <summary>
    ///     Gets the profile of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>Profile of the window.</returns>
    public static Profile GetProfile(GlfwWindow window)
    {
        return (Profile) GetWindowAttribute(window, (int) ContextAttributes.OpenglProfile);
    }

    /// <summary>
    ///     Gets the robustness value of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <returns>Current set value of the robustness.</returns>
    public static Robustness GetRobustness(GlfwWindow window)
    {
        return (Robustness) GetWindowAttribute(window, (int) ContextAttributes.ContextRobustness);
    }

    /// <summary>
    ///     Gets the current video mode of the specified monitor.
    ///     <para>
    ///         If you have created a full screen window for that monitor, the return value will depend on whether that
    ///         window is iconified.
    ///     </para>
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>The current mode of the monitor, or <c>null</c> if an error occurred.</returns>
    public static GlfwVideoMode GetVideoMode(GlfwMonitor monitor)
    {
        var ptr = GetVideoModeInternal(monitor);
        return Marshal.PtrToStructure<GlfwVideoMode>(ptr);
    }

    /// <summary>
    ///     Gets an array of all video modes supported by the specified monitor.
    ///     <para>
    ///         The returned array is sorted in ascending order, first by color bit depth (the sum of all channel depths) and
    ///         then by resolution area (the product of width and height).
    ///     </para>
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>The array of video modes.</returns>
    public static GlfwVideoMode[] GetVideoModes(GlfwMonitor monitor)
    {
        var pointer = GetVideoModes(monitor, out var count);
        var modes = new GlfwVideoMode[count];
        for (var i = 0; i < count; i++, pointer += Marshal.SizeOf<GlfwVideoMode>())
            modes[i] = Marshal.PtrToStructure<GlfwVideoMode>(pointer);
        return modes;
    }

    /// <summary>
    ///     Gets the value of an attribute of the specified window or its OpenGL or OpenGL ES context.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="attribute">The window attribute whose value to return.</param>
    /// <returns>The value of the attribute, or zero if an error occurred.</returns>
    public static bool GetWindowAttribute(GlfwWindow window, WindowAttribute attribute)
    {
        return GetWindowAttribute(window, (int) attribute) == (int) Constants.True;
    }

    /// <summary>
    ///     Sets the system clipboard to the specified string.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="str">The string to set to the clipboard.</param>
    public static void SetClipboardString(GlfwWindow window, string str)
    {
        SetClipboardString(window, Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    ///     Sets the window title, encoded as UTF-8, of the specified window.
    /// </summary>
    /// <param name="window">A window instance.</param>
    /// <param name="title">The title to set.</param>
    public static void SetWindowTitle(GlfwWindow window, string title)
    {
        SetWindowTitle(window, Encoding.UTF8.GetBytes(title));
    }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, bool value)
    {
        WindowHint(hint, value ? Constants.True : Constants.False);
    }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, ClientApi value) { WindowHint(hint, (int) value); }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, Constants value) { WindowHint(hint, (int) value); }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, ContextApi value) { WindowHint(hint, (int) value); }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, Robustness value) { WindowHint(hint, (int) value); }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, Profile value) { WindowHint(hint, (int) value); }

    /// <summary>
    ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
    ///     until changed by a call to <see cref="WindowHint(GlfwHint, int)" /> or <see cref="DefaultWindowHints" />, or until the
    ///     library is
    ///     terminated.
    ///     <para>
    ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
    ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
    ///     </para>
    /// </summary>
    /// <param name="hint">The hint.</param>
    /// <param name="value">The value.</param>
    public static void WindowHint(GlfwHint hint, ReleaseBehavior value) { WindowHint(hint, (int) value); }

    private static void GetContextVersion(GlfwWindow window, out int major, out int minor, out int revision)
    {
        major = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionMajor);
        minor = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionMinor);
        revision = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionRevision);
    }

    private static void GlfwError(ErrorCode code, nint message)
    {
        throw new GlfwException(Util.PtrToStringUTF8(message));
    }

    /// <summary>
    ///     Implements the Vulkan specific functions of GLFW.
    ///     <para>See http://www.glfw.org/docs/latest/vulkan_guide.html for detailed documentation.</para>
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class Vulkan
    {
        /// <summary>
        ///     Gets whether the Vulkan loader has been found. This check is performed by <see cref="Glfw.Init" />.
        /// </summary>
        /// <value>
        ///     <c>true</c> if Vulkan is supported; otherwise <c>false</c>.
        /// </value>
        public static bool IsSupported => VulkanSupported();

        /// <summary>
        ///     This function creates a Vulkan surface for the specified window.
        /// </summary>
        /// <param name="vulkan">A pointer to the Vulkan instance.</param>
        /// <param name="window">The window handle.</param>
        /// <param name="allocator">A pointer to the allocator to use, or <see cref="IntPtr.Zero" /> to use default allocator.</param>
        /// <param name="surface">The handle to the created Vulkan surface.</param>
        /// <returns>VK_SUCCESS if successful, or a Vulkan error code if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwCreateWindowSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CreateWindowSurface(nint vulkan, nint window, nint allocator, out nint surface);

        /// <summary>
        ///     This function returns whether the specified queue family of the specified physical device supports presentation to
        ///     the platform GLFW was built for.
        /// </summary>
        /// <param name="instance">The instance that the physical device belongs to.</param>
        /// <param name="device">The physical device that the queue family belongs to.</param>
        /// <param name="family">The index of the queue family to query.</param>
        /// <returns><c>true</c> if the queue family supports presentation, or <c>false</c> otherwise.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetPhysicalDevicePresentationSupport",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetPhysicalDevicePresentationSupport(nint instance, nint device, uint family);

        [DllImport(LIBRARY, EntryPoint = "glfwGetInstanceProcAddress",
            CallingConvention = CallingConvention.Cdecl)]
        private static extern nint GetInstanceProcAddress(nint vulkan, byte[] procName);

        [DllImport(LIBRARY, EntryPoint = "glfwGetRequiredInstanceExtensions",
            CallingConvention = CallingConvention.Cdecl)]
        private static extern nint GetRequiredInstanceExtensions(out uint count);

        [DllImport(LIBRARY, EntryPoint = "glfwVulkanSupported", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool VulkanSupported();

        /// <summary>
        ///     This function returns the address of the specified Vulkan core or extension function for the specified instance. If
        ///     instance is set to <see cref="IntPtr.Zero" /> it can return any function exported from the Vulkan loader.
        ///     <para>
        ///         If Vulkan is not available on the machine, this function returns <see cref="IntPtr.Zero" /> and generates an
        ///         error. Use <see cref="IsSupported" /> to check whether Vulkan is available.
        ///     </para>
        /// </summary>
        /// <param name="vulkan">The vulkan instance.</param>
        /// <param name="procName">Name of the function.</param>
        /// <returns>The address of the function, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        public static nint GetInstanceProcAddress(nint vulkan, [NotNull] string procName)
        {
            return GetInstanceProcAddress(vulkan, Encoding.ASCII.GetBytes(procName));
        }

        /// <summary>
        ///     This function returns an array of names of Vulkan instance extensions required by GLFW for creating Vulkan surfaces
        ///     for GLFW windows. If successful, the list will always contains VK_KHR_surface, so if you don't require any
        ///     additional extensions you can pass this list directly to the VkInstanceCreateInfo struct.
        ///     <para>
        ///         If Vulkan is not available on the machine, this function returns generates an error, use
        ///         <see cref="IsSupported" /> to first check if supported.
        ///     </para>
        ///     <para>
        ///         If Vulkan is available but no set of extensions allowing window surface creation was found, this function
        ///         returns an empty array. You may still use Vulkan for off-screen rendering and compute work.
        ///     </para>
        /// </summary>
        /// <returns>An array of extension names.</returns>
        public static string[] GetRequiredInstanceExtensions()
        {
            var ptr = GetRequiredInstanceExtensions(out var count);
            var extensions = new string[count];
            if (count > 0 && ptr != IntPtr.Zero)
            {
                var offset = 0;
                for (var i = 0; i < count; i++, offset += IntPtr.Size)
                {
                    var p = Marshal.ReadIntPtr(ptr, offset);
                    extensions[i] = Marshal.PtrToStringAnsi(p)!;
                }
            }

            return extensions.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }
    }

    /// <summary>
    ///     Provides access to relevant native functions of the current operating system.
    ///     <para>
    ///         By using the native access functions you assert that you know what you're doing and how to fix problems
    ///         caused by using them.
    ///     </para>
    ///     <para>If you don't, you shouldn't be using them.</para>
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class Native
    {
        /// <summary>
        ///     Returns the CGDirectDisplayID of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The CGDirectDisplayID of the specified monitor, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetCocoaMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetCocoaMonitor(GlfwMonitor monitor);

        /// <summary>
        ///     Retrieves a pointer to the X11 display.
        ///     <para>The pointer is to a native <c>Display</c> struct defined by X11..</para>
        /// </summary>
        /// <returns>A pointer to the X11 display struct.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetX11Display", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetX11Display();

        /// <summary>
        ///     Retrieves a pointer to the Wayland display.
        ///     <para>The pointer is to a native <c>wl_display</c> struct defined in wayland-client.c.</para>
        /// </summary>
        /// <returns>A pointer to the Wayland display struct.</returns>
        /// <seealso href="https://github.com/msteinert/wayland/blob/master/src/wayland-client.c" />
        [DllImport(LIBRARY, EntryPoint = "glfwGetWaylandDisplay", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetWaylandDisplay();

        /// <summary>
        ///     Retrieves a pointer to the Wayland output monitor.
        ///     <para>The pointer is to a native <c>wl_output</c> struct defined in wayland-client.c.</para>
        /// </summary>
        /// <returns>A pointer to the Wayland output struct.</returns>
        /// <seealso href="https://github.com/msteinert/wayland/blob/master/src/wayland-client.c" />
        [DllImport(LIBRARY, EntryPoint = "glfwGetWaylandMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetWaylandMonitor(GlfwMonitor monitor);

        /// <summary>
        ///     Returns the pointer to the Wayland window for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>A pointer to a Wayland window, or <see cref="IntPtr.Zero" /> if error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWaylandWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetWaylandWindow(GlfwWindow window);

        /// <summary>
        ///     Returns the pointer to the GLX window for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>A pointer to a GLX window, or <see cref="IntPtr.Zero" /> if error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetGLXWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetGLXWindow(GlfwWindow window);

        /// <summary>
        ///     Returns the pointer to the X11 window for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>A pointer to an X11 window, or <see cref="IntPtr.Zero" /> if error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetX11Window", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetX11Window(GlfwWindow window);

        /// <summary>
        ///     Returns the RROutput of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The RROutput of the specified monitor, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetX11Monitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetX11Monitor(GlfwMonitor monitor);

        /// <summary>
        ///     Returns the RRCrtc of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The RRCrtc of the specified monitor, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetX11Adapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetX11Adapter(GlfwMonitor monitor);

        /// <summary>
        ///     Returns the pointer to the Cocoa window for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>A pointer to a Cocoa window, or <see cref="IntPtr.Zero" /> if error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetCocoaWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetCocoaWindow(GlfwWindow window);

        /// <summary>
        ///     Returns the NSOpenGLContext of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The NSOpenGLContext of the specified window, or <see cref="NSOpenGLContext.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetNSGLContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern NSOpenGLContext GetNSGLContext(GlfwWindow window);

        /// <summary>
        ///     Returns the OSMesaContext of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The OSMesaContext of the specified window, or <see cref="OSMesaContext.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetOSMesaContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern OSMesaContext GetOSMesaContext(GlfwWindow window);

        /// <summary>
        ///     Returns the GLXContext of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The GLXContext of the specified window, or <see cref="GLXContext.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetGLXContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern GLXContext GetGLXContext(GlfwWindow window);

        /// <summary>
        ///     Returns the EGLContext of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The EGLContext of the specified window, or <see cref="EGLContext.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetEGLContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern EGLContext GetEglContext(GlfwWindow window);

        /// <summary>
        ///     Returns the EGLDisplay used by GLFW.
        /// </summary>
        /// <returns>The EGLDisplay used by GLFW, or <see cref="EGLDisplay.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetEGLDisplay", CallingConvention = CallingConvention.Cdecl)]
        public static extern EGLDisplay GetEglDisplay();

        /// <summary>
        ///     Returns the <see cref="EGLSurface" /> of the specified window
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The EGLSurface of the specified window, or <see cref="EGLSurface.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetEGLSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern EGLSurface GetEglSurface(GlfwWindow window);

        /// <summary>
        ///     Returns the WGL context of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The WGL context of the specified window, or <see cref="EGLContext.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWGLContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern HGLRC GetWglContext(GlfwWindow window);

        /// <summary>
        ///     Returns the HWND of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The HWND of the specified window, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWin32Window", CallingConvention = CallingConvention.Cdecl)]
        public static extern nint GetWin32Window(GlfwWindow window);

        /// <summary>
        ///     Returns the contents of the selection as a string.
        /// </summary>
        /// <returns>The selected string, or <c>null</c> if error occurs or no string is selected.</returns>
        public static string? GetX11SelectionString()
        {
            var ptr = GetX11SelectionStringInternal();
            return ptr == IntPtr.Zero ? null : Util.PtrToStringUTF8(ptr);
        }

        /// <summary>
        ///     Sets the clipboard string of an X11 window.
        /// </summary>
        /// <param name="str">The string to set.</param>
        public static void SetX11SelectionString([NotNull] string str)
        {
            SetX11SelectionString(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        ///     Retrieves the color buffer associated with the specified window.
        /// </summary>
        /// <param name="window">The window whose color buffer to retrieve.</param>
        /// <param name="width">The width of the color buffer.</param>
        /// <param name="height">The height of the color buffer.</param>
        /// <param name="format">The pixel format of the color buffer.</param>
        /// <param name="buffer">A pointer to the first element in the buffer.</param>
        /// <returns><c>true</c> if operation was successful, otherwise <c>false</c>.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetOSMesaColorBuffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool
            GetOSMesaColorBuffer(GlfwWindow window, out int width, out int height, out int format, out nint buffer);

        /// <summary>
        ///     Retrieves the depth buffer associated with the specified window.
        /// </summary>
        /// <param name="window">The window whose depth buffer to retrieve.</param>
        /// <param name="width">The width of the depth buffer.</param>
        /// <param name="height">The height of the depth buffer.</param>
        /// <param name="bytesPerValue">The number of bytes per element in the buffer.</param>
        /// <param name="buffer">A pointer to the first element in the buffer.</param>
        /// <returns><c>true</c> if operation was successful, otherwise <c>false</c>.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetOSMesaDepthBuffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool
            GetOSMesaDepthBuffer(GlfwWindow window, out int width, out int height, out int bytesPerValue,
                out nint buffer);


        [DllImport(LIBRARY, EntryPoint = "glfwSetX11SelectionString", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetX11SelectionString([NotNull] byte[] str);

        [DllImport(LIBRARY, EntryPoint = "glfwGetX11SelectionString", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint GetX11SelectionStringInternal();

        [DllImport(LIBRARY, EntryPoint = "glfwGetWin32Adapter", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint GetWin32AdapterInternal(GlfwMonitor monitor);

        [DllImport(LIBRARY, EntryPoint = "glfwGetWin32Monitor", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint GetWin32MonitorInternal(GlfwMonitor monitor);

        /// <summary>
        ///     Gets the win32 adapter.
        /// </summary>
        /// <param name="monitor">A monitor instance.</param>
        /// <returns>dapter device name (for example \\.\DISPLAY1) of the specified monitor, or <c>null</c> if an error occurred.</returns>
        public static string GetWin32Adapter(GlfwMonitor monitor)
        {
            return Util.PtrToStringUTF8(GetWin32AdapterInternal(monitor));
        }

        /// <summary>
        ///     Returns the display device name of the specified monitor
        /// </summary>
        /// <param name="monitor">A monitor instance.</param>
        /// <returns>
        ///     The display device name (for example \\.\DISPLAY1\Monitor0) of the specified monitor, or <c>null</c> if an
        ///     error occurred.
        /// </returns>
        public static string GetWin32Monitor(GlfwMonitor monitor)
        {
            return Util.PtrToStringUTF8(GetWin32MonitorInternal(monitor));
        }
    }
}