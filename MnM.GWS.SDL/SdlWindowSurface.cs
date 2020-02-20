using System;
using System.Linq;

namespace MnM.GWS
{
    public sealed class SdlWindowSurface: GwsGraphics, IWindowSurface, ISdlSurface
    {
        IRenderWindow Window;
        #region CONSTRUCTORS
        public SdlWindowSurface(IRenderWindow window)
        {
            this.Window = window;
            PrimaryBuffer = new SdlBuffer(Handle.ToStruct<SurfaceInfo>());
        }
        #endregion

        #region PROPERTIES
        public IntPtr Handle { get; private set; }
        public bool SuspendLayout 
        { 
            get => Window.SuspendLayout; 
            set => Window.SuspendLayout = value; 
        }
     
        #endregion

        #region NEW INSTANCE
        protected override void Initialize( int width,  int height) 
        { }
        protected override GwsGraphics newInstance() =>
            new SdlWindowSurface(Window);
        #endregion

        #region RESIZE
        protected override void GetOutputSize(out int rw, out int rh)
        {
            rw = PrimaryBuffer.Width;
            rh = PrimaryBuffer.Height;
        }
        protected override void ResetDataInternally(int width, int height)
        {
            Handle = SdlFactory.GetWindowSurface(Window.Handle);
            PrimaryBuffer = new SdlBuffer(Handle.ToStruct<SurfaceInfo>());
        }
        #endregion

        #region DISPOSE
        protected override void OnDispose()
        {
            SdlFactory.FreeSurface(Handle);
        }
        #endregion

        #region UPDATE
        public void Submit()
        {
            SdlFactory.UpdateWindowSurface(Window.Handle, PendingUpdates.Select(x => new Rect(x)).ToArray());
            PendingUpdates.Clear();
        }
        public void Submit(IRectangle rectangle)
        {
            SdlFactory.UpdateWindowSurface(Window.Handle, new Rect[] { new Rect(rectangle) });
            PendingUpdates.Clear(rectangle);
        }
        #endregion

        #region DISCARD
        public void Discard()
        {
            //
        }
        public void Discard(IRectangle rectangle)
        {
            //
        }
        #endregion
    }
}
