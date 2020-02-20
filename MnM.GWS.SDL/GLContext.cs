/* Licensed under the MIT/X11 license.
* Copyright (c) 2016-2018 jointly owned by eBestow Technocracy India Pvt. Ltd. & M&M Info-Tech UK Ltd.
* This notice may not be removed from any source distribution.
* See license.txt for detailed licensing details. */

using MnM.GWS;
using System;
using System.Runtime.InteropServices;

namespace MnM
{
    namespace GWS
    {
        public enum ContextAttribute
        {
            RED_SIZE,
            GREEN_SIZE,
            BLUE_SIZE,
            ALPHA_SIZE,
            BUFFER_SIZE,
            DOUBLEBUFFER,
            DEPTH_SIZE,
            STENCIL_SIZE,
            ACCUM_RED_SIZE,
            ACCUM_GREEN_SIZE,
            ACCUM_BLUE_SIZE,
            ACCUM_ALPHA_SIZE,
            STEREO,
            MULTISAMPLEBUFFERS,
            MULTISAMPLESAMPLES,
            ACCELERATED_VISUAL,
            RETAINED_BACKING,
            CONTEXT_MAJOR_VERSION,
            CONTEXT_MINOR_VERSION,
            CONTEXT_EGL,
            CONTEXT_FLAGS,
            CONTEXT_PROFILE_MASK,
            SHARE_WITH_CURRENT_CONTEXT
        }

#if false
        public sealed class GLContext : MnMObj
        {
#region variables
            IntPtr window;
            static IntPtr current;
            IntPtr? texture;
#endregion

#region constructors
            GLContext() { }
            internal static GLContext Create(IWindow window)
            {
                var gl = new GLContext();
                gl.window = window.Handle;
                gl.handle = Sdl.CreateGLContext(window.Handle);
                return gl;
            }
#endregion

            public int SwapInterval
            {
                get
                {
                    MakeCurrent();
                    return GetGLSwapInterval();
                }
                set
                {
                    MakeCurrent();
                    SetGLSwapInterval(value);
                }
            }
            public bool IsCurrent =>
                current == Handle;
            public int this[ContextAttribute attribute]
            {
                get
                {
                    MakeCurrent();
                    return GetGLAttribute(attribute);
                }
                set
                {
                    MakeCurrent();
                    SetGLAttribute(attribute, value);
                }
            }

            //public static int GetAttribute(ContextAttribute attr)
            //{
            //    getAttribute(attr, out int value);
            //    return value;
            //}
            //public static void SetAttribute(ContextAttribute attr, int value) =>
            //     setAttribute(attr, value);
            public void Reset() =>
                ResetGLAttributes();
            //public static void ResetAttributes() =>
            //    resetAttributes();

            public static void GetCurrent() =>
                GetCurrentGLContext();
            public static IntPtr GetFunction(string fxName) =>
                GetGLFunction(fxName);

            public unsafe void BindTexture(IntPtr texture, float? width = null, float? height = null)
            {
                float w, h;

                w = width ?? texture.Width;
                h = height ?? texture.Height;

                BindGLTexture(texture, &w, &h);
                this.texture = texture;
            }
            public void UnbindTexture()
            {
                if (texture != null)
                {
                    UnbindGLTexture(texture.Value);
                    texture = null;
                }
            }
            public void MakeCurrent()
            {
                if (!IsCurrent)
                    MakeGLCurrent(window, Handle);
                current = Handle;
            }
            public void Update()
            {
                MakeCurrent();
                SwapGLWindow(window);
            }
            protected sealed override void OnDispose() =>
                DestroyGLContext(Handle);

#region enums
            [Flags]
            internal enum ContextFlags
            {
                DEBUG = 0x0001,
                FORWARD_COMPATIBLE = 0x0002,
                ROBUST_ACCESS = 0x0004,
                RESET_ISOLATION = 0x0008
            }
            [Flags]
            internal enum ContextProfileFlags
            {
                CORE = 0x0001,
                COMPATIBILITY = 0x0002,
                ES = 0x0004
            }
#endregion

#region sdl - GL mapping
            const string libSDL = Sdl.libSDL;

            [DllImport(libSDL, EntryPoint = "SDL_GL_CreateContext", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern IntPtr CreateGLContext(IntPtr window);

            [DllImport(libSDL, EntryPoint = "SDL_GL_DeleteContext", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern void DestroyGLContext(IntPtr context);

            [DllImport(libSDL, EntryPoint = "SDL_GL_GetAttribute", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern int GetGLAttribute(ContextAttribute attr, out int value);

            public static int GetGLAttribute(ContextAttribute attr)
            {
                GetGLAttribute(attr, out int value);
                return value;
            }


            [DllImport(libSDL, EntryPoint = "SDL_GL_GetCurrentContext", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern IntPtr GetCurrentGLContext();

            [DllImport(libSDL, EntryPoint = "SDL_GL_GetDrawableSize", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern void GetGLDrawableSize(IntPtr IntPtr, out int w, out int h);

            [DllImport(libSDL, EntryPoint = "SDL_GL_GetProcAddress", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern IntPtr GetGLProcAddress(IntPtr proc);
            public static IntPtr GetGLFunction(string proc)
            {
                IntPtr p = Marshal.StringToHGlobalAnsi(proc);
                try
                {
                    return GetGLProcAddress(p);
                }
                finally
                {
                    Marshal.FreeHGlobal(p);
                }
            }

            [DllImport(libSDL, EntryPoint = "SDL_GL_GetSwapInterval", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern int GetGLSwapInterval();

            [DllImport(libSDL, EntryPoint = "SDL_GL_MakeCurrent", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern int MakeGLCurrent(IntPtr window, IntPtr context);

            [DllImport(libSDL, EntryPoint = "SDL_GL_SetAttribute", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern int SetGLAttribute(ContextAttribute attr, int value);

            [DllImport(libSDL, EntryPoint = "SDL_GL_SetSwapInterval", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern int SetGLSwapInterval(int interval);

            [DllImport(libSDL, EntryPoint = "SDL_GL_SwapWindow", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern void SwapGLWindow(IntPtr window);

            [DllImport(libSDL, EntryPoint = "SDL_GL_ResetAttributes", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static extern void ResetGLAttributes();

            [DllImport(libSDL, EntryPoint = "SDL_GL_BindTexture", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static unsafe extern int BindGLTexture(IntPtr sdlTexture, float* texw, float* texh);

            [DllImport(libSDL, EntryPoint = "SDL_GL_UnbindTexture", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            public static unsafe extern int UnbindGLTexture(IntPtr sdlTexture);
#endregion
        }
#endif
    }
}
