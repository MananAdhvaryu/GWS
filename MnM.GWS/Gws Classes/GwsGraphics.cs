using System;
using System.Runtime.CompilerServices;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public abstract partial class GwsGraphics : GwsBuffer, IGraphics
    {
        #region VARIABLES
        IBuffer primaryBuffer;
        #endregion

        #region CONSTRUCTORS
        protected GwsGraphics()
        {
            ID = Factory.NewID(this);
        }
        #endregion

        #region PROPERTIES
        public int? ColorKey { get; set; }
        public override int Width => PrimaryBuffer.Width;
        public override int Height => PrimaryBuffer.Height;
        public bool IsDisposed { get; private set; }
        public override IntPtr Pixels
        {
            get
            {
#if AdvancedVersion
                if(BufferIndex != -1)
                    return Buffers.Current.Pixels;
#endif
                return PrimaryBuffer.Pixels;
            }
        }
        public override IUpdateManager PendingUpdates
        {
            get
            {
#if AdvancedVersion
                if (BufferIndex != -1)
                    return Buffers.Current.PendingUpdates;
#endif
                return primaryBuffer.PendingUpdates;
            }
        }
        protected IBuffer PrimaryBuffer
        {
            get => primaryBuffer;
            set
            {
                primaryBuffer = value;
#if AdvancedVersion
                Buffers.ChangePrimary(primaryBuffer);
#endif
            }
        }
        public override int this[int index]
        {
            get
            {
#if AdvancedVersion
                if(BufferIndex != -1)
                    return Buffers.Current[index];
#endif
                return PrimaryBuffer[index];
            }
            set
            {
#if AdvancedVersion
                if(BufferIndex != -1)
                    Buffers.Current[index] = value;
#endif
                PrimaryBuffer[index] = value;
            }
        }
#if AdvancedVersion
        protected int BufferIndex => Buffers.CurrentIndex;
#endif
        #endregion

        #region INITIALIZE
        protected abstract void Initialize(int width, int height);
        #endregion

        #region NEW INSTANCE
        protected abstract GwsGraphics newInstance();
        protected virtual GwsGraphics Clone(int? width = null, int? height = null)
        {
            var pixels = newInstance();
            pixels.Initialize(width ?? Width, height ?? Height);
            return pixels;
        }
        #endregion


        #region INDEX OF
        #endregion

        #region CORRECT UPLOAD REGIONS
        protected bool CorrectRegions(IRectangle sourceRc, IRectangle destRc, out IRectangle srcRc, out IRectangle dstRc)
        {
            if (IsDisposed)
            {
                srcRc = dstRc = null;
                return false;
            }
            var sw = sourceRc?.Width ?? Width;
            var sh = sourceRc?.Height ?? Height;

            sw = Math.Min(sw, Width);
            sh = Math.Min(sh, Height);

            GetOutputSize(out int rw, out int rh);

            var dw = destRc?.Width ?? Width;
            var dh = destRc?.Height ?? Height;

            dw = Math.Min(dw, Width);
            dh = Math.Min(dh, Height);

            srcRc = Geometry.CompitibleRC(Width, Height, sourceRc?.X, sourceRc?.Y, sw, sh);
            dstRc = Geometry.CompitibleRC(rw, rh, destRc?.X, destRc?.Y, dw, dh);

            if (destRc.Width == 0 || destRc.Height == 0 || srcRc.Width == 0 || srcRc.Height == 0)
                return false;
            return true;
        }
        protected abstract void GetOutputSize(out int rw, out int rh);
        #endregion

        #region TO PEN
        public IBufferPen ToPen(int? width = null, int? height = null)
        {
            if (!NewPenRequired(width, height, out int w, out int h))
                return this;
            var reader = Clone(w, h);
            return reader;
        }
        protected bool NewPenRequired(int? width, int? height, out int w, out int h)
        {
            w = width ?? 0;
            h = height ?? 0;
            bool ok = true;

            if (w == 0 || h == 0 || (w == Width && h == Height))
                ok = false;
            if (w == 0)
                w = Width;
            if (h == 0)
                h = Height;
            return ok;
        }
        #endregion

        #region DISPOSE
        public override void Dispose()
        {
            IsDisposed = true;
            Factory.Remove(this, ObjType.Buffer);
            OnDispose();
            Disposed?.Invoke(this, Factory.EventArgsEmpty);
        }
        protected abstract void OnDispose();
        public event EventHandler<IEventArgs> Disposed;
        #endregion

        #region EVENTS
        protected virtual void OnWindowChangeAttmepted(CancelEventArgs<IRenderWindow> e) =>
            AllowWindowChange?.Invoke(this, e);
        public event EventHandler<CancelEventArgs<IRenderWindow>> AllowWindowChange;
        #endregion

        void IStoreable.AssignIDIfNone() { }
        int IBufferPen.IndexOf(int val, int axis, bool horizontal) =>
            IndexOf(Entity.BufferPen, val, axis, horizontal);
        int IBuffer.IndexOf(int val, int axis, bool horizontal) =>
            IndexOf(Entity.Buffer, val, axis, horizontal);
        int IGraphics.IndexOf(int val, int axis, bool horizontal) =>
            IndexOf(Entity.Buffer, val, axis, horizontal);
    }
}
