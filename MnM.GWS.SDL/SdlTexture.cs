using System;
using System.Linq;
using System.Runtime.CompilerServices;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class SdlFactory
    {
#else
    public
#endif
        class SdlTexture : GwsTexture, ITexture
        {
            #region VARIABLES
            protected int w, h;
            protected static readonly IntPtr i0 = IntPtr.Zero;
            protected IntPtr Renderer;
            protected bool locked;
            protected int texturePitch;
            protected IntPtr textureData;
            protected IRectangle lockedArea;
            protected uint? pixelFormat;
            protected TextureAccess textureAccess;
            #endregion

            #region CONSTRUCTORS
            public SdlTexture(IRenderWindow window, int? w = null, int? h = null, bool isPrimary = false, uint? pixelFormat = null)
            {
                Initialize(window, isPrimary);
                Initialize(w ?? window.Width, window.Height);
                Renderer = SdlFactory.GetRenderer(window.Handle);
                this.pixelFormat = pixelFormat;
                textureAccess = TextureAccess.Streaming;
                if (Renderer == IntPtr.Zero)
                    Renderer = SdlFactory.CreateRenderer(window.Handle, -1, window.RendererFlags);
                Handle = SdlFactory.CreateTexture(Renderer,
                    pixelFormat ?? Factory.PixelFormat, textureAccess, Width, Height);
                SdlFactory.SetTextureBlendMod(Handle, BlendMode.None);
            }
            public SdlTexture(IRenderWindow window, IBuffer source, bool isPrimary = false, uint? pixelFormat = null)
            {
                Initialize(window, isPrimary);
                Initialize(source.Width, source.Height);
                Renderer = SdlFactory.GetRenderer(window.Handle);
                textureAccess = TextureAccess.Static;
                this.pixelFormat = pixelFormat;
                if (Renderer == IntPtr.Zero)
                    Renderer = SdlFactory.CreateRenderer(window.Handle, -1, window.RendererFlags);
                Handle = SdlFactory.CreateTexture(Renderer, source);
                SdlFactory.SetTextureBlendMod(Handle, BlendMode.None);
            }
            protected override void Initialize(int width, int height)
            {
                w = width;
                h = height;
            }
            #endregion

            #region PROPERTIES
            public int LockedLength
            {
                get
                {
                    if (lockedArea == null)
                        return 0;
                    return lockedArea.Height * texturePitch;
                }
            }
            public sealed override BlendMode Mode
            {
                get => SdlFactory.GetTextureBlendMode(Handle);
                set =>
                    SdlFactory.SetTextureBlendMod(Handle, value);
            }
            public sealed override byte Alpha
            {
                get => SdlFactory.GetTextureAlpha(Handle);
                set => SdlFactory.SetTextureAlpha(Handle, value);
            }
            public sealed override int ColorMode
            {
                get => SdlFactory.GetTextureColorMod(Handle);
                set => SdlFactory.SetTextureColorMod(Handle, value);
            }
            public sealed override int Width => w;
            public sealed override int Height => h;
            #endregion

            #region LOCK - UNLOCK
            protected internal void LockTexture(IRectangle copyRc = null)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                if (locked)
                    UnlockTexture();

                texturePitch = 0;
                textureData = IntPtr.Zero;

                if (copyRc == null)
                {
                    SdlFactory.LockTexture(Handle, i0, out textureData, out texturePitch);
                    lockedArea = Factory.newRectangle(0, 0, Width, Height);
                }
                else
                {
                    lockedArea = Geometry.CompitibleRC(Width, Height, copyRc.X, copyRc.Y, copyRc.Width, copyRc.Height);
                    if (copyRc.Width == 0 || copyRc.Height == 0)
                    {
                        textureData = IntPtr.Zero;
                        lockedArea = null;
                        texturePitch = 0;
                        return;
                    }
                    SdlFactory.LockTexture(Handle, Factory.newRectangle(copyRc).Handle, out textureData, out texturePitch);
                }
                locked = true;
            }
            protected internal void UnlockTexture()
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                if (!locked)
                    return;
                SdlFactory.UnlockTexture(Handle);
                locked = false;
                lockedArea = null;
                texturePitch = 0;
                textureData = IntPtr.Zero;
            }
            #endregion

            #region UPLOAD
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sealed override void Upload(IRectangle area)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                var rc = this.CompitibleRC(area.X, area.Y, area.Width, area.Height);
                if (!CopyInternal(Window, rc, rc, out IRectangle s, out IRectangle r))
                    return;
                SdlFactory.RenderCopyTexture(Renderer, Handle, s, r);
                SdlFactory.UpdateRenderer(Renderer);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sealed override void Upload()
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");
                var updates = Window.PendingUpdates.ToArray();
                foreach (var rc in updates)
                {
                    if (!CopyInternal(Window, rc, rc, out IRectangle s, out IRectangle r))
                        return;
                    SdlFactory.RenderCopyTexture(Renderer, Handle, s, r);
                }
                SdlFactory.UpdateRenderer(Renderer);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sealed override void Upload(IRectangle sourceRc, IRectangle destRc)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                if (!CopyInternal(Window, sourceRc, destRc, out IRectangle s, out IRectangle r))
                    return;
                SdlFactory.RenderCopyTexture(Renderer, Handle, s, r);
                SdlFactory.UpdateRenderer(Renderer);
            }
            protected override void GetOutputSize(out int w, out int h) =>
                SdlFactory.GetRendererOutputSize(Renderer, out w, out h);
            #endregion

            #region DOWNLOAD
            public unsafe override void Download()
            {
                if (Window == null)
                    return;
                foreach (var item in Window.PendingUpdates)
                    CopyTo(item, Window.Pixels, Window.Length, Window.Width, item.X, item.Y);
                Window.PendingUpdates.Clear();
            }
            public unsafe override void Download(IRectangle rectangle)
            {
                if (Window == null)
                    return;
                CopyTo(rectangle, Window.Pixels, Window.Length, Window.Width, rectangle.X, rectangle.Y);
                Window.PendingUpdates.Clear(rectangle);
            }
            #endregion

            #region BIND - UNBIND
            public sealed override void Bind() =>
                SdlFactory.SetRenderTarget(Renderer, Handle);
            public sealed override void Unbind() =>
                SdlFactory.SetRenderTarget(Renderer, i0);
            #endregion

            #region COPY FROM - TO
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            unsafe bool CopyInternal(IBufferCopy source, IRectangle sourceRc, IRectangle destRc, out IRectangle src, out IRectangle dest)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");

                IRectangle srcRc = sourceRc;
                if (source is ISolidPen)
                    srcRc = destRc;

                if (!CorrectRegions(srcRc, destRc, out src, out dest))
                    return false;

                LockTexture(dest);
                source.CopyTo(src, textureData, LockedLength, Width, 0, 0);
                UnlockTexture();
                Window.PendingUpdates.Clear(src);
                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override unsafe IRectangle CopyTo(IRectangle copyRc, IntPtr dest, int destLen, int destW, int destX, int destY)
            {
                var rc = Factory.newRectangle(0, 0, copyRc.Width, copyRc.Height);
                int length = LockedLength;

                LockTexture(copyRc);
                var src = (int*)textureData;
                var dst = (int*)dest;
                var result = CopyBlock(length, copyRc, Width, destX, destY, destW, destLen, (si, di, w, i) => CopyMemory(src, si, dst, di, w));

                UnlockTexture();
                return result; 
            }
            #endregion

            #region RESIZE
            public unsafe override void Resize(int? width = null, int? height = null)
            {
                var textureRc = Geometry.CompitibleRC(this, w0: width, h0: height);

                var srcW = width ?? w;
                var srcH = height ?? h;

                int[] data = new int[srcW * srcH];

                LockTexture(textureRc);

                CopyBlock(LockedLength, lockedArea, w, 0, 0, width ?? w, Length,
                    (si, di, w, i) => CopyMemory((int*)textureData, si, data, di, w));

                UnlockTexture();

                if (Handle != IntPtr.Zero)
                    SdlFactory.DestroyTexture(Handle);

                Handle = SdlFactory.CreateTexture(Renderer, pixelFormat ?? Factory.PixelFormat, textureAccess, width ?? Width, height ?? Height);
                SdlFactory.QueryTexture(Handle, out _, out _, out w, out h);

                var buffer = Factory.newBuffer(data, srcW, srcH);
                buffer.CopyTo(Factory.newRectangle(0, 0, srcW, srcH), textureData, LockedLength, Width, 0, 0);
                UnlockTexture();
                SdlFactory.SetTextureBlendMod(Handle, BlendMode.None);
            }
            #endregion

            #region TO SURFACE
            public sealed override IGraphics ToCanvas(int? x = null, int? y = null, int? width = null, int? height = null)
            {
                var dr = Geometry.CompitibleRC(this, x, y, width, height);
                var surface = new SdlSurface();
                surface.Initialize(dr.Width, dr.Height, default(int[]));
                SdlFactory.UpdateRenderer(Renderer);
                SdlFactory.SetRenderTarget(Renderer, Handle);
                var handle = dr.Handle;
                SdlFactory.ReadPixels(Renderer, handle, SdlFactory.pixelFormat,
                    surface.Pixels, dr.Width * SdlFactory.Pitch(SdlFactory.pixelFormat));
                SdlFactory.SetRenderTarget(Renderer, i0);
                return surface;
            }
            #endregion

            #region DISPOSE
            public sealed override void Dispose()
            {
                base.Dispose();
                SdlFactory.DestroyTexture(Handle);
            }
            #endregion
        }
#if AllHidden
    }
#endif
}
