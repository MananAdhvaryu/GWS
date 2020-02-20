using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
#if Window
    public interface IWindowID
    {
        /// <summary>
        /// Returns the Window Handle id given by the operating system.
        /// </summary>
        int WindowID { get; }
    }

    /// <summary>
    /// Addional properties and methods of Windows that buffers do not have.
    /// </summary>
    public interface IWindow : IWindowID, IWindowEventProvider, IEventProcessor, IHandle, IRenderWindow,
        IFocusable, IMoveable, IResizeable, IOVerlap, IUpdatable, IParent, IEventProvider, IMinMaxSizeHolder,
        IDispose, IObject, IRefreshable, IPoint, ISize
    {
        /// <summary>
        /// Returns true if this object is being disposed.
        /// </summary>
        new bool IsDisposed { get; }
        /// <summary>
        /// Pixel format as used by the windowing system e.g. SDL.
        /// </summary>
        uint PixelFormat { get; }
        /// <summary>
        /// Gets or sets the screen properties.
        /// </summary>
        IScreen Screen { get; set; }
        /// <summary>
        /// Gets or sets the transparency of the Window.
        /// </summary>
        float Transparency { get; set; }
        /// <summary>
        /// Gets the current window state enum: Normal, Minimised...
        /// </summary>
        WindowState WindowState { get; }
        /// <summary>
        /// Gets the enum documenting the display properties of the border:Fixed, Resizable...
        /// </summary>
        WindowBorder WindowBorder { get; }
        /// <summary>
        /// Gets or sets the Window Title text.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Defines a rectangle on the Window in which changes can be made.
        /// </summary>
        IRectangle ClipRectangle { get; set; }
        /// <summary>
        /// Gets or sets points on the window and their properties.
        /// </summary>
        PointF Scale { get; set; }
        /// <summary>
        /// Returns True if Window has been initialised
        /// </summary>
        bool Exists { get; }
        /// <summary>
        /// Gets or sets the visibility state of the cursor.
        /// </summary>
        bool CursorVisible { get; set; }
        /// <summary>
        /// Gets the ISound object for audio playback.
        /// </summary>
        ISound Sound { get; }
        /// <summary>
        /// Close the window and manage memory.
        /// </summary>
        void Close();
        /// <summary>
        /// Sets the Cursor to given (x,y) co-ordinates. 
        /// </summary>
        /// <param name="x">x ordinate of cursor position.</param>
        /// <param name="y">y ordinate of cursor position.</param>
        void SetCursor(int x, int y);
        /// <summary>
        /// Set the cursor to visible.
        /// </summary>
        void ShowCursor();
        /// <summary>
        /// Sets the cursor to hidden.
        /// </summary>
        void HideCursor();
        /// <summary>
        /// Move Window to the screen with givn number.
        /// </summary>
        /// <param name="screenIndex">Number of the Screen to move to.</param>
        void ChangeScreen(int screenIndex);
        /// <summary>
        /// Sets mouse pointer within the Window area.
        /// </summary>
        /// <param name="flag">!!!!</param>
        void ContainMouse(bool flag);

        /// <summary>
        /// Changes window state: Normal, Maximised etc
        /// </summary>
        /// <param name="state">Windows star Enum describing state.</param>
        void ChangeState(WindowState state);
        /// <summary>
        /// Change the border stting ENum so the the border can be drawn 
        /// </summary>
        /// <param name="border">Enum for new border state.</param>
        void ChangeBorder(WindowBorder border);
        
        Point PointToClient(Point point);
        Point PointToScreen(Point point);

        /// <summary>
        /// Shows this window on screen.
        /// </summary>
        void Show();
        /// <summary>
        /// Hides this window from screen.
        /// </summary>
        void Hide();
    }
    /// <summary>
    /// Object used to get the parameters of the screen.
    /// </summary>
    public interface IScreen : ISize, IEnumerable<IResolution>, IDisposable
    {
        /// <summary>
        /// Returns the resolution of each display
        /// </summary>
        /// <param name="index">Display number</param>
        /// <returns></returns>
        IResolution this[int index] { get; }
        /// <summary>
        /// Returns the number of Displays.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the visible bounds of the Display.
        /// </summary>
        IRectangle Bounds { get; }
        /// <summary>
        /// Returns true if each display is a Mirror of the Primary and this Display is the Primary.
        /// </summary>
        bool IsPrimary { get; }
        /// <summary>
        /// Returns the Resolution object for display.
        /// </summary>
        IResolution Resolution { get; }

        /// <summary>
        /// Change the Resolution of the Display.
        /// </summary>
        /// <param name="resolutionIndex"></param>
        void ChangeResolution(int resolutionIndex);

        /// <summary>
        /// Restore resolution to the previous values before it was changed.
        /// </summary>
        void RestoreResolution();
    }
    /// <summary>
    /// Object that contains the specifications of one Display.
    /// </summary>
    public interface IResolution : ISize
    {
        /// <summary>
        /// True if Screen present.
        /// </summary>
        bool Valid { get; }
        /// <summary>
        /// Defines the visible rectangle that forms the Screen.
        /// </summary>
        IRectangle Bounds { get; }
        /// <summary>
        /// X offset of visible screen.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y offset of visible screen
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Gets the Colour resolution of the Screen.
        /// </summary>
        int BitsPerPixel { get; }
        /// <summary>
        /// Gets the refresh rate of the Display (if relevant)
        /// </summary>
        float RefreshRate { get; }
        /// <summary>
        /// Returns the coulor format used by the display.
        /// </summary>
        uint Format { get; }
    }
    public interface IScreens : IEnumerable<IScreen>
    {
        /// <summary>
        /// Returns an screen information for the indexed display.
        /// </summary>
        /// <param name="index">Display number.</param>
        /// <returns></returns>
        IScreen this[int index] { get; }
        /// <summary>
        /// Returns the number of Displays available.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns Primary Display number. All displays mirror the Primary.
        /// </summary>
        IScreen Primary { get; }
        /// <summary>
        /// Returns IScreen for the Display containing Point (x,y).
        /// Each Display is different in this case.
        /// </summary>
        /// <param name="x">x ordinate of point of interest.</param>
        /// <param name="y">y ordinate of point of interest.</param>
        /// <returns>Display information of the Display containing (X,Y).</returns>
        IScreen FromPoint(int x, int y);
    }
#endif
}
