using System;
using System.Runtime.CompilerServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public abstract class GwsBuffer: IBuffer
    {
        protected GwsBuffer()
        {
            ID = Factory.NewID(this);
#if AdvancedVersion
            Buffers = Factory.newBufferCollection();
#endif
        }

        #region PROPERTIES
        public abstract int this[int index] { get; set; }
        public abstract IntPtr Pixels { get; }
        public int Length => Width * Height;
        public abstract int Width { get; }
        public abstract int Height { get; }
        public string ID { get; protected set; }
        public string BackgroundPen { get; private set; }

        public abstract IUpdateManager PendingUpdates { get; }
#if AdvancedVersion
        public IBufferCollection Buffers { get; private set; }
#endif
        #endregion

        #region METHODS
        public unsafe IRectangle CopyTo(IRectangle copyRc, IntPtr dest, int destLen, int destW, int destX, int destY)
        {
            var rc = Factory.newRectangle(0, 0, copyRc.Width, copyRc.Height);
            int length = Length;

            var src = (int*)Pixels;
            var dst = (int*)dest;
            var result = CopyBlock(length, copyRc, Width, destX, destY, destW, destLen, (si, di, w, i) => CopyMemory(src, si, (int*)dst, di, w));
            return result;
        }
        public void XYOf(int index, out int x, out int y)
        {
            y = index / Width;
            x = index - y * Width;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(int val, int axis, bool horizontal) =>
            IndexOf(Entity.Buffer, val, axis, horizontal);

        protected int IndexOf(Entity entity, int val, int axis, bool horizontal)
        {
            int i;

            int x = horizontal ? val : axis;
            int y = horizontal ? axis : val;
            Renderer.CorrectXY(entity, ref x, ref y);

            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return -1;

            i = x + y * Width;
            if (i >= Length || i < 0)
                return -1;
            return i;
        }
        #endregion

        #region APPLY BACKGROUND
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ApplyBackground(IPenContext context)
        {
            if (context == null)
                return;

            IBufferPen Pen = (context is IBufferPen) ? context as IBufferPen : context.ToPen(Width, Height);
            if (Pen == null || Pen.ID == BackgroundPen)
                return;

            var rc = Factory.newRectangle(0, 0, Pen.Width, Pen.Height);
            Pen.CopyTo(rc, Pixels, Length, Width, 0, 0);
            BackgroundPen = Pen.ID;
            Factory.Add(Pen, ObjType.Buffer);
            PendingUpdates.Clear();
            BackgroundChanged?.Invoke(this, Factory.EventArgsEmpty);
            //RefreshAll();
            PendingUpdates.Clear();
            PendingUpdates.Invalidate(rc);
        }
        public unsafe bool IsBackgroundPixel(int val, int axis, bool horizontal)
        {
            var i = IndexOf(val, axis, horizontal);
            if (i == -1)
                return true;
            var x = horizontal ? val : axis;
            var y = horizontal ? axis : val;

            bool yes = false;

            int* src = (int*)Pixels;
            if (BackgroundPen != null)
            {
                IBufferPen pen = Factory.Get<IBufferPen>(BackgroundPen, ObjType.Buffer);
                if (pen == null)
                    return false;
                yes = src[i] == pen.ReadPixel(val, axis, horizontal);
            }
            else
                yes = src[i] == 0;
            return yes;
        }
        #endregion

        #region CLEAR
        public void ClearBackground(int x, int y, int w, int h)
        {
            ClearBackground(this.CompitibleRC(x, y, w, h));
        }
        public void ClearBackground() =>
            ClearBackground(null);
        public unsafe void ClearBackground(IRectangle target)
        {
            IBufferPen pen = null;

            var rc = this.CompitibleRC(target?.X, target?.Y, target?.Width, target?.Height);

            if (!string.IsNullOrEmpty(BackgroundPen))
            {
                Factory.Get(BackgroundPen, out pen, ObjType.Buffer);
                if (pen is IPenContext && (pen.Width != Width || pen.Height != Height))
                {
                    Factory.Remove(pen.ID, ObjType.Buffer);
                    pen = (pen as IPenContext).ToPen(Width, Height);
                    BackgroundPen = pen.ID;
                    Factory.Add(pen, ObjType.Buffer);
                }
            }
            if (pen != null)
            {
                pen.CopyTo(rc, Pixels, Length, Width, rc.X, rc.Y);
                goto mks;
            }

            int[] array = new int[rc.Width * rc.Height];
            fixed (int* p = array)
            {
                CopyMemory(p, 0, (int*)Pixels, IndexOf(rc.X, rc.Y, true), array.Length);
            }
        mks:
            PendingUpdates.Invalidate(target);
        }
        #endregion

        #region RESIZE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int? width = null, int? height = null)
        {
            if (width == null && height == null)
                return;

            var w = width ?? this.Width;
            var h = height ?? this.Height;

            ResetDataInternally(w, h);
#if AdvancedVersion
            Buffers?.ResizeAll();
#endif
            PendingUpdates.Clear();
            PendingUpdates.Invalidate(Factory.newRectangle(0, 0, Width, Height));
        }
        protected abstract void ResetDataInternally(int width, int height);
        #endregion

        #region SAVE AS
        public unsafe void SaveAs(string file, ImageFormat format, IRectangle portion = null, int quality = 50)
        {
            file += "." + format.ToString();
            IBuffer img;
            bool donotdispose = true;

            if (portion == null)
                img = this;
            else
            {
                var rc = this.CompitibleRC(portion?.X, portion?.Y, portion?.Width, portion?.Height);
                img = Factory.newBuffer(rc.Width, rc.Height);
                CopyTo(rc, img.Pixels, img.Length, img.Width, 0, 0);
                donotdispose = false;
            }
#if Window
            if (format == ImageFormat.BMP)
            {
                var done = Factory.SaveAsBitmap(img, file);
                if (done)
                {
                    if(!donotdispose)
                        img.Dispose();
                    return;
                }
            }
#endif
            Factory.ImageProcessor.Writer.Write(img, file, format, quality);
            if (!donotdispose)
                img.Dispose();
        }
        #endregion
        
        #region DISPOSE
        public abstract void Dispose();
        #endregion

        #region ASSIGN ID
        protected virtual void AssignIDIfNone() { }
        void IStoreable.AssignIDIfNone() => AssignIDIfNone();
        #endregion

        public event EventHandler<IEventArgs> BackgroundChanged;
    }
}
