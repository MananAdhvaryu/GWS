
using System;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
        unsafe sealed class Graphics : GwsGraphics, IGraphics, IBufferPen, IResizeable
        {
            #region VARIABLES
            #endregion

            #region CONSTRUCTORS
            Graphics() { }
            public Graphics( int width,  int height, byte[] buffer,  bool copy = false) : this() =>
                Initialize(width, height, buffer, copy);
            public Graphics( int width,  int height, int[] buffer,  bool copy = false) : this() =>
                Initialize(width, height, buffer, copy);
            public Graphics( int width,  int height, IntPtr buffer,  int bufferLength) : this() =>
                Initialize(width, height, buffer, bufferLength);
            #endregion

            #region INITIALIZE
            void Initialize( int width,  int height, byte[] buffer,  bool copy = false)
            {
                if (buffer == null)
                {
                    Initialize(width, height, default(int[]), copy);
                    return;
                }
                fixed (byte* b = buffer)
                {
                    var d = new IntPtr(b);
                    Initialize(width, height, d, buffer.Length / 4);
                }
            }
            unsafe void  Initialize( int width,  int height,  int[] buffer,  bool copy = false)
            {
                int[] data = null;

                if (buffer == null || copy)
                    data = new int[width * height];

                if (buffer != null)
                {
                    if (copy)
                    {
                        var len = Math.Min(data.Length, buffer.Length);
                        Array.Copy(buffer, 0, data, 0, len);
                    }
                    else
                        data = buffer;
                }
                fixed (int* p = data)
                    PrimaryBuffer = Factory.newBuffer(p, width, height);
            }
            unsafe void Initialize( int width,  int height, IntPtr buffer,  int bufferLength)
            {
                if (buffer == IntPtr.Zero)
                    PrimaryBuffer = Factory.newBuffer(width, height);
                else
                    PrimaryBuffer = Factory.newBuffer((int*)buffer, width, height);
            }
            protected override void Initialize( int width,  int height) =>
                PrimaryBuffer = Factory.newBuffer(width, height);
            #endregion

            #region PROPERTIES
            #endregion

            #region NEW INSTANCE
            protected override GwsGraphics newInstance() => 
                new Graphics();
            protected override GwsGraphics Clone(int? width = null, int? height = null)
            {
                var canvas = base.Clone(width, height) as Graphics;
                var rc = this.CompitibleRC(0, 0, width?? Width, height?? PrimaryBuffer.Height);
                PrimaryBuffer.CopyMemoryTo(canvas.PrimaryBuffer, rc, 0, 0);
                return canvas;
            }
            #endregion

            #region RESIZE
            protected sealed override void ResetDataInternally(int width, int height)
            {
                if (PrimaryBuffer is IResizeable)
                    (PrimaryBuffer as IResizeable).Resize(width, height);
                else
                    PrimaryBuffer = PrimaryBuffer.ResizedData(width, height);
            }
            protected override void GetOutputSize(out int rw, out int rh)
            {
                rw = PrimaryBuffer.Width;
                rh = PrimaryBuffer.Height;
            }
            #endregion

            #region DISPOSE
            protected override void OnDispose()
            {
                PrimaryBuffer = null;
            }
            #endregion

            #region OLD Drawmode implementation
            //public unsafe virtual bool SetPixel(int src, byte? externalAlpha, int destIndex, CopyMode? mode = null)
            //{
            //    var m = mode ?? CopyMode.Opaque;
            //    fixed (int* dest = data)
            //    {
            //        var destLen = data.Length;

            //        if (destIndex < 0 || destIndex > destLen - 4)
            //            return false;

            //        if (!Instance.IsPixelOkToWrite(destIndex))
            //            return false;

            //        src = src.Change(externalAlpha);
            //        var srcAlpha = src.Alpha();

            //        if (m.HasFlag(CopyMode.Opaque))
            //            return Instance.Opaque(dest, destIndex, destLen, src);

            //        if (srcAlpha == 0)
            //            return false;

            //        var destAlpha = dest[destIndex].Alpha();

            //        if (m.HasFlag(CopyMode.Default) || m.HasFlag(CopyMode.Front))
            //        {
            //            if (srcAlpha == 255 || destAlpha == 0)
            //                return Instance.Opaque(dest, destIndex, destLen, src);
            //            return Instance.Blend(dest, destIndex, destLen, src);
            //        }
            //        else if (m.HasFlag(CopyMode.Back))
            //        {
            //            if (destAlpha == 0)
            //                return Instance.Opaque(dest, destIndex, destLen, src);
            //            if (destAlpha != 255)
            //                return Instance.Blend(dest, destIndex, destLen, src);
            //            return false;
            //        }
            //        else if (m.HasFlag(CopyMode.IntersectExclude))
            //        {
            //            return Instance.Opaque(dest, destIndex, destLen, src);
            //        }
            //        else if (m.HasFlag(CopyMode.FloodFill))
            //        {
            //            if (destAlpha != 255)
            //                return false;
            //            return Instance.Opaque(dest, destIndex, destLen, src);
            //        }
            //        else if (m.HasFlag(CopyMode.Mix))
            //        {
            //            if (destAlpha == 255 && srcAlpha == 255)
            //            {
            //                Instance.Mix(dest, destIndex, destLen, src);
            //                return true;
            //            }
            //            if (srcAlpha == 255 || destAlpha == 0)
            //                return Instance.Opaque(dest, destIndex, destLen, src);
            //            return Instance.Blend(dest, destIndex, destLen, src);
            //        }
            //        else if (m.HasFlag(CopyMode.Mask))
            //        {
            //            if (destAlpha != 0)
            //            {
            //                if (srcAlpha == 255)
            //                    return Instance.Opaque(dest, destIndex, destLen, src);
            //                if (srcAlpha == 255 || destAlpha == 0)
            //                    return Instance.Opaque(dest, destIndex, destLen, src);
            //                return Instance.Blend(dest, destIndex, destLen, src);
            //            }
            //            return false;
            //        }
            //        else
            //            return Instance.Blend(dest, destIndex, destLen, src);
            //    }
            //}
            #endregion
        }
#if AllHidden
    }
#endif
}
