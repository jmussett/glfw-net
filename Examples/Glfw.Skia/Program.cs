namespace Glfw.Skia
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using GLFW;
    using SkiaSharp;

    class Program
    {
        private static NativeWindow window;
        private static SKCanvas canvas;

        private static Keys? lastKeyPressed;
        private static Point? lastMousePosition;

        //----------------------------------
        //NOTE: On Windows you must copy SharedLib manually (https://github.com/ForeverZer0/glfw-net#microsoft-windows)
        //----------------------------------
        
        static void Main(string[] args)
        {
            using (window = new NativeWindow(800, 600, "Skia Example"))
            {
                SubscribeToWindowEvents();

                using var context = GenerateSkiaContext(window);
                using var skiaSurface = GenerateSkiaSurface(context, window.ClientSize);
                canvas = skiaSurface.Canvas;

                while (!window.IsClosing)
                {
                    Render();
                    Glfw.WaitEvents();
                }
            }
        }

        private static void SubscribeToWindowEvents()
        {
            window.SizeChanged += OnWindowsSizeChanged;
            window.Refreshed += OnWindowRefreshed;
            window.KeyPress += OnWindowKeyPress;
            window.MouseMoved += OnWindowMouseMoved;
        }

        private static GRContext GenerateSkiaContext(NativeWindow nativeWindow)
        {
            var nativeContext = GetNativeContext(nativeWindow);
            var glInterface = GRGlInterface.AssembleGlInterface(nativeContext, (contextHandle, name) => Glfw.GetProcAddress(name));
            return GRContext.Create(GRBackend.OpenGL, glInterface);
        }

        private static object GetNativeContext(NativeWindow nativeWindow)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Native.GetWglContext(nativeWindow);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // XServer
                return Native.GetGLXContext(nativeWindow);
                // Wayland
                //return Native.GetEglContext(nativeWindow);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Native.GetNSGLContext(nativeWindow);
            }
            
            throw new PlatformNotSupportedException();
        }
        
        private static SKSurface GenerateSkiaSurface(GRContext skiaContext, Size surfaceSize)
        {
            var frameBufferInfo = new GRGlFramebufferInfo((uint)new UIntPtr(0), GRPixelConfig.Rgba8888.ToGlSizedFormat());
            var backendRenderTarget = new GRBackendRenderTarget(surfaceSize.Width,
                                                                surfaceSize.Height,
                                                                0, 
                                                                8,
                                                                frameBufferInfo);
            return SKSurface.Create(skiaContext, backendRenderTarget, GRSurfaceOrigin.BottomLeft, SKImageInfo.PlatformColorType);
        }
        
        private static void Render()
        {
            canvas.Clear(SKColor.Parse("#F0F0F0"));
            var headerPaint = new SKPaint {Color = SKColor.Parse("#333333"), TextSize = 50, IsAntialias = true};
            canvas.DrawText("Hello from GLFW.NET + SkiaSharp!", 10, 60, headerPaint);
            
            var inputInfoPaint = new SKPaint {Color = SKColor.Parse("#F34336"), TextSize = 18, IsAntialias = true};
            canvas.DrawText($"Last key pressed: {lastKeyPressed}", 10, 90, inputInfoPaint);
            canvas.DrawText($"Last mouse position: {lastMousePosition}", 10, 120, inputInfoPaint);
            
            var exitInfoPaint = new SKPaint {Color = SKColor.Parse("#3F51B5"), TextSize = 18, IsAntialias = true};
            canvas.DrawText("Press Enter to Exit.", 10, 160, exitInfoPaint);

            canvas.Flush();
            window.SwapBuffers();
        }

        private static void OnWindowsSizeChanged(object sender, SizeChangeEventArgs e)
        {
            Render();
        }
        
        private static void OnWindowKeyPress(object sender, KeyEventArgs e)
        {
            lastKeyPressed = e.Key;
            if (e.Key == Keys.Enter || e.Key == Keys.NumpadEnter)
            {
                window.Close();
            }
        }

        private static void OnWindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            lastMousePosition = e.Position;
        }
        
        private static void OnWindowRefreshed(object sender, EventArgs e)
        {
            Render();
        }
    }
}