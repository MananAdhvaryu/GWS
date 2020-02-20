using System;

namespace MnM.GWS
{
    public interface IEvent : IStoreable
    {
        GwsEvent Type { get; }
        IExEvent Window { get; }
    }
    public interface IExEvent
    {
        GwsEvent Type { get; }
        int WindowID { get; }
        WindowEventID Event { get; }
    }
    //public interface WindowEvent
    //{
    //    public EventType Type;
    //    public uint Timestamp;
    //    public int WindowID;
    //    public WindowEventID Event;
    //    private byte padding1;
    //    private byte padding2;
    //    private byte padding3;
    //    public int Data1;
    //    public int Data2;
    //}
    public interface IEventProcessor : IStoreable
    {
        bool ProcessEvent( IEvent @event);
    }
    public interface IEventUser : IStoreable
    {
        EventUseStatus UseEvent(EventInfo e);
        bool IsMouseDragging { get; }
    }
    public interface IEventInfo
    {
        string Sender { get; }
        IEventArgs Args { get; }
        GwsEvent Type { get; }
    }
    public interface IEventProvider : IStoreable
    {
        event EventHandler<IEventArgs> Moved;
        event EventHandler<ISizeEventArgs> Resized;
        event EventHandler<IEventArgs> LostFocus;
        event EventHandler<IEventArgs> GotFocus;
        event EventHandler<IKeyEventArgs> KeyDown;
        event EventHandler<IKeyEventArgs> KeyUp;
        event EventHandler<IKeyEventArgs> PreviewKeyDown;
        event EventHandler<IKeyPressEventArgs> KeyPress;
        event EventHandler<IMouseEventArgs> MouseWheelScrolled;
        event EventHandler<IMouseEventArgs> MouseDown;
        event EventHandler<IMouseEventArgs> MouseUp;
        event EventHandler<IMouseEventArgs> MouseClick;
        event EventHandler<IMouseEventArgs> MouseDoubleClick;
        event EventHandler<IMouseEventArgs> MouseMove;
        event EventHandler<IMouseEventArgs> AppClicked;
        event EventHandler<IEventArgs> MouseEnter;
        event EventHandler<IEventArgs> MouseLeave;
        event EventHandler<IEventArgs> VisibleChanged;
        event EventHandler<ITouchEventArgs> TouchBegan;
        event EventHandler<ITouchEventArgs> TouchMoved;
        event EventHandler<ITouchEventArgs> TouchEnded;
        event EventHandler<IMouseEventArgs> MouseDrag;
        event EventHandler<IMouseEventArgs> MouseDragBegin;
        event EventHandler<IMouseEventArgs> MouseDragEnd;
        event EventHandler<IJoystickButtonEventArgs> JoystickDown;
        event EventHandler<IJoystickButtonEventArgs> JoystickUp;
        event EventHandler<IJoystickAxisEventArgs> JoystickMove;
        event EventHandler<IEventArgs> JoystickConnected;
        event EventHandler<IEventArgs> JoystickDisconnected;
    }
    public interface IWindowEventProvider : IEventProvider
    {
        event EventHandler<ICancelEventArgs> Closing;
        event EventHandler<IEventArgs> Closed;
        event EventHandler<IEventArgs> Load;
        event EventHandler<IEventArgs> TitleChanged;
        event EventHandler<IEventArgs> WindowBorderChanged;
        event EventHandler<IEventArgs> WindowStateChanged;
        event EventHandler<IPaintEventArgs> Paint;
    }
}
