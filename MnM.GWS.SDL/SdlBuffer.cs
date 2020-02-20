using System;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public sealed class SdlBuffer: GwsBuffer, IBuffer, IHandle, ISdlSurface, IResizeable
    {
        SurfaceInfo surface;
        IUpdateManager pendingUpdates;

        #region CONSTRUCTORS
        public SdlBuffer()
        {
            pendingUpdates = Factory.newUpdateManager();
        }
        public unsafe SdlBuffer(int w, int h) : this()
        {
            Handle = SdlFactory.CreateSurface(w, h);
            surface = Handle.ToStruct<SurfaceInfo>();
        }
        public unsafe SdlBuffer(int[] data, int w, int h, bool makeCopy = false) : this()
        {
            fixed (int* p = data)
            {
                if (makeCopy)
                    Handle = SdlFactory.CreateSurface(w, h);
                else
                    Handle = SdlFactory.CreateSurface((IntPtr)p, w, h, Factory.PixelFormat);

                surface = Handle .ToStruct<SurfaceInfo>();

                if (makeCopy)
                    Implementation.CopyMemory(p, 0, (int*)surface.Pixels, 0, w * h);
            }
        }
        public unsafe SdlBuffer(int* data, int w, int h, bool makeCopy = false) : this()
        {
            if (makeCopy)
                Handle = SdlFactory.CreateSurface(w, h);
            else
                Handle = SdlFactory.CreateSurface((IntPtr)data, w, h, Factory.PixelFormat);
 

            surface = Handle.ToStruct<SurfaceInfo>();

            if (makeCopy)
                Implementation.CopyMemory(data, 0, (int*)surface.Pixels, 0, w * h);
        }
        internal SdlBuffer(SurfaceInfo surface): this()
        {
            this.surface = surface;
            Handle = surface.ToPtr();
        }
        #endregion

        #region PROPERTIES
        public override IntPtr Pixels => surface.Pixels;
        public override int Width => surface.Width;
        public override int Height => surface.Height;
        public override unsafe int this[int index]
        {
            get => ((int*)surface.Pixels)[index];
            set => ((int*)surface.Pixels)[index] = value;
        }
        public IntPtr Handle { get; private set; }
        public override IUpdateManager PendingUpdates => pendingUpdates;
        #endregion

        #region RESIZE
        protected override void ResetDataInternally(int width, int height)
        {
            var newHandle = SdlFactory.CreateSurface((int)width, (int)height);
            var newSurface = newHandle.ToStruct<SurfaceInfo>();
            var srcRc = (this).CompitibleRC(null, null, (int?)width, (int?)height);
            var srcLen = surface.Width * surface.Height;
            var destLen = newSurface.Width * newSurface.Height;
            Implementation.CopyMemoryTo(surface.Pixels, newSurface.Pixels, srcRc, srcLen, surface.Width, 0, 0, newSurface.Width, destLen);
            SdlFactory.FreeSurface(Handle);
            Handle = newHandle;
            surface = newSurface;
        }
        #endregion

        #region DISPOSE
        public override void Dispose()
        {
            SdlFactory.FreeSurface(Handle);
        }
        #endregion
    }
}
