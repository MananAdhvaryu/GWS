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

        unsafe class Buffer : GwsBuffer, IBuffer
        {
            int[] pixels;
            int width, height;
            IUpdateManager pendingUpdates;

            #region CONSTRUCTORS
            Buffer(int w, int h, bool initPixels)
            {
                width = w;
                height = h;
                if(initPixels)
                    pixels = new int[Length];
                pendingUpdates = Factory.newUpdateManager();
            }
            public unsafe Buffer(int w, int h) :
                this(w, h, true)
            { }
            public unsafe Buffer(int[] data, int w, int h, bool makeCopy = false) : 
                this(w, h, false)
            {
                if(makeCopy)
                {
                    pixels = new int[w * h];
                    Array.Copy(data, 0, pixels, 0, pixels.Length);
                }
                else
                    pixels = data;
            }
            public unsafe Buffer(int* data, int w, int h, bool makeCopy = false) :
                this(w, h, false)
            {
                pixels = new int[Length];
                CopyMemory(data, 0, pixels, 0, Length);
            }
            #endregion

            #region PROPERTIES
            public override IntPtr Pixels
            {
                get
                {
                    fixed (int* p = pixels)
                        return (IntPtr)(p);
                }
            }
            public override int Width => width;
            public override int Height => height;
            public override int this[int index]
            {
                get => pixels[index];
                set => pixels[index] = value;
            }
            public override IUpdateManager PendingUpdates => pendingUpdates;
            #endregion

            protected override void ResetDataInternally(int width, int height)
            {
                pixels = Geometry.ResizedData(pixels, width, height, ref width, ref height);
            }

            #region DISPOSE
            public override void Dispose() { }
            #endregion
        }
#if AllHidden
    }
#endif
}
