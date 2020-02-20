using System;
using System.Collections.Generic;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AdvancedVersion
    public abstract partial class GwsControl : IControl
    {
        protected IBuffer Buffer;

        protected GwsControl()
        {
            Name = GetType().Name;
            ID = Factory.NewID(Name);
            Popups = Implementation.Factory.newPopupCollection(this);
            Controls = Implementation.Factory.newElementCollection(this);
        }

        #region PROPERTIES
        public abstract int X { get; }
        public abstract int Y { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public IPopupCollection Popups { get; private set; }
        public bool SuspendLayout { get; set; }
        public IBufferCollection Buffers => Buffer.Buffers;
        public IElementCollection Controls { get; private set; }
        public abstract Size MinSize { get; set; }
        public abstract Size MaxSize { get; set; }
        public string ID { get; protected set; }
        public string Name { get; private set; }
        public virtual bool IsDisposed { get; private set; }
        public abstract IRectangleF Bounds { get; }
        public IParent Parent { get; private set; }
        #endregion

        #region ADVANCED VERSION PROPERTIES
#if AdvancedVersion
        public virtual IEventUser ActiveObject { get; set; }
        public abstract bool IsMouseDragging { get; }
        public virtual bool IsWindow => false;
        public virtual int TabIndex { get; set; }
        public abstract bool Focused { get; }
        public abstract bool Visible { get; set; }
        public abstract bool Enabled { get; set; }
#endif
        #endregion

        #region REFRESH
        public abstract void Refresh();
        #endregion

        #region SHOW - HIDE
        public abstract void Show();
        public abstract void Hide();
        #endregion

        #region POINT TO CLIENT - SCREEN
        public abstract Point PointToClient(Point point);
        public abstract Point PointToScreen(Point point);
        public abstract bool Contains(float x, float y);
        #endregion

        #region DISPOSE
        public virtual void Dispose()
        {
            IsDisposed = true;
        }
        #endregion

        void IStoreable.AssignIDIfNone()
        {
            if (ID == null)
                ID = Factory.NewID(Name);
        }

        #region BUFFER CHANGED
        public void BufferChanged(IBuffer buffer, IElementCollection path)
        {
            if (buffer == Buffer && path[this]?.Shape == ID)
                return;
            Buffer = buffer;
            Parent = path.Window;
        }
        #endregion

        #region SUBMIT
        public virtual void Submit()
        {
            if (Parent == null || SuspendLayout)
                return;
            Parent.Submit();
        }
        public virtual void Submit(IRectangle rectangle)
        {
            if (Parent == null || SuspendLayout)
                return;
            Parent.Submit(rectangle);
        }
        #endregion

        #region DISCARD
        public void Discard()
        {
            if (Parent == null)
                return;
            Parent.Discard();
        }
        public void Discard(IRectangle rectangle)
        {
            if (Parent == null)
                return;
            Parent.Discard(rectangle);
        }
        #endregion

        #region INVALIDATE
        public virtual void Invalidate(IRectangle rectangle)
        {
            Buffer?.PendingUpdates?.Invalidate(rectangle);
        }
        public virtual void Invalidate(IEnumerable<IRectangle> rectangles)
        {
            Buffer?.PendingUpdates?.Invalidate(rectangles);
        }
        #endregion

        #region MOVE - RESIZE
        public abstract void Move(int? x = null, int? y = null);
        public void Resize(int? w = null, int? h = null)
        {
            if (w != null || h != null)
            {
                var wd = w ?? Width;
                var ht = h ?? Height;
                HandleResize(new SizeEventArgs(wd, ht));
            }
        }
        protected abstract void HandleResize(ISizeEventArgs e);
        #endregion

        #region HANDLE PAINT
        protected abstract void HandlePaint();
        #endregion

        #region ZORDER
        public abstract void BringToFront();
        public abstract void SendToBack();
        public abstract void BringForward(int numberOfPlaces = 1);
        public abstract void SendBackward(int numberOfPlaces = 1);
        #endregion

        #region FOCUS
        public abstract bool Focus();
        #endregion

        #region USE EVENT
        public abstract EventUseStatus UseEvent(EventInfo e);
        #endregion

        #region EVENT DECLARATION
        public event EventHandler<IPaintEventArgs> Paint;
        public event EventHandler<IEventArgs> Moved;
        public event EventHandler<ISizeEventArgs> Resized;
        public event EventHandler<IEventArgs> LostFocus;
        public event EventHandler<IEventArgs> GotFocus;
        public event EventHandler<IKeyEventArgs> KeyDown;
        public event EventHandler<IKeyEventArgs> KeyUp;
        public event EventHandler<IKeyEventArgs> PreviewKeyDown;
        public event EventHandler<IKeyPressEventArgs> KeyPress;
        public event EventHandler<IMouseEventArgs> MouseWheelScrolled;
        public event EventHandler<IMouseEventArgs> MouseDown;
        public event EventHandler<IMouseEventArgs> MouseUp;
        public event EventHandler<IMouseEventArgs> MouseClick;
        public event EventHandler<IMouseEventArgs> MouseDoubleClick;
        public event EventHandler<IMouseEventArgs> MouseMove;
        public event EventHandler<IMouseEventArgs> AppClicked;
        public event EventHandler<IEventArgs> MouseEnter;
        public event EventHandler<IEventArgs> MouseLeave;
        public event EventHandler<IEventArgs> VisibleChanged;
        public event EventHandler<IEventArgs> AbilityChanged;

        public event EventHandler<ITouchEventArgs> TouchBegan;
        public event EventHandler<ITouchEventArgs> TouchMoved;
        public event EventHandler<ITouchEventArgs> TouchEnded;
        public event EventHandler<IMouseEventArgs> MouseDrag;
        public event EventHandler<IMouseEventArgs> MouseDragBegin;
        public event EventHandler<IMouseEventArgs> MouseDragEnd;
        public event EventHandler<IJoystickButtonEventArgs> JoystickDown;
        public event EventHandler<IJoystickButtonEventArgs> JoystickUp;
        public event EventHandler<IJoystickAxisEventArgs> JoystickMove;
        public event EventHandler<IEventArgs> JoystickConnected;
        public event EventHandler<IEventArgs> JoystickDisconnected;
        public event EventHandler<IEventArgs> Load;
        #endregion

        #region INPUT PROCESSING
        protected virtual void OnKeyDown(IKeyEventArgs e) =>
            KeyDown?.Invoke(this, e);
        protected virtual void OnPreviewKeyDown(IKeyEventArgs e) =>
            PreviewKeyDown?.Invoke(this, e);

        protected virtual void OnKeyUp(IKeyEventArgs e) =>
            KeyUp?.Invoke(this, e);
        protected virtual bool OnKeyPress(IKeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
            return true;
        }
        protected virtual void OnMouseWheel(IMouseEventArgs e) =>
            MouseWheelScrolled?.Invoke(this, e);
        protected virtual void OnMouseDown(IMouseEventArgs e) =>
            MouseDown?.Invoke(this, e);
        protected virtual void OnMouseUp(IMouseEventArgs e) =>
            MouseUp?.Invoke(this, e);
        protected virtual void OnMouseClick(IMouseEventArgs e) =>
            MouseClick?.Invoke(this, e);
        protected virtual void OnMouseDoubleClick(IMouseEventArgs e) =>
            MouseDoubleClick?.Invoke(this, e);
        protected virtual void OnMouseDragBegin(IMouseEventArgs e) =>
            MouseDragBegin?.Invoke(this, e);
        protected virtual void OnMouseDragEnd(IMouseEventArgs e) =>
            MouseDragEnd?.Invoke(this, e);
        protected virtual void OnMouseDrag(IMouseEventArgs e) =>
            MouseDrag?.Invoke(this, e);

        protected virtual void OnMouseMove(IMouseEventArgs e) =>
            MouseMove?.Invoke(this, e);
        protected virtual void OnAppClicked(IMouseEventArgs e) =>
            AppClicked?.Invoke(this, e);
        protected virtual void OnMouseEnter(IMouseEventArgs e) =>
            MouseEnter?.Invoke(this, e);
        protected virtual void OnMouseLeave(IMouseEventArgs e) =>
            MouseLeave?.Invoke(this, e);

        protected virtual void OnPaint(IPaintEventArgs e) =>
            Paint?.Invoke(this, e);
        protected virtual void OnVisibleChanged(IEventArgs e) =>
            VisibleChanged?.Invoke(this, e);

        protected virtual void OnAbilityChanged(IEventArgs e) =>
            AbilityChanged?.Invoke(this, e);

        protected virtual void OnMoved(IEventArgs e) =>
            Moved?.Invoke(this, e);
        protected virtual void OnResize(ISizeEventArgs e) =>
            Resized?.Invoke(this, e);
        protected virtual void OnTouchBegan(ITouchEventArgs e) =>
            TouchBegan?.Invoke(this, e);
        protected virtual void OnTouchMoved(ITouchEventArgs e) =>
            TouchMoved?.Invoke(this, e);
        protected virtual void OnTouchEnded(ITouchEventArgs e) =>
            TouchEnded?.Invoke(this, e);

        protected virtual void OnJoystickDown(IJoystickButtonEventArgs e) =>
            JoystickDown?.Invoke(this, e);
        protected virtual void OnJoystickUp(IJoystickButtonEventArgs e) =>
            JoystickUp?.Invoke(this, e);
        protected virtual void OnJoystickMove(IJoystickAxisEventArgs e) =>
            JoystickMove?.Invoke(this, e);

        protected virtual void OnGotFocus(IEventArgs e) =>
            GotFocus?.Invoke(this, e);
        protected virtual void OnLostFocus(IEventArgs e) =>
            LostFocus?.Invoke(this, e);
        protected virtual void OnLoad(IEventArgs e) =>
            Load?.Invoke(this, e);
        protected virtual void OnJoystickConnected(IEventArgs e) =>
            JoystickConnected?.Invoke(this, e);
        protected virtual void OnJoystickDisconnected(IEventArgs e) =>
            JoystickDisconnected?.Invoke(this, e);
        #endregion
    }

    partial class GwsControl
    {
        #region IBUFFER
#if AdvancedVersion
        public IUpdateManager PendingUpdates =>
            Buffer.PendingUpdates;
        public string BackgroundPen => Buffer.BackgroundPen;
        int IBuffer.this[int index]
        {
            get => Buffer[index];
            set => Buffer[index] = value;
        }
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
        public void SaveAs(string file, ImageFormat format, IRectangle portion = null, int quality = 50)
        {
            Buffer.SaveAs(file, format, portion, quality);
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

#endif
        #endregion
    }
#endif
}
