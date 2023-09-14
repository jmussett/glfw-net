namespace GLFW;

/// <summary>
/// Base exception class for GLFW related errors.
/// </summary>
public class GlfwException : System.Exception
{
    /// <summary>
    ///     Generic error messages if only an error code is supplied as an argument to the constructor.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <returns>Error message.</returns>
    public static string GetErrorMessage(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.NotInitialized => Strings.NotInitialized,
            ErrorCode.NoCurrentContext => Strings.NoCurrentContext,
            ErrorCode.InvalidEnum => Strings.InvalidEnum,
            ErrorCode.InvalidValue => Strings.InvalidValue,
            ErrorCode.OutOfMemory => Strings.OutOfMemory,
            ErrorCode.ApiUnavailable => Strings.ApiUnavailable,
            ErrorCode.VersionUnavailable => Strings.VersionUnavailable,
            ErrorCode.PlatformError => Strings.PlatformError,
            ErrorCode.FormatUnavailable => Strings.FormatUnavailable,
            ErrorCode.NoWindowContext => Strings.NoWindowContext,
            _ => Strings.UnknownError,
        };
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GlfwException" /> class.
    /// </summary>
    /// <param name="error">The error code to create a generic message from.</param>
    public GlfwException(ErrorCode error) : base(GetErrorMessage(error)) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GlfwException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GlfwException(string message) : base(message) { }
}