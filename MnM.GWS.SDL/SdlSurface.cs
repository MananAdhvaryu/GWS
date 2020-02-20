
using System;

namespace MnM.GWS
{
#if AllHidden
    partial class SdlFactory
    {
#else
    public
#endif
        sealed unsafe class SdlSurface : GwsGraphics, ISurface, ISdlSurface
        {
            #region CONSTRUCTORS
            internal SdlSurface() { }
            #endregion

            #region PROPERTIES
            public IntPtr Handle { get; private set; }
            #endregion

            #region RESIZE
            protected sealed override void ResetDataInternally(int width, int height)
            {
                var newBuffer = new SdlBuffer(width, height);
                var srcRc = this.CompitibleRC(null, null, width, height);
                PrimaryBuffer.CopyMemoryTo(newBuffer, srcRc, 0, 0);
                PrimaryBuffer.Dispose();
                PrimaryBuffer = newBuffer;
                Handle = newBuffer.Handle;
                ID = "Surface" + Handle.ToString();
            }
            #endregion

            #region UPDATE CLEAR
            protected override void GetOutputSize(out int rw, out int rh)
            {
                rw = PrimaryBuffer.Width;
                rh = PrimaryBuffer.Height;
            }
            #endregion

            #region NEW INSTANCE
            protected override GwsGraphics newInstance() =>
                new SdlSurface();
            #endregion
         
            #region INITIALIZE
            internal void Initialize(int width, int height, int[] buffer, bool copy = false)
            {
                if (buffer == null)
                {
                    Initialize(width, height);
                    return;
                }
                fixed (int* p = buffer)
                {
                    Initialize(width, height, new IntPtr(p), 0);
                }
            }
            internal void Initialize(int width, int height, byte[] buffer, bool copy = false)
            {
                if (buffer == null)
                {
                    Initialize(width, height);
                    return;
                }
                fixed (byte* p = buffer)
                {
                    Initialize(width, height, new IntPtr(p), 0);
                }
            }
            internal void Initialize(int width, int height, IntPtr buffer, int bufferLength)
            {
                SdlBuffer sdlBuffer = new SdlBuffer((int*)buffer, width, height);
                Handle = sdlBuffer.Handle;
                PrimaryBuffer = sdlBuffer;
                ID = "Surface" + Handle.ToString();
            }

            protected override void Initialize( int width,  int height)
            {
                SdlBuffer sdlBuffer = new SdlBuffer(width, height);
                Handle = sdlBuffer.Handle;
                PrimaryBuffer = sdlBuffer;
                ID = "Surface" + Handle.ToString();
            }
            internal void Initialize(int width, int height, IntPtr buffer) =>
                Initialize(width, height, buffer, 0);
            #endregion

            #region DISPOSE
            protected override void OnDispose()
            {
                SdlFactory.FreeSurface(Handle);
            }
            #endregion
        }
#if AllHidden
    }
#endif
}
