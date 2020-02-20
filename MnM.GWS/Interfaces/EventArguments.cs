using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public interface IEventArgs
    {
        bool Handled { get; set; }
    }
    public interface ITextInputEventArgs: IEventArgs
    {
        char[] Characters { get; }
    }
    public interface IInputEventArgs : IEventArgs
    {
        bool Enter { get; set; }
    }
    public interface IJoystickDevice
    {
        int Which { get; }
    }
    public interface IJoystickHatEventArgs : IJoystickDevice, IEventArgs
    {
        int Index { get; }
        int Value { get; }
    }
    public interface IJoystickButtonEventArgs : IJoystickDevice, IEventArgs
    {
        int Button { get; }
        bool Pressed { get; }
    }
    public interface IJoystickBallEventArgs : IEventArgs
    {
        int Device { get; }
        int Button { get; }
        bool Pressed { get; }
        int XDelta { get; }
        int YDelta { get; }
    }
    public interface IJoystickAxisEventArgs : IEventArgs
    {
        int Device { get; }
        int Axis { get; }
        float Value { get; }
        float Delta { get; }
    }

    public interface IGamePad
    {
        /// <summary>
        /// Gets the <see cref="State"/> for the up button.
        /// </summary>
        /// <value><c>ButtonState.Pressed</c> if the up button is pressed; otherwise, <c>ButtonState.Released</c>.</value>
        bool Up { get; }

        /// <summary>
        /// Gets the <see cref="State"/> for the down button.
        /// </summary>
        /// <value><c>ButtonState.Pressed</c> if the down button is pressed; otherwise, <c>ButtonState.Released</c>.</value>
        bool Down { get; }

        /// <summary>
        /// Gets the <see cref="State"/> for the left button.
        /// </summary>
        /// <value><c>ButtonState.Pressed</c> if the left button is pressed; otherwise, <c>ButtonState.Released</c>.</value>
        bool Left { get; }

        /// <summary>
        /// Gets the <see cref="State"/> for the right button.
        /// </summary>
        /// <value><c>ButtonState.Pressed</c> if the right button is pressed; otherwise, <c>ButtonState.Released</c>.</value>
        bool Right { get; }

        /// <summary>
        /// Gets a value indicating whether the up button is pressed.
        /// </summary>
        /// <value><c>true</c> if the up button is pressed; otherwise, <c>false</c>.</value>
        bool IsUp { get; }

        /// <summary>
        /// Gets a value indicating whether the down button is pressed.
        /// </summary>
        /// <value><c>true</c> if the down button is pressed; otherwise, <c>false</c>.</value>
        bool IsDown { get; }

        /// <summary>
        /// Gets a value indicating whether the left button is pressed.
        /// </summary>
        /// <value><c>true</c> if the left button is pressed; otherwise, <c>false</c>.</value>
        bool IsLeft { get; }

        /// <summary>
        /// Gets a value indicating whether the right button is pressed.
        /// </summary>
        /// <value><c>true</c> if the right button is pressed; otherwise, <c>false</c>.</value>
        bool IsRight { get; }
    }
    public interface IGamePadThumbSticks : IEquatable<IGamePadThumbSticks>
    {
        PointF Left { get; }
        PointF Right { get; }
    }
    public interface IGamePadCapabilities : IEquatable<IGamePadCapabilities>
    {
        GamePadType GamePadType { get; }

        int Buttons { get; }

        bool HasDPadUpButton { get; }
        bool HasDPadDownButton { get; }
        bool HasDPadLeftButton { get; }
        bool HasDPadRightButton { get; }
        bool HasAButton { get; }
        bool HasBButton { get; }
        bool HasXButton { get; }
        bool HasYButton { get; }
        bool HasLeftStickButton { get; }
        bool HasRightStickButton { get; }
        bool HasLeftShoulderButton { get; }
        bool HasRightShoulderButton { get; }
        bool HasBackButton { get; }
        bool HasBigButton { get; }
        bool HasStartButton { get; }
        bool HasLeftXThumbStick { get; }
        bool HasLeftYThumbStick { get; }
        bool HasRightXThumbStick { get; }
        bool HasRightYThumbStick { get; }
        bool HasLeftTrigger { get; }
        bool HasRightTrigger { get; }
        bool HasLeftVibrationMotor { get; }
        bool HasRightVibrationMotor { get; }
        bool HasVoiceSupport { get; }
        bool IsConnected { get; }
        bool IsMapped { get; }
    }
    public interface IGamePadDriver
    {
        IGamePadState GetState( int index);
        IGamePadCapabilities GetCapabilities( int index);
        string GetName( int index);
        bool SetVibration( int index,  float left,  float right);
    }
    public interface IGamePadState : IEquatable<IGamePadState>
    {
        IGamePadThumbSticks ThumbSticks { get; }
        IGamePadButtons Buttons { get; }
        IGamePadDPad DPad { get; }
        IGamePadTriggers Triggers { get; }
        bool IsConnected { get; }
        int PacketNumber { get; }
    }
    public interface IGamePadButtons
    {
        int Buttons { get; }
        State A { get; }
        State B { get; }
        State X { get; }
        State Y { get; }
        State Back { get; }
        State BigButton { get; }
        State LeftShoulder { get; }
        State LeftStick { get; }
        State RightShoulder { get; }
        State RightStick { get; }
        State Start { get; }
        bool IsAnyButtonPressed { get; }
    }
    public interface IGamePadDPad
    {
        int Buttons { get; }
        State Up { get; }
        State Down { get; }
        State Left { get; }
        State Right { get; }
        bool IsUp { get; }
        bool IsDown { get; }
        bool IsLeft { get; }
        bool IsRight { get; }
    }
    public interface IGamePadTriggers
    {
        float Left { get; }
        float Right { get; }
    }

    public interface IKeyEventArgs : IInputEventArgs
    {
        Key KeyCode { get; }
        int ScanCode { get; }
        bool Alt { get; }
        bool Control { get; }
        bool Shift { get; }
        Key Modifiers { get; }
        KeyState State { get; }
        bool SupressKeypress { get; set; }
    }
    public interface IMouseEventArgs : IInputEventArgs
    {
        int X { get; }
        int Y { get; }
        MouseState State { get; }
        MouseButton Button { get; }
        int Clicks { get; }
        int Delta { get; }
        Point Position { get; }
        bool Clicked { get; }
        Point StartPoint { get; }
        int XDelta { get; }
        int YDelta { get; }
    }
    public interface IPaintEventArgs : IEventArgs, IDisposable
    {
        IBuffer Graphics { get; }
    }
    public interface ICancelEventArgs : IEventArgs
    {
        bool Cancel { get; set; }
    }
    public interface ISizeEventArgs : IEventArgs, ISize
    {
        Size Size { get; }
    }
    public interface ITickEventArgs : IEventArgs
    {
        int LastTick { get; }
        int Fps { get; }
        int Tick { get; }
        int TicksElapsed { get; }
        float SecondsElapsed { get; }
    }
    public interface IFileDropEventArgs : IEventArgs
    {
        string FileName { get; set; }
    }
    public interface IFrameEventArgs : IEventArgs
    {
        /// <summary>
        /// Gets a <see cref="System.Double"/> that indicates how many seconds of time elapsed since the previous event.
        /// </summary>
        double Time { get; }
    }
    public interface ITouchEventArgs : IEventArgs
    {
        float X { get; }
        float Y { get; }
        /// <summary>Index of the finger  case of multi-touch events</summary>
        long Finger { get; }
    }

    public interface IKeyPressEventArgs : IEventArgs
    {
        char KeyChar { get; }
    }
    public interface IUploadEventArgs : IEventArgs
    {
        byte[] Data { get; }
        IRectangle Area { get; }
    }
}
