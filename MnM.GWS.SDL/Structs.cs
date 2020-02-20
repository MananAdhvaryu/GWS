/* Licensed under the MIT/X11 license.
* Copyright (c) 2016-2018 jointly owned by eBestow Technocracy India Pvt. Ltd. & M&M Info-Tech UK Ltd.
* This notice may not be removed from any source distribution.
* See license.txt for detailed licensing details. */

using MnM.GWS;

using System;
using System.Runtime.InteropServices;
using static MnM.GWS.Implementation;

namespace MnM
{
    internal interface ISdlSurface : IHandle { }
  
    namespace GWS
    {
        [StructLayout(LayoutKind.Sequential)]
        unsafe struct SurfaceInfo
        {
            #region VARIABLES
            public uint Flags;
            public IntPtr Format; //PixelFormat
            public int Width;
            public int Height;
            public int Pitch;
            public IntPtr Pixels;
            public IntPtr UserData;
            public int Locked;
            public IntPtr LockData;
            public Rect ClipRect;
            public IntPtr Map;
            public int Refcount;
            #endregion
        }

        struct SysWMInfo
        {
            public Version Version;
            public SysWMType Subsystem;
            public SysInfo Info;

            [StructLayout(LayoutKind.Explicit)]
            public struct SysInfo
            {
                [FieldOffset(0)]
                public WindowsInfo Windows;
                [FieldOffset(0)]
                public X11Info X11;
                [FieldOffset(0)]
                public WaylandInfo Wayland;
                [FieldOffset(0)]
                public DirectFBInfo DirectFB;
                [FieldOffset(0)]
                public CocoaInfo Cocoa;
                [FieldOffset(0)]
                public UIKitInfo UIKit;

                public struct WindowsInfo
                {
                    public IntPtr Window;
                }

                public struct X11Info
                {
                    public IntPtr Display;
                    public IntPtr Window;
                }

                public struct WaylandInfo
                {
                    public IntPtr Display;
                    public IntPtr Surface;
                    public IntPtr ShellSurface;
                }

                public struct DirectFBInfo
                {
                    public IntPtr Dfb;
                    public IntPtr Window;
                    public IntPtr Surface;
                }

                public struct CocoaInfo
                {
                    public IntPtr Window;
                }

                public struct UIKitInfo
                {
                    public IntPtr Window;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct RendererInfo
        {
            public IntPtr name; // const char*
            public uint flags;
            public uint num_texture_formats;
            public fixed uint texture_formats[16];
            public int max_texture_width;
            public int max_texture_height;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MessageBoxButtonData
        {
            public MessageBoxButtonFlags flags;
            public int buttonid;
            public string text; /* The UTF-8 button text */

        }

        [StructLayout(LayoutKind.Sequential)]
        struct MessageBoxColor
        {
            public byte r, g, b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MessageBoxColorScheme
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = (int)MessageBoxColorType.OfColorMax)]
            public MessageBoxColor[] colors;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MessageBoxData
        {
            public MessageBoxFlags flags;
            public IntPtr window;               /* Parent window, can be NULL */
            public string title;                /* UTF-8 title */
            public string message;              /* UTF-8 message text */
            public int numbuttons;
            public MessageBoxButtonData[] buttons;
            public MessageBoxColorScheme? colorScheme;  /* Can be NULL to use system settings */
        }

        [StructLayout(LayoutKind.Sequential)]
        struct InternalMessageBoxButtonData
        {
            public MessageBoxButtonFlags flags;
            public int buttonid;
            public IntPtr text; /* The UTF-8 button text */
        }

        [StructLayout(LayoutKind.Sequential)]
        struct InternalMessageBoxData
        {
            public MessageBoxFlags flags;
            public IntPtr window;               /* Parent window, can be NULL */
            public IntPtr title;                /* UTF-8 title */
            public IntPtr message;              /* UTF-8 message text */
            public int numbuttons;
            public IntPtr buttons;
            public IntPtr colorScheme;          /* Can be NULL to use system settings */
        }

        struct Version
        {
            public byte Major;
            public byte Minor;
            public byte Patch;
            public int Number
            {
                get { return 1000 * Major + 100 * Minor + Patch; }
            }
        }

        struct DisplayMode
        {
            public uint Format;
            public int Width;
            public int Height;
            public int RefreshRate;
            public IntPtr DriverData;
        }
    }
}

