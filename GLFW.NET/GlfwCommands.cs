using System;
using System.Runtime.InteropServices;

namespace GLFW;

internal unsafe class GlfwCommands
{
    public delegate* unmanaged[Cdecl]<nint, float*, float*, void> glfwGetMonitorContentScale;
    public delegate* unmanaged[Cdecl]<nint, nint> glfwGetMonitorUserPointer;
    public delegate* unmanaged[Cdecl]<nint, nint, void> glfwSetMonitorUserPointer;
    public delegate* unmanaged[Cdecl]<nint, float> glfwGetWindowOpacity;
    public delegate* unmanaged[Cdecl]<nint, float, void> glfwSetWindowOpacity;
    public delegate* unmanaged[Cdecl]<Hint, byte*, void> glfwWindowHintString;
    public delegate* unmanaged[Cdecl]<nint, float*, float*> glfwGetWindowContentScale;
    public delegate* unmanaged[Cdecl]<nint, void> glfwRequestWindowAttention;
    public delegate* unmanaged[Cdecl]<bool> glfwRawMouseMotionSupported;
    public delegate* unmanaged[Cdecl]<nint, WindowMaximizedCallback, WindowMaximizedCallback> glfwSetWindowMaximizeCallback;
    public delegate* unmanaged[Cdecl]<nint, WindowContentsScaleCallback, WindowContentsScaleCallback> glfwSetWindowContentScaleCallback;
    public delegate* unmanaged[Cdecl]<Keys, int> glfwGetKeyScancode;
    public delegate* unmanaged[Cdecl]<nint, WindowAttribute, bool, void> glfwSetWindowAttrib;
    public delegate* unmanaged[Cdecl]<int, int*, nint> glfwGetJoystickHats;
    public delegate* unmanaged[Cdecl]<int, nint> glfwGetJoystickGUID;
    public delegate* unmanaged[Cdecl]<int, nint> glfwGetJoystickUserPointer;
    public delegate* unmanaged[Cdecl]<int, nint, void> glfwSetJoystickUserPointer;
    public delegate* unmanaged[Cdecl]<int, bool> glfwJoystickIsGamepad;
    public delegate* unmanaged[Cdecl]<byte*, bool> glfwUpdateGamepadMappings;
    public delegate* unmanaged[Cdecl]<int, nint> glfwGetGamepadName;
    public delegate* unmanaged[Cdecl]<int, GamePadState*, bool> glfwGetGamepadState;
    public delegate* unmanaged[Cdecl]<Hint, bool, void> glfwInitHint;
    public delegate* unmanaged[Cdecl]<bool> glfwInit;
    public delegate* unmanaged[Cdecl]<bool> glfwTerminate;
    public delegate* unmanaged[Cdecl]<ErrorCallback, ErrorCallback> glfwSetErrorCallback;
    public delegate* unmanaged[Cdecl]<int, int, byte*, Monitor, Window, Window> glfwCreateWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwDestroyWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwShowWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwHideWindow;
    public delegate* unmanaged[Cdecl]<Window, int*, nint*, void> glfwGetWindowPos;
    public delegate* unmanaged[Cdecl]<Window, int, int, void> glfwSetWindowPos;
    public delegate* unmanaged[Cdecl]<Window, int*, int*, void> glfwGetWindowSize;
    public delegate* unmanaged[Cdecl]<Window, int, int, void> glfwSetWindowSize;
    public delegate* unmanaged[Cdecl]<Window, int*, int*, void> glfwGetFramebufferSize;
    public delegate* unmanaged[Cdecl]<Window, PositionCallback, PositionCallback> glfwSetWindowPosCallback;
    public delegate* unmanaged[Cdecl]<Window, SizeCallback, SizeCallback> glfwSetWindowSizeCallback;
    public delegate* unmanaged[Cdecl]<Window, byte*, void> glfwSetWindowTitle;
    public delegate* unmanaged[Cdecl]<Window, void> glfwFocusWindow;
    public delegate* unmanaged[Cdecl]<Window, FocusCallback, FocusCallback> glfwSetWindowFocusCallback;
    public delegate* unmanaged[Cdecl]<int*, int*, int*, void> glfwGetVersion;
    public delegate* unmanaged[Cdecl]<nint> glfwGetVersionString;
    public delegate* unmanaged[Cdecl]<double> glfwGetTime;
    public delegate* unmanaged[Cdecl]<double, void> glfwSetTime;
    public delegate* unmanaged[Cdecl]<ulong> glfwGetTimerFrequency;
    public delegate* unmanaged[Cdecl]<ulong> glfwGetTimerValue;
    public delegate* unmanaged[Cdecl]<Window, int*, int*, int*, void> glfwGetWindowFrameSize;
    public delegate* unmanaged[Cdecl]<Window, void> glfwMaximizeWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwIconifyWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwRestoreWindow;
    public delegate* unmanaged[Cdecl]<Window, void> glfwMakeContextCurrent;
    public delegate* unmanaged[Cdecl]<Window, void> glfwSwapBuffers;
    public delegate* unmanaged[Cdecl]<int, void> glfwSwapInterval;
    public delegate* unmanaged[Cdecl]<byte*, bool> glfwExtensionSupported;
    public delegate* unmanaged[Cdecl]<void> glfwDefaultWindowHints;
    public delegate* unmanaged[Cdecl]<Window, bool> glfwWindowShouldClose;
    public delegate* unmanaged[Cdecl]<Window, bool, void> glfwSetWindowShouldClose;
    public delegate* unmanaged[Cdecl]<Window, int, Image*, void> glfwSetWindowIcon;
    public delegate* unmanaged[Cdecl]<void> glfwWaitEvents;
    public delegate* unmanaged[Cdecl]<void> glfwPollEvents;
    public delegate* unmanaged[Cdecl]<void> glfwPostEmptyEvent;
    public delegate* unmanaged[Cdecl]<double, void> glfwWaitEventsTimeout;
    public delegate* unmanaged[Cdecl]<Window, WindowCallback, WindowCallback> glfwSetWindowCloseCallback;
    public delegate* unmanaged[Cdecl]<Monitor> glfwGetPrimaryMonitor;
    public delegate* unmanaged[Cdecl]<Monitor, nint> glfwGetVideoMode;
    public delegate* unmanaged[Cdecl]<Monitor, int*, nint> glfwGetVideoModes;
    public delegate* unmanaged[Cdecl]<Window, Monitor> glfwGetWindowMonitor;
    public delegate* unmanaged[Cdecl]<Window, Monitor, int, int, int, int, int, void> glfwSetWindowMonitor;
    public delegate* unmanaged[Cdecl]<Monitor, nint> glfwGetGammaRamp;
    public delegate* unmanaged[Cdecl]<Monitor, GammaRamp, void> glfwSetGammaRamp;
    public delegate* unmanaged[Cdecl]<Monitor, float, void> glfwSetGamma;
    public delegate* unmanaged[Cdecl]<Window, nint> glfwGetClipboardString;
    public delegate* unmanaged[Cdecl]<Window, byte*, void> glfwSetClipboardString;
    public delegate* unmanaged[Cdecl]<Window, FileDropCallback, FileDropCallback> glfwSetDropCallback;
    public delegate* unmanaged[Cdecl]<Monitor, nint> glfwGetMonitorName;
    public delegate* unmanaged[Cdecl]<Image, int, int, Cursor> glfwCreateCursor;
    public delegate* unmanaged[Cdecl]<Cursor, void> glfwDestroyCursor;
    public delegate* unmanaged[Cdecl]<Window, Cursor, void> glfwSetCursor;
    public delegate* unmanaged[Cdecl]<CursorType, Cursor> glfwCreateStandardCursor;
    public delegate* unmanaged[Cdecl]<Window, double*, double*, void> glfwGetCursorPos;
    public delegate* unmanaged[Cdecl]<Window, double, double, void> glfwSetCursorPos;
    public delegate* unmanaged[Cdecl]<Window, MouseCallback, MouseCallback> glfwSetCursorPosCallback;
    public delegate* unmanaged[Cdecl]<Window, MouseEnterCallback, MouseEnterCallback> glfwSetCursorEnterCallback;
    public delegate* unmanaged[Cdecl]<Window, MouseButtonCallback, MouseButtonCallback> glfwSetMouseButtonCallback;
    public delegate* unmanaged[Cdecl]<Window, MouseCallback, MouseCallback> glfwSetScrollCallback;
    public delegate* unmanaged[Cdecl]<InputState, Window, MouseButton> glfwGetMouseButton;
    public delegate* unmanaged[Cdecl]<Window, nint, void> glfwSetWindowUserPointer;
    public delegate* unmanaged[Cdecl]<Window, nint> glfwGetWindowUserPointer;
    public delegate* unmanaged[Cdecl]<Window, int, int, int, int, void> glfwSetWindowSizeLimits;
    public delegate* unmanaged[Cdecl]<Window, int, int, void> glfwSetWindowAspectRatio;
    public delegate* unmanaged[Cdecl]<Window> glfwGetCurrentContext;
    public delegate* unmanaged[Cdecl]<Monitor, int*, int*, void> glfwGetMonitorPhysicalSize;
    public delegate* unmanaged[Cdecl]<Monitor, int*, int*, void> glfwGetMonitorPos;
    public delegate* unmanaged[Cdecl]<int*, nint> glfwGetMonitors;
    public delegate* unmanaged[Cdecl]<Window, CharCallback, CharCallback> glfwSetCharCallback;
    public delegate* unmanaged[Cdecl]<Window, CharModsCallback, CharModsCallback> glfwSetCharModsCallback;
    public delegate* unmanaged[Cdecl]<Window, Keys, InputState> glfwGetKey;
    public delegate* unmanaged[Cdecl]<Keys, int, nint> glfwGetKeyName;
    public delegate* unmanaged[Cdecl]<Window, SizeCallback, SizeCallback> glfwSetFramebufferSizeCallback;
    public delegate* unmanaged[Cdecl]<Window, WindowCallback, WindowCallback> glfwSetWindowRefreshCallback;
    public delegate* unmanaged[Cdecl]<Window, KeyCallback, KeyCallback> glfwSetKeyCallback;
    public delegate* unmanaged[Cdecl]<Joystick, bool> glfwJoystickPresent;
    public delegate* unmanaged[Cdecl]<Joystick, nint> glfwGetJoystickName;
    public delegate* unmanaged[Cdecl]<Joystick, int*, nint> glfwGetJoystickAxes;
    public delegate* unmanaged[Cdecl]<Joystick, int*, nint> glfwGetJoystickButtons;
    public delegate* unmanaged[Cdecl]<JoystickCallback, JoystickCallback> glfwSetJoystickCallback;
    public delegate* unmanaged[Cdecl]<MonitorCallback, MonitorCallback> glfwSetMonitorCallback;
    public delegate* unmanaged[Cdecl]<Window, IconifyCallback, IconifyCallback> glfwSetWindowIconifyCallback;
    public delegate* unmanaged[Cdecl]<Window, InputMode, int, void> glfwSetInputMode;
    public delegate* unmanaged[Cdecl]<Window, InputMode, void> glfwGetInputMode;
    public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, int*, int*, void> glfwGetMonitorWorkarea;
    public delegate* unmanaged[Cdecl]<byte*, nint> glfwGetProcAddress;
    public delegate* unmanaged[Cdecl]<Hint, int, void> glfwWindowHint;
    public delegate* unmanaged[Cdecl]<Window, int, int> glfwGetWindowAttrib;
    public delegate* unmanaged[Cdecl]<nint*, ErrorCode> glfwGetError;

    public GlfwCommands(nint libraryHandle)
    {
        glfwGetMonitorContentScale = (delegate* unmanaged[Cdecl]<nint, float*, float*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorContentScale");

        glfwGetMonitorUserPointer = (delegate* unmanaged[Cdecl]<nint, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorUserPointer");

        glfwSetMonitorUserPointer = (delegate* unmanaged[Cdecl]<nint, nint, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetMonitorUserPointer");

        glfwGetWindowOpacity = (delegate* unmanaged[Cdecl]<nint, float>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowOpacity");

        glfwSetWindowOpacity = (delegate* unmanaged[Cdecl]<nint, float, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowOpacity");

        glfwWindowHintString = (delegate* unmanaged[Cdecl]<Hint, byte*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwWindowHintString");

        glfwGetWindowContentScale = (delegate* unmanaged[Cdecl]<nint, float*, float*>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowContentScale");

        glfwRequestWindowAttention = (delegate* unmanaged[Cdecl]<nint, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwRequestWindowAttention");

        glfwRawMouseMotionSupported = (delegate* unmanaged[Cdecl]<bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwRawMouseMotionSupported");

        glfwSetWindowMaximizeCallback = (delegate* unmanaged[Cdecl]<nint, WindowMaximizedCallback, WindowMaximizedCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowMaximizeCallback");

        glfwSetWindowContentScaleCallback = (delegate* unmanaged[Cdecl]<nint, WindowContentsScaleCallback, WindowContentsScaleCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowContentScaleCallback");

        glfwGetKeyScancode = (delegate* unmanaged[Cdecl]<Keys, int>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetKeyScancode");

        glfwSetWindowAttrib = (delegate* unmanaged[Cdecl]<nint, WindowAttribute, bool, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowAttrib");

        glfwGetJoystickHats = (delegate* unmanaged[Cdecl]<int, int*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickHats");

        glfwGetJoystickGUID = (delegate* unmanaged[Cdecl]<int, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickGUID");

        glfwGetJoystickUserPointer = (delegate* unmanaged[Cdecl]<int, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickUserPointer");

        glfwSetJoystickUserPointer = (delegate* unmanaged[Cdecl]<int, nint, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetJoystickUserPointer");

        glfwJoystickIsGamepad = (delegate* unmanaged[Cdecl]<int, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwJoystickIsGamepad");

        glfwUpdateGamepadMappings = (delegate* unmanaged[Cdecl]<byte*, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwUpdateGamepadMappings");

        glfwGetGamepadName = (delegate* unmanaged[Cdecl]<int, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetGamepadName");

        glfwGetGamepadState = (delegate* unmanaged[Cdecl]<int, GamePadState*, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetGamepadState");

        glfwInitHint = (delegate* unmanaged[Cdecl]<Hint, bool, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwInitHint");

        glfwInit = (delegate* unmanaged[Cdecl]<bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwInit");

        glfwTerminate = (delegate* unmanaged[Cdecl]<bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwTerminate");

        glfwSetErrorCallback = (delegate* unmanaged[Cdecl]<ErrorCallback, ErrorCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetErrorCallback");

        glfwCreateWindow = (delegate* unmanaged[Cdecl]<int, int, byte*, Monitor, Window, Window>)
            NativeLibrary.GetExport(libraryHandle, "glfwCreateWindow");

        glfwDestroyWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwDestroyWindow");

        glfwShowWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwShowWindow");

        glfwHideWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwHideWindow");

        glfwGetWindowPos = (delegate* unmanaged[Cdecl]<Window, int*, nint*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowPos");

        glfwSetWindowPos = (delegate* unmanaged[Cdecl]<Window, int, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowPos");

        glfwGetWindowSize = (delegate* unmanaged[Cdecl]<Window, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowSize");

        glfwSetWindowSize = (delegate* unmanaged[Cdecl]<Window, int, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowSize");

        glfwGetFramebufferSize = (delegate* unmanaged[Cdecl]<Window, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetFramebufferSize");

        glfwSetWindowPosCallback = (delegate* unmanaged[Cdecl]<Window, PositionCallback, PositionCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowPosCallback");

        glfwSetWindowSizeCallback = (delegate* unmanaged[Cdecl]<Window, SizeCallback, SizeCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowSizeCallback");

        glfwSetWindowTitle = (delegate* unmanaged[Cdecl]<Window, byte*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowTitle");

        glfwFocusWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwFocusWindow");

        glfwSetWindowFocusCallback = (delegate* unmanaged[Cdecl]<Window, FocusCallback, FocusCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowFocusCallback");

        glfwGetVersion = (delegate* unmanaged[Cdecl]<int*, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetVersion");

        glfwGetVersionString = (delegate* unmanaged[Cdecl]<nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetVersionString");

        glfwGetTime = (delegate* unmanaged[Cdecl]<double>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetTime");

        glfwSetTime = (delegate* unmanaged[Cdecl]<double, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetTime");

        glfwGetTimerFrequency = (delegate* unmanaged[Cdecl]<ulong>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetTimerFrequency");

        glfwGetTimerValue = (delegate* unmanaged[Cdecl]<ulong>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetTimerValue");

        glfwGetWindowFrameSize = (delegate* unmanaged[Cdecl]<Window, int*, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowFrameSize");

        glfwMaximizeWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwMaximizeWindow");

        glfwIconifyWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwIconifyWindow");

        glfwRestoreWindow = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwRestoreWindow");

        glfwMakeContextCurrent = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwMakeContextCurrent");

        glfwSwapBuffers = (delegate* unmanaged[Cdecl]<Window, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSwapBuffers");

        glfwSwapInterval = (delegate* unmanaged[Cdecl]<int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSwapInterval");

        glfwExtensionSupported = (delegate* unmanaged[Cdecl]<byte*, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwExtensionSupported");

        glfwDefaultWindowHints = (delegate* unmanaged[Cdecl]<void>)
            NativeLibrary.GetExport(libraryHandle, "glfwDefaultWindowHints");

        glfwWindowShouldClose = (delegate* unmanaged[Cdecl]<Window, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwWindowShouldClose");

        glfwSetWindowShouldClose = (delegate* unmanaged[Cdecl]<Window, bool, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowShouldClose");

        glfwSetWindowIcon = (delegate* unmanaged[Cdecl]<Window, int, Image*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowIcon");

        glfwWaitEvents = (delegate* unmanaged[Cdecl]<void>)
            NativeLibrary.GetExport(libraryHandle, "glfwWaitEvents");

        glfwPollEvents = (delegate* unmanaged[Cdecl]<void>)
            NativeLibrary.GetExport(libraryHandle, "glfwPollEvents");

        glfwPostEmptyEvent = (delegate* unmanaged[Cdecl]<void>)
            NativeLibrary.GetExport(libraryHandle, "glfwPostEmptyEvent");

        glfwWaitEventsTimeout = (delegate* unmanaged[Cdecl]<double, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwWaitEventsTimeout");

        glfwSetWindowCloseCallback = (delegate* unmanaged[Cdecl]<Window, WindowCallback, WindowCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowCloseCallback");

        glfwGetPrimaryMonitor = (delegate* unmanaged[Cdecl]<Monitor>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetPrimaryMonitor");

        glfwGetVideoMode = (delegate* unmanaged[Cdecl]<Monitor, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetVideoMode");

        glfwGetVideoModes = (delegate* unmanaged[Cdecl]<Monitor, int*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetVideoModes");

        glfwGetWindowMonitor = (delegate* unmanaged[Cdecl]<Window, Monitor>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowMonitor");

        glfwSetWindowMonitor = (delegate* unmanaged[Cdecl]<Window, Monitor, int, int, int, int, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowMonitor");

        glfwGetGammaRamp = (delegate* unmanaged[Cdecl]<Monitor, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetGammaRamp");

        glfwSetGammaRamp = (delegate* unmanaged[Cdecl]<Monitor, GammaRamp, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetGammaRamp");

        glfwSetGamma = (delegate* unmanaged[Cdecl]<Monitor, float, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetGamma");

        glfwGetClipboardString = (delegate* unmanaged[Cdecl]<Window, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetClipboardString");

        glfwSetClipboardString = (delegate* unmanaged[Cdecl]<Window, byte*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetClipboardString");

        glfwSetDropCallback = (delegate* unmanaged[Cdecl]<Window, FileDropCallback, FileDropCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetDropCallback");

        glfwGetMonitorName = (delegate* unmanaged[Cdecl]<Monitor, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorName");

        glfwCreateCursor = (delegate* unmanaged[Cdecl]<Image, int, int, Cursor>)
            NativeLibrary.GetExport(libraryHandle, "glfwCreateCursor");

        glfwDestroyCursor = (delegate* unmanaged[Cdecl]<Cursor, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwDestroyCursor");

        glfwSetCursor = (delegate* unmanaged[Cdecl]<Window, Cursor, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCursor");

        glfwCreateStandardCursor = (delegate* unmanaged[Cdecl]<CursorType, Cursor>)
            NativeLibrary.GetExport(libraryHandle, "glfwCreateStandardCursor");

        glfwGetCursorPos = (delegate* unmanaged[Cdecl]<Window, double*, double*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetCursorPos");

        glfwSetCursorPos = (delegate* unmanaged[Cdecl]<Window, double, double, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCursorPos");

        glfwSetCursorPosCallback = (delegate* unmanaged[Cdecl]<Window, MouseCallback, MouseCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCursorPosCallback");

        glfwSetCursorEnterCallback = (delegate* unmanaged[Cdecl]<Window, MouseEnterCallback, MouseEnterCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCursorEnterCallback");

        glfwSetMouseButtonCallback = (delegate* unmanaged[Cdecl]<Window, MouseButtonCallback, MouseButtonCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetMouseButtonCallback");

        glfwSetScrollCallback = (delegate* unmanaged[Cdecl]<Window, MouseCallback, MouseCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetScrollCallback");

        glfwGetMouseButton = (delegate* unmanaged[Cdecl]<InputState, Window, MouseButton>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMouseButton");

        glfwSetWindowUserPointer = (delegate* unmanaged[Cdecl]<Window, nint, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowUserPointer");

        glfwGetWindowUserPointer = (delegate* unmanaged[Cdecl]<Window, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowUserPointer");

        glfwSetWindowSizeLimits = (delegate* unmanaged[Cdecl]<Window, int, int, int, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowSizeLimits");

        glfwSetWindowAspectRatio = (delegate* unmanaged[Cdecl]<Window, int, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowAspectRatio");

        glfwGetCurrentContext = (delegate* unmanaged[Cdecl]<Window>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetCurrentContext");

        glfwGetMonitorPhysicalSize = (delegate* unmanaged[Cdecl]<Monitor, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorPhysicalSize");

        glfwGetMonitorPos = (delegate* unmanaged[Cdecl]<Monitor, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorPos");

        glfwGetMonitors = (delegate* unmanaged[Cdecl]<int*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitors");

        glfwSetCharCallback = (delegate* unmanaged[Cdecl]<Window, CharCallback, CharCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCharCallback");

        glfwSetCharModsCallback = (delegate* unmanaged[Cdecl]<Window, CharModsCallback, CharModsCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetCharModsCallback");

        glfwGetKey = (delegate* unmanaged[Cdecl]<Window, Keys, InputState>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetKey");

        glfwGetKeyName = (delegate* unmanaged[Cdecl]<Keys, int, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetKeyName");

        glfwSetFramebufferSizeCallback = (delegate* unmanaged[Cdecl]<Window, SizeCallback, SizeCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetFramebufferSizeCallback");

        glfwSetWindowRefreshCallback = (delegate* unmanaged[Cdecl]<Window, WindowCallback, WindowCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowRefreshCallback");

        glfwSetKeyCallback = (delegate* unmanaged[Cdecl]<Window, KeyCallback, KeyCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetKeyCallback");

        glfwJoystickPresent = (delegate* unmanaged[Cdecl]<Joystick, bool>)
            NativeLibrary.GetExport(libraryHandle, "glfwJoystickPresent");

        glfwGetJoystickName = (delegate* unmanaged[Cdecl]<Joystick, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickName");

        glfwGetJoystickAxes = (delegate* unmanaged[Cdecl]<Joystick, int*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickAxes");

        glfwGetJoystickButtons = (delegate* unmanaged[Cdecl]<Joystick, int*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetJoystickButtons");

        glfwSetJoystickCallback = (delegate* unmanaged[Cdecl]<JoystickCallback, JoystickCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetJoystickCallback");

        glfwSetMonitorCallback = (delegate* unmanaged[Cdecl]<MonitorCallback, MonitorCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetMonitorCallback");

        glfwSetWindowIconifyCallback = (delegate* unmanaged[Cdecl]<Window, IconifyCallback, IconifyCallback>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetWindowIconifyCallback");

        glfwSetInputMode = (delegate* unmanaged[Cdecl]<Window, InputMode, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwSetInputMode");

        glfwGetInputMode = (delegate* unmanaged[Cdecl]<Window, InputMode, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetInputMode");

        glfwGetMonitorWorkarea = (delegate* unmanaged[Cdecl]<IntPtr, int*, int*, int*, int*, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetMonitorWorkarea");

        glfwGetProcAddress = (delegate* unmanaged[Cdecl]<byte*, nint>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetProcAddress");

        glfwWindowHint = (delegate* unmanaged[Cdecl]<Hint, int, void>)
            NativeLibrary.GetExport(libraryHandle, "glfwWindowHint");

        glfwGetWindowAttrib = (delegate* unmanaged[Cdecl]<Window, int, int>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetWindowAttrib");

        glfwGetError = (delegate* unmanaged[Cdecl]<nint*, ErrorCode>)
            NativeLibrary.GetExport(libraryHandle, "glfwGetError");
    }
}
