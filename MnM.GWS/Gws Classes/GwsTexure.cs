using System;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if Window
    public abstract class GwsTexture : GwsUploadable, ITexture
    {
        #region CONSTRUCTORS
        protected GwsTexture() { }
        #endregion

        #region PROPERTIES
        public bool IsPrimary { get; private set; }
        public int Length => Width * Height;
        public IRenderWindow Window { get; private set; }
        public IntPtr Handle { get; protected set; }
        public abstract BlendMode Mode { get; set; }
        public abstract byte Alpha { get; set; }
        public abstract int ColorMode { get; set; }
        public Angle Angle { get; set; }
        public RendererFlip Flip { get; set; }
        public bool IsDisposed { get; private set; }
        #endregion

        #region INITIALIZE
        protected void Initialize(IRenderWindow window, bool isPrimary)
        {
            Window = window;
            IsPrimary = isPrimary;
        }
        protected abstract void Initialize(int width, int height);
        #endregion

        #region BIND - UNBIND
        public abstract void Bind();
        public abstract void Unbind();
        #endregion

        #region COPY TO
        public abstract unsafe IRectangle CopyTo(IRectangle copyRc, IntPtr dest, int destLen, int destW, int destX, int destY);
        #endregion

        #region TO SURFACE
        public abstract IGraphics ToCanvas(int? x, int? y, int? width = null, int? height = null);
        #endregion

        public virtual void Dispose()
        {
            IsDisposed = true;
            Window = null;
        }
        public abstract void Resize(int? width = null, int? height = null);
    }
#endif
}
