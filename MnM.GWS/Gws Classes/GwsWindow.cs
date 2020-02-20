using System;
using System.Collections.Generic;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if Window
    public abstract class GwsWindow : GwsRenderWindow, IWindow
    {
        #region VARIABLES
        protected bool previousCursorVisible = true;
        protected bool cursorVisible;
        readonly KeyPressEventArgs KeyPressEventArgs = new KeyPressEventArgs();
        readonly ICancelEventArgs CancelEventArgs = Factory.newCancelEventArgs();
        readonly EventInfo Event = new EventInfo();
        #endregion

        #region CONSTRUCTORS
        protected GwsWindow(string title = null, int? width = null, int? height = null,
            int? x = null, int? y = null, GwsWindowFlags? flags = null, IScreen display = null,
            IntPtr? externalWindow = null, bool isSingleThreaded = true, RendererFlags? renderFlags = null)
        {
            Name = GetType().Name;
            if (!Initialize(title, width, height, x, y, flags, display, externalWindow, isSingleThreaded, renderFlags))
                throw new Exception("Window could not be initialized!");
        }
        protected abstract bool Initialize(string title = null, int? width = null, int? height = null,
                int? x = null, int? y = null, GwsWindowFlags? flags = null, IScreen display = null,
                IntPtr? externalWindow = null, bool isSingleThreaded = true, RendererFlags? renderFlags = null);
        #endregion

        #region PROPERTIES
        public bool IsDisposed { get; private set; }
        public string Name { get; protected set; }
        public int WindowID { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public bool Exists { get; protected set; }
        public abstract Size MinSize { get; set; }
        public abstract Size MaxSize { get; set; }

        public IRectangleF Bounds
        {
            get => Factory.newRectangleF(X, Y, Width, Height);
            protected set
            {
                value.Round(out int x, out int y, out int w, out int h);
                InitializeArea(x, y, w, h);
            }
        }
        public bool Transparent => Transparency != 0f;
        public virtual IScreen Screen { get; set; }
        public virtual float Transparency { get; set; }
        public virtual WindowState WindowState { get; protected set; }
        public virtual WindowBorder WindowBorder { get; protected set; }
        public virtual string Title { get; set; }
        public virtual IRectangle ClipRectangle { get; set; }
        public virtual PointF Scale { get; set; }
        public virtual bool CursorVisible { get; set; }
        protected CursorType? ResizeCursor { get; set; }
#if AdvancedVersion
        public bool IsMouseDragging => Controls.IsMouseDragging;
#endif
        public virtual int TabIndex { get; set; }
        public abstract uint PixelFormat { get; }
        public abstract ISound Sound { get; }
        public abstract bool Focused { get; }
        public abstract bool Visible { get; set; }
        public abstract bool Enabled { get; set; }
        #endregion

        #region SHOW - HIDE
        public void Show() =>
            ChangeVisible(true);
        public void Hide() =>
            ChangeVisible(false);
        protected abstract void ChangeVisible(bool value);
        #endregion

        #region SHOW - HIDE - SET CURSOR
        public abstract void SetCursor(int x, int y);
        public abstract void ShowCursor();
        public abstract void HideCursor();
        #endregion

        #region POINT TO CLIENT - SCREEN
        public abstract Point PointToClient(Point point);
        public abstract Point PointToScreen(Point point);
        public bool Contains(float x, float y) =>
            x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        public abstract void ContainMouse(bool flag);
        #endregion

        #region REFRESH
        public abstract void Refresh();
        #endregion

        #region CHANGE SCREEN
        public abstract void ChangeScreen(int screenIndex);
        #endregion

        #region CHANGE STATE
        public abstract void ChangeState(WindowState state);
        #endregion

        #region CHANGE BBORDER
        public abstract void ChangeBorder(WindowBorder border);
        #endregion

        #region INITIALIZE AREA
        public void InitializeArea(int x, int y, int width, int height)
        {
            if (Handle == IntPtr.Zero)
                return;
            Move(x, y);
            Resize(width, height);
        }
        #endregion

        #region CLOSE - DISPOSE
        public abstract void Close();
        public override void Dispose()
        {
            IsDisposed = true;
            Close();
        }
        #endregion

        #region INPUT PROCESSING
        public bool ProcessEvent(IEvent @event)
        {
            var e = Process(@event);
            if (e == null)
                return false;
            Event.Sender = ID;
            Event.Args = e;
            Event.Type = @event.Type;
            return UseEvent(Event) != EventUseStatus.StopSendingMore;
        }
        protected abstract IEventArgs Process(IEvent @event);

        virtual public EventUseStatus UseEvent(EventInfo e)
        {
            EventUseStatus result = EventUseStatus.NotAbleToUse;

#if AdvancedVersion
            result = Controls.UseEvent(e);
#endif
            if (result == EventUseStatus.Used || result == EventUseStatus.StopSendingMore)
                return result;

            switch (e.Type)
            {
                case GwsEvent.KEYDOWN:
                    OnKeyDown(e.Args as IKeyEventArgs);
                    break;
                case GwsEvent.KEYUP:
                    OnKeyDown(e.Args as IKeyEventArgs);
                    break;
                case GwsEvent.TEXTINPUT:
                    var txtInput = e.Args as ITextInputEventArgs;
                    if (txtInput != null)
                    {
                        foreach (var item in txtInput.Characters)
                        {
                            KeyPressEventArgs.KeyChar = item;
                            OnKeyPress(KeyPressEventArgs);
                        }
                    }
                    break;
                case GwsEvent.MOUSEMOTION:
                    OnMouseMove(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.MOUSEBUTTONDOWN:
                    OnMouseDown(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.MOUSEBUTTONUP:
                    OnMouseUp(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.MOUSEWHEEL:
                    OnMouseWheel(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.SHOWN:
                case GwsEvent.HIDDEN:
                    OnVisibleChanged(e.Args);
                    break;
                case GwsEvent.EXPOSED:
                    HandlePaint();
                    break;
                case GwsEvent.MOVED:
                    OnMoved(e.Args as IEventArgs);
                    break;
                case GwsEvent.SIZE_CHANGED:
                    break;
                case GwsEvent.RESIZED:
                    HandleResize(e.Args as ISizeEventArgs);
                    break;
                case GwsEvent.ENTER:
                    OnMouseEnter(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.LEAVE:
                    OnMouseLeave(e.Args as IMouseEventArgs);
                    break;
                case GwsEvent.FOCUS_GAINED:
                    OnGotFocus(e.Args);
                    break;
                case GwsEvent.FOCUS_LOST:
                    OnLostFocus(e.Args);
                    break;
                case GwsEvent.CONTROLLERAXISMOTION:
                case GwsEvent.JOYAXISMOTION:
                    OnJoystickMove(e.Args as IJoystickAxisEventArgs);
                    break;
                case GwsEvent.JOYBUTTONDOWN:
                    OnJoystickDown(e.Args as IJoystickButtonEventArgs);
                    break;
                case GwsEvent.JOYBUTTONUP:
                    OnJoystickUp(e.Args as IJoystickButtonEventArgs);
                    break;
                case GwsEvent.FINGERDOWN:
                    OnTouchBegan(e.Args as ITouchEventArgs);
                    break;
                case GwsEvent.FINGERMOTION:
                    OnTouchBegan(e.Args as ITouchEventArgs);
                    break;
                case GwsEvent.FINGERUP:
                    OnTouchBegan(e.Args as ITouchEventArgs);
                    break;
                case GwsEvent.MINIMIZED:
                    OnMinimized(EventArgs.Empty);
                    break;
                case GwsEvent.MAXIMIZED:
                    OnMaximized(EventArgs.Empty);
                    break;
                case GwsEvent.RESTORED:
                    OnRestored(EventArgs.Empty);
                    break;
                case GwsEvent.CLOSE:
                    CancelEventArgs.Cancel = false;
                    OnClosing(CancelEventArgs);
                    if (CancelEventArgs.Cancel)
                        break;
                    OnClosed(e.Args);
                    break;
                case GwsEvent.LASTEVENT:
                case GwsEvent.FIRSTEVENT:
                case GwsEvent.QUIT:
                default:
                    break;
            }
            return result;
        }

        virtual
        protected void OnGotFocus(IEventArgs e)
        {
            if (!previousCursorVisible)
            {
                previousCursorVisible = true;
                CursorVisible = false;
            }
            GotFocus?.Invoke(this, e);
        }

        virtual
        protected void OnLostFocus(IEventArgs e)
        {
            if (!previousCursorVisible)
            {
                previousCursorVisible = true;
                CursorVisible = false;
            }
            GotFocus?.Invoke(this, e);
        }
        protected virtual void OnTitleChanged(IEventArgs e) =>
            TitleChanged?.Invoke(this, e);
        protected virtual void OnWindowBorderChanged(IEventArgs e) =>
            WindowBorderChanged?.Invoke(this, e);
        protected virtual void OnWindowStateChanged(IEventArgs e) =>
            WindowStateChanged?.Invoke(this, e);
        protected virtual void OnClosed(IEventArgs e) =>
            Closed?.Invoke(this, e);
        protected virtual void OnClosing(ICancelEventArgs e) =>
            Closed?.Invoke(this, e);
        protected virtual void OnMaximized(IEventArgs e) =>
            Closed?.Invoke(this, e);
        protected virtual void OnMinimized(IEventArgs e) =>
            Closed?.Invoke(this, e);
        protected virtual void OnRestored(IEventArgs e) =>
            Closed?.Invoke(this, e);
        #endregion

        #region EVENT DECLARATION
        public event EventHandler<IEventArgs> TitleChanged;
        public event EventHandler<IEventArgs> WindowBorderChanged;
        public event EventHandler<IEventArgs> WindowStateChanged;
        public event EventHandler<ICancelEventArgs> Closing;
        public event EventHandler<IEventArgs> Closed;

        public event EventHandler<IEventArgs> Minimized;
        public event EventHandler<IEventArgs> Maximized;
        public event EventHandler<IEventArgs> Restored;
        #endregion

        #region HANDLE RESIZE
        protected virtual void HandleResize(ISizeEventArgs e)
        {
            (Texture as IResizeable)?.Resize(e.Width, e.Height);
            Buffer.Resize(e.Width, e.Height);
            Buffer.ClearBackground(0, 0, e.Width, e.Height);
            Controls.RefreshAll();
            Refresh();
        }
        #endregion

        #region HANDLE PAINT
        protected virtual void HandlePaint()
        {
            if (Width == 0 || Height == 0 || !Visible)
                return;

            using (var e = Factory.newPaintEventArgs(Buffer))
            {
                OnPaint(e);
            }
        }
        #endregion

        #region MOVE
        public abstract void Move(int? x = null, int? y = null);
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

        #region EVENT DECLARATION
        public event EventHandler<EventArgs<IEnumerable<IRectangle>>> Updated;
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
        protected virtual void OnMoved(IEventArgs e) =>
            Moved?.Invoke(this, e);
        protected virtual void OnResize(ISizeEventArgs e)
        {
            HandleResize(e);
            Resized?.Invoke(this, e);
        }
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
        protected virtual void OnLoad(IEventArgs e) =>
            Load?.Invoke(this, e);
        protected virtual void OnJoystickConnected(IEventArgs e) =>
            JoystickConnected?.Invoke(this, e);
        protected virtual void OnJoystickDisconnected(IEventArgs e) =>
            JoystickDisconnected?.Invoke(this, e);
        protected virtual void OnUpdated(EventArgs<IEnumerable<IRectangle>> e) =>
            Updated?.Invoke(this, e);
        #endregion
    }
#endif
}
