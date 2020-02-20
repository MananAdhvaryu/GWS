using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    public abstract partial class GwsRenderWindow: IRenderWindow
    {
        protected GwsRenderWindow()
        {
            Popups = Implementation.Factory.newPopupCollection(this);
            Buffer = newBuffer();
            Controls = Implementation.Factory.newElementCollection(this);
            BackgroundChanged += Buffer_BackgroundChanged;
        }

        #region PROPERTIES
        public bool SuspendLayout { get; set; }
        public IntPtr Handle { get; protected set; }
        public RendererFlags RendererFlags { get; protected set; }
        public IPopupCollection Popups { get; private set; }
        public bool IsWindow => true;
        public string ID { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public virtual IEventUser ActiveObject { get; set; }
        public IElementCollection Controls { get; private set; }
        protected IBuffer Buffer { get; private set; }
        public bool HasTexture => Texture != null;
        protected IUploadable Texture { get; set; }
        #endregion

        #region NEW BUFFER
        protected abstract IBuffer newBuffer();
        #endregion

        #region UPDATE
        public void Submit()
        {
            if (Buffer == null || SuspendLayout)
                return;

            if (Texture != null)
                Texture.Upload();
            else if (Buffer is IUpdatable)
                (Buffer as IUpdatable).Submit();
        }
        public void Submit(IRectangle rectangle)
        {
            if (Buffer == null || SuspendLayout)
                return;

            if (Buffer is IUpdatable)
                (Buffer as IUpdatable).Submit(rectangle);
            else if (Texture != null)
                Texture.Upload(rectangle);
        }
        #endregion

        #region DISCARD
        public void Discard()
        {
            if (Texture != null)
                Texture.Download();
           
            else if(Buffer is IUpdatable)
                (Buffer as IUpdatable).Discard();
        }
        public void Discard(IRectangle rectangle)
        {
            if (Texture != null)
                Texture.Download(rectangle);

            else if (Buffer is IUpdatable)
                (Buffer as IUpdatable).Discard(rectangle);
        }
        #endregion

        #region INVALIDATE
        public void Invalidate(IRectangle rectangle) =>
            Buffer.PendingUpdates?.Invalidate(rectangle);
        public void Invalidate(IEnumerable<IRectangle> rectangles) =>
            Buffer.PendingUpdates?.Invalidate(rectangles);
        #endregion

        void IStoreable.AssignIDIfNone() { }
    }

    partial class GwsRenderWindow
    {
#if AdvancedVersion
        public IBufferCollection Buffers => Buffer.Buffers;

#endif
        public IUpdateManager PendingUpdates => 
            Buffer.PendingUpdates;
        public string BackgroundPen => Buffer.BackgroundPen;

        int IBuffer.this[int index] 
        { 
            get => Buffer[index]; 
            set => Buffer[index] = value;
        }
        string IBuffer.ID => Buffer.ID;
        IntPtr IBufferData.Pixels => Buffer.Pixels;
        int IBufferSize.Length => Buffer.Length;
        int IBuffer.IndexOf(int val, int axis, bool horizontal) =>
            Buffer.IndexOf(val, axis, horizontal);
        void IBuffer.XYOf(int index, out int x, out int y) =>
            Buffer.XYOf(index, out x, out y);

        public IRectangle CopyTo(IRectangle copyRc, IntPtr dest, int destLen, int destW, int destX, int destY)
        {
            return Buffer.CopyTo(copyRc, dest, destLen, destW, destX, destY);
        }
        public virtual void Dispose()
        {
            Buffer.Dispose();
        }
        public void SaveAs(string file, ImageFormat format, IRectangle portion = null, int quality = 50)
        {
            Buffer.SaveAs(file, format, portion, quality);
        }
        public virtual void Resize(int? width =null, int? height = null)
        {
            Buffer.Resize(width, height);
        }
        public event EventHandler<IEventArgs> BackgroundChanged
        {
            add => Buffer.BackgroundChanged += value;
            remove => Buffer.BackgroundChanged -= value;
        }

        void Buffer_BackgroundChanged(object sender, IEventArgs e)
        {
            Controls.RefreshAll();
        }

        public void ApplyBackground(IPenContext context) =>
            Buffer.ApplyBackground(context);

        public bool IsBackgroundPixel(int val, int axis, bool horizontal) =>
            Buffer.IsBackgroundPixel(val, axis, horizontal);

        public void ClearBackground(int x, int y, int w, int h) =>
            Buffer.ClearBackground(x, y, w, h);

        public void ClearBackground(IRectangle area) =>
            Buffer.ClearBackground(area);

        public void ClearBackground() =>
            Buffer.ClearBackground();
    }
}
