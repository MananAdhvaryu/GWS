using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    /// <summary>
    /// Represents an object having a unique ID
    /// </summary>
    public interface IStoreable
    {
        /// <summary>
        /// A unique ID. Serves as a key to store or retrieve the object from store.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// This enables control collection to force this object to have an unique ID beause it needs to store this object internally.
        /// </summary>
        void AssignIDIfNone();
    }

    /// <summary>
    /// Replacement of System.ICloneable. 
    /// Represents an object which is capable of cloning iteself.
    /// </summary>
    public interface ICloneable
    {
        /// <summary>
        /// Clones iteself.
        /// </summary>
        /// <returns>Newly made clonned copy</returns>
        object Clone();
    }
    /// <summary>
    /// Represents a pointer of an object to pass about.
    /// </summary>
    public interface IHandle
    {
        /// <summary>
        /// IntPtr value represnts the information of pointer representing an object.
        /// </summary>
        IntPtr Handle { get; }
    }

    /// <summary>
    /// Indicates if an object that can be resized.
    /// </summary>
    public interface IResizeable
    {
        /// <summary>
        /// Resizes an object.
        /// </summary>
        /// <param name="width">the new width of the object.If null is passed or nothing is passed the object will keep the original width.</param>
        /// <param name="height">this new height of object. If null is passed or nothing is passed the object will keep the original height.</param>
        void Resize(int? width = null, int? height = null);
    }

    public interface IMinMaxSizeHolder
    {
        Size MinSize { get; set; }
        Size MaxSize { get; set; }
    }
    /// <summary>
    /// Transparency Colour of an object.
    /// </summary>
    public interface ITransparent
    {
        /// <summary>
        /// The color made transparent. If Null the background colour will become transparent. Set to 0 for opaque.
        /// </summary>
        int? ColorKey { get; set; }
    }

    /// <summary>
    /// Indicates if an object can be moved.
    /// </summary>
    public interface IMoveable
    {
        /// <summary>
        /// Moves an object.
        /// </summary>
        /// <param name="x">the new x cordinate which the object is to be moved to. If null is passed or nothing is passed the object will keep the original x position.</param>
        /// <param name="y">the new y cordinate which the object is to be moved to. If null is passed or nothing is passed the object will keep the original y position.</param>
        void Move(int? x = null, int? y = null);
    }

    /// <summary>
    /// Indicates if an object can receive or lose focus.
    /// </summary>
    public interface IFocus
    {
        /// <summary>
        /// Indicates if the object is currently focused.
        /// </summary>
        bool Focused { get; }

        /// <summary>
        /// Bring focus to the object. Only one object can have focus at a time.
        /// </summary>
        /// <returns></returns>
        bool Focus();
    }

    /// <summary>
    /// Reprsents an object which can receive and lose focus and also can be drawn on a given surface.
    /// </summary>
    public interface IFocusable : IElement, IFocus
    {
        /// <summary>
        /// Indicates the position of an object  relation to the others  a particular collection to receive focus.
        /// </summary>
        int TabIndex { get; set; }
    }

    /// <summary>
    /// Indicates if an object can be shown and hidden as wells as disabled and enabled.
    /// </summary>
    public interface IVisible
    {
        /// <summary>
        /// Gets or sets if an object is visble or not.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets if an object is enabled or not. However, only visible object can be treated enabled if it is enabled for the purpose of receiving inputs.
        /// </summary>
        bool Enabled { get; set; }
    }
    /// <summary>
    /// Represents an object which can be drawn either on top or behind the other objects it overlaps with.
    /// </summary>
    public interface IOVerlap
    {
        /// <summary>
        /// Bring the object upfront so nothing overlaps its area.
        /// </summary>
        void BringToFront();
        /// <summary>
        /// Sends the object to the bottom of the stack of objects so every other one overlaps it.
        /// </summary>
        void SendToBack();

        /// <summary>
        /// Moves the object forwards the specified number of places  the list of objects drawn. The object will now overlap an additional number of objects equalling the specified number
        /// </summary>
        void BringForward(int numberOfPlaces = 1);

        /// <summary>
        /// Moves the object backwards the specified number of places  the list of objects drawn. The object will now be overlapped by a number of objects equalling the specified number.
        /// </summary>
        void SendBackward(int numberOfPlaces = 1);
    }

    /// <summary>
    /// Represents an object which is capable of keeping track of an element with in its collection of child elements for
    /// the purpose of routing user inputs to it so that it can use them.
    /// </summary>
    public interface IIntercative
    {
        /// <summary>
        /// Gets or sets an active object from the perspective of handling user inputs.
        /// </summary>
        IEventUser ActiveObject { get; set; }
    }

     /// <summary>
     /// Represents an object which allowers regualar activitiy on a specific time interval.
     /// </summary>
    public interface ITimer : IStoreable, IDisposable
    {
        /// <summary>
        /// Enables or disables the timer.
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets interval by which the tick event gets fired.
        /// </summary>
        int Interval { get; set; }

        /// <summary>
        /// Specifies if timer is due to fire tick event.
        /// </summary>
        bool Due { get; }

        /// <summary>
        /// Starts this timer.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this timer.
        /// </summary>
        void Stop();

        /// <summary>
        /// Restaart this timer.
        /// </summary>
        void Restart();
        /// <summary>
        /// Reset this timer. Sets its interval sum to zero.
        /// </summary>
        void Reset();
        /// <summary>
        /// Forces a tick event to fire.
        /// </summary>
        void FireEvent();

        /// <summary>
        /// Tick event which gets invoked by the interval specified.
        /// </summary>
        event EventHandler<IEventArgs> Tick;
    }

    /// <summary>
    /// Extended version of System.IDisposeable
    /// Represents an object which can be disposed.
    /// </summary>
    public interface IDispose : IDisposable
    {
        /// <summary>
        /// Lets it be known that it is already disposed.
        /// </summary>
        bool IsDisposed { get; }
    }

    /// <summary>
    /// Represents a very basic object deriving from IElement.
    /// </summary>
    public interface IObject : IStoreable, IElement
#if AdvancedVersion
         , IEventUser, IIntercative
#endif
    {
    }

    /// <summary>
    /// Represents an object which has a screen display capabilites such as window.
    /// </summary>
    public interface IParent : IStoreable, IInvalidater, IBuffer, IUpdatable, IContainer
#if AdvancedVersion
        , IIntercative, IMultiBuffersProvider
#endif
    {
        /// <summary>
        /// True if this object is in fact a window.
        /// </summary>
        bool IsWindow { get; }
        /// <summary>
        /// Collection of popup objects. Please not that elements in this collection take preccedence over the elements of control collection
        /// when it comes to distribute user inputs.
        /// </summary>
        IPopupCollection Popups { get; }
    }

#if AdvancedVersion
    /// <summary>
    /// Represents an object which is in fact a full fledged control. It is afully expanded version of IElement interface.
    /// All the UI controls such as ITextBox, IListBox etc. etc. derives from this.
    /// </summary>
    public interface IControl : IDispose, IObject, IRefreshable, IPoint, ISize, IFocusable, IMoveable, IResizeable, IOVerlap, IParent, IMinMaxSizeHolder
    {
        Point PointToClient(Point point);
        Point PointToScreen(Point point);
        void Show();
        void Hide();
        IParent Parent { get; }
        void BufferChanged(IBuffer buffer, IElementCollection path);
    }
#endif

    /// <summary>
    /// Represents a very basic window which has the capabilty of rendering on screen.
    /// </summary>
    public interface IRenderWindow : IHandle, IParent, IInvalidater, ISize
    {
        /// <summary>
        /// Indicates if this window has a primary texture attached to it or not.
        /// </summary>
        bool HasTexture { get; }
        /// <summary>
        /// Gets a rendererflags by which default renderer for this window is created.
        /// </summary>
        RendererFlags RendererFlags { get; }
    }

    /// <summary>
    /// Represents an object which is associated with a certain render window.
    /// </summary>
    public interface IWindowHolder
    {
        /// <summary>
        /// Window which this object is associated with.
        /// </summary>
        IRenderWindow Window { get; }
    }

    /// <summary>
    /// Represents an object which has a capability to serve as temporary object on screen.
    /// This kind of objects gets preccesedence over other elements when it comes to receiving user inputs.
    /// </summary>
    public interface IPopupObject: IObject, ISize, IDisposable
    {
        /// <summary>
        /// PArent window this popup belogs to.
        /// </summary>
        IParent Window { get; set; }
        /// <summary>
        /// True if this popup is visible.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Gets or sets a default background pen to be used while showing this popup.
        /// </summary>
        IPenContext Background { get; set; }
        /// <summary>
        /// Gets or sets a default foreground pen to be used while drawing a bunch of popup items on screen.
        /// </summary>
        IPenContext Foreground { get; set; }
        /// <summary>
        /// Gets or sets a flag to determine if this popup shoud hide on any item selection.
        /// </summary>
        bool HideOnClick { get; set; }

        /// <summary>
        /// Shows this popup at a location having x and y cordinates specified.
        /// If not specified or null, it takes the coordinates from the current mouse position on screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void Show(int? x = null, int? y = null);
        /// <summary>
        /// Hide this poup.
        /// </summary>
        void Hide();
    }
    /// <summary>
    /// Represents a popup object of certain T type elements.
    /// </summary>
    /// <typeparam name="T">Type of items</typeparam>
    /// <typeparam name="U">EvenArgs class of Argument of type T</typeparam>
    public interface IPopupObject<T, U> : IPopupObject  
    {
        /// <summary>
        /// Find an item on a given mouse coordinates along with the index it is situated in this popup object.
        /// </summary>
        /// <param name="e">Mouse coordinte argmetns</param>
        /// <param name="item">Item found if at all</param>
        /// <param name="index">Inde xof an item found</param>
        /// <returns></returns>
        bool FindItem(IMouseEventArgs e, out T item, out int index);

        /// <summary>
        /// Fires when a mouse hovers on an item.
        /// </summary>
        event EventHandler<U> Hover;
        /// <summary>
        /// Fires when a mouse is clicked on an item.
        /// </summary>
        event EventHandler<U> Click;

    }

    /// <summary>
    /// Represents a single layered popup object. i.e popup without any sub popup.
    /// </summary>
    public interface ISimplePopup : IPopupObject<SimplePopupItem, SimplePopupItemEventArgs>, IEnumerable<SimplePopupItem>
    {
        /// <summary>
        /// Gets or sets a flag indicating if this popup should resize automatically according to the size of its items.
        /// </summary>
        bool AutoSize { get; set; }
        /// <summary>
        /// Location at which this popup is displayed.
        /// </summary>
        Point Location { get; }
        /// <summary>
        /// Gets or set a left margin of its items from the left and top of this popup.
        /// </summary>
        Point LTMargin { get; set; }
        /// <summary>
        /// Gets or sets a min size of this popup.
        /// </summary>
        Size MinSize { get; set; }
        /// <summary>
        /// Gets or sets a font object to draw text of the items.
        /// </summary>
        IFont Font { get; set; }
        /// <summary>
        /// Gets a simple popup item at agiven index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        SimplePopupItem this[int index] { get; }
        /// <summary>
        /// Gets a umbe of items contained in this popup.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get a last clicket item.
        /// </summary>
        SimplePopupItem ClickedItem { get; }
        /// <summary>
        /// Gets an item currently mouse is hovering on.
        /// </summary>
        SimplePopupItem HoveredItem { get; }

        /// <summary>
        /// Copies items to the another item array.
        /// </summary>
        /// <param name="array"> Array the items to be copied to</param>
        /// <param name="arrayIndex">Index from which it should start paste items</param>
        void CopyTo(SimplePopupItem[] array, int arrayIndex);
    }
    /// <summary>
    /// Represents a collection of popup items.
    /// </summary>
    public interface IPopupCollection : IObjectCollection<IPopupObject> { }

    /// <summary>
    /// Represents an animator popup with built-in as well as userdefined animation capabilities.
    /// </summary>
    public interface IAnimator: IPopupObject
    {
        AnimationMode Mode { get; set; }
    }

#if VCSupport
    public interface IToolTipControl : IPopupObject
    {
        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>The display.</value>
        ContentDisplay ContentDisplay { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [display tool tip].
        /// </summary>
        /// <value><c>true</c> if [display tool tip]; otherwise, <c>false</c>.</value>
        bool DisplayToolTip { get; set; }

        /// <summary>
        /// Shows the tool tip.
        /// </summary>
        /// <param name="tipText">The tip text.</param>
        /// <param name="p">The p.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="font">The font.</param>
        void ShowToolTip(string tipText, Point p = default, int? duration = null, IFont font = null);

        /// <summary>
        /// Hides the tool tip.
        /// </summary>
        void HideToolTip();
    }
    public interface IDDControl : IStoreable
    {
        /// <summary>
        /// Gets or sets a value indicating whether [drop down style].
        /// </summary>
        /// <value><c>true</c> if [drop down style]; otherwise, <c>false</c>.</value>
        bool IsDDCStyle { get; set; }

        ///// <summary>
        ///// Gets or sets the size of the collapse.
        ///// </summary>
        ///// <value>The size of the collapse.</value>
        //Size CollapseSize { get; set; }

        IRectangle GapArea { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [dropped down].
        /// </summary>
        /// <value><c>true</c> if [dropped down]; otherwise, <c>false</c>.</value>
        bool DroppedDown { get; set; }

        IRectangle DropDownButton { get; }
    }
    public interface IAreaStyler
    {
        int Width { get; }
        int Height { get; }
        IFillStyle this[FillStyleOf area] { get; }
    }
#endif
}
