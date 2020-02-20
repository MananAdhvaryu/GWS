using System;

namespace MnM.GWS
{
    #region STORABLE OBJECTS
    /// ****<summary>
    /// Represents type of objects that can be stored in a system dictionary.
    /// </summary>
    public enum ObjType
    {
        /// <summary>
        /// Window type objects - deriving from IWindow interface only
        /// </summary>
        Window,
        /// <summary>
        /// Element type objects - deriving from IElement interface only - which includes IShape as well!
        /// </summary>
        Element,
        /// <summary>
        /// Buffer type objects - deriving from IBufferData interface - mainly all the Graphics and Brushes and pens!
        /// </summary>
        Buffer,
        /// <summary>
        /// Only objects which derives from ITimer interface are allowed to be stored under this category.
        /// </summary>
        Timer,
    }
    #endregion

    #region DRAWSTYLE - FILLSTYLE
    /// ***<summary>
    /// Provide options for buffer copying operations - on what to do with destination pixel while overwriting it
    /// for the purpose of copying one memory block to the other.
    /// </summary>
    [Flags]
    public enum DrawMode
    {
        /// <summary>
        /// Normal operation - no check before overwriting destination pixel.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Overwrite pixel only if source pixel is non transparent.
        /// </summary>
        Transparent = 0x10,
        /// <summary>
        /// Overwrite destination pixel with source pixel completely. 
        /// </summary>
        Front = 0x20,
        /// <summary>
        /// Overwrite destination pixel only if it is transparent.
        /// </summary>
        Back = 0x40,
        /// <summary>
        /// Overwrites destination pixel only if it is non transparent and it does not have a background color. i.e if something is drawn there.
        /// </summary>
        Mask = 0x80,
        /// <summary>
        /// Mix colors of source and destination pixels.
        /// </summary>
        MixColor = 0x100,
        /// <summary>
        /// Ignores itersection area of of two shapes and draws the rest only when source pixel is non transparent.
        /// </summary>
        IntersectExclude = 0x200,
        /// <summary>
        /// Writes pixels only between a segment of 2 filled pixels. follows odd-even pattern.
        /// </summary>
        FloodFill = 0x400,
        /// <summary>
        /// Ignores color provided as color key while overwriting destination pixel.
        /// </summary>
        ColorKeyIgnore = 0x800,
        /// <summary>
        /// Not implemented! Sorry.
        /// </summary>
        Animation = 0x1000,
    }

    /// ***<summary>
    /// Represents various option to create a brush to fill a surface.
    /// </summary>
    public enum FillMode
    {
        /// <summary>
        /// Fill the shape as it physically appears with its original area.
        /// </summary>
        Original,
        /// <summary>
        /// If stroke is applied, it fills the inner child shape. Outer shape is non existant.
        /// </summary>
        Inner,
        /// <summary>
        /// If stroke is applied, it draws the pixel boundary between outer and inner child shape.
        /// </summary>
        DrawOutLine,
        /// <summary>
        /// If stroke is applied, it fills the pixel boundary between outer and inner child shape.
        /// </summary>
        FillOutLine,
        /// <summary>
        /// If stroke is applied, it draws the pixel boundary of outer shape and fills inner child shape, giving a fill of hollow shape.
        /// </summary>
        ExceptOutLine,
        /// <summary>
        /// If stroke is applied, it fills the outer shape wholly which results in inner child shape non existant.
        /// </summary>
        Outer,
        /// <summary>
        /// It lets GWS to know that current draw operation is an exercise to wipe the given shape's area.
        /// </summary>
        Erase,
        /// <summary>
        /// It lets GWS to know that the given shape is being drawn with disabled pen.
        /// </summary>
        DisabledDraw,
    }

    /// ***<summary>
    /// Provides scanline fill options while filling a given shape.
    /// </summary>
    [Flags]
    public enum PolyFill
    {
        /// <summary>
        /// Polygon filling using Odd-Even pattern.
        /// </summary>
        OddEven = 0x1,
        /// <summary>
        /// Polygon filling using flood fill
        /// </summary>
        Flood = 0x2,
        /// <summary>
        /// Tells GWS if it has to draw Antialised ends of scan line.
        /// </summary>
        AAEnds = 0x4,
        /// <summary>
        /// Tells GWS it it has to draw ends points of a scan line and skip the portion in between from drawing.
        /// </summary>
        DrawEndsOnly = 0x8,
        /// <summary>
        /// Tells GWS if it has to sort list which contains scan line data.
        /// </summary>
        NoSorting = 0x10
    }

    [Flags]
    public enum StrokeMode
    {
        /// <summary>
        /// Positions stroke so that its middle is the thoeretical border of the shape. 
        /// Both expands the rectangle that contains the shape and shrinks the vissible space inside the stroke.
        /// </summary>
        Middle = 0x0,
        /// <summary>
        /// Positions stroke on the outside of the theoretical border of the shape. 
        /// Expands the rectangle containing the shape but does not change the internal area of the shpe.
        /// </summary>
        Outer = 0x1,
        /// <summary>
        /// Positions stroke so that the theoretical border of the shape is on the outside. 
        /// Shrinks the space inside the shape and maintains the size of the enclosing rectangle.
        /// </summary>
        Inner = 0x2,
    }

    [Flags]
    public enum AfterStroke
    {
        /// <summary>
        /// Do nothing.!!!!
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Normally Strokes are drawn from the second point on the line.
        /// In some cases we need to rest the points that have been removed.
        /// For example: an Arc which must close the ends of the stroke perimiter lines and so the end points are required.
        /// </summary>
        Reset1st = 0x4,
        /// <summary>
        /// Joins the end points of the two lines on the perimeter of the stroke and joins them. 
        /// Example the ends of a stroke drawing an arc must be closed.
        /// </summary>
        JoinEnds = 0x8,
        /// <summary>
        /// Used for Strokes that define a perimiter that has a start and end such as an Arc.
        /// </summary>
        Both = Reset1st | JoinEnds,
    }

    [Flags]
    public enum PointJoin
    {
        /// <summary>
        /// No Join required.!!!!
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Ensures each point is connected to its neighbours.
        /// </summary>
        ConnectEach = 0x1,
        /// <summary>
        /// Only take a point in to account if it is a minimum distance away.
        /// </summary>
        RemoveLast = 0x2,
        /// <summary>
        /// Used in Bezier to prevent errors in stroke drawing.
        /// </summary>
        AvoidTooClose = 0x4,
        /// <summary>
        /// Ignores repeated points and join the remainder.
        /// </summary>
        NoRepeat = 0x8,
        /// <summary>
        /// Connects the end point to the start point.
        /// </summary>
        ConnectEnds = 0x10,
        /// <summary>
        /// Connect each point as long as it is not a repeated point.
        /// </summary>
        CircularJoinOpen = ConnectEach | NoRepeat,
        /// <summary>
        /// Used for closed shapes ignore repeated points
        /// </summary>
        CircularJoin = ConnectEach | ConnectEnds | NoRepeat,
        /// <summary>
        /// Connects end with start point.
        /// </summary>
        PieJoin = CircularJoin,
        /// <summary>
        /// Connects each point but do not connect end to start
        /// </summary>
        ArcJoin = CircularJoinOpen | NoRepeat | RemoveLast,
        /// <summary>
        /// Connect each point and join end to start.
        /// </summary>
        CloseArcJoin = ConnectEach | ConnectEnds,
        /// <summary>
        /// Ensures the firt and last point in the Bezier are not connected.
        /// </summary>
        BezierJoin = ConnectEach | RemoveLast,
        /// <summary>
        /// Joins end point to start point.
        /// </summary>
        PolygonJoin = CircularJoin
    }

    /// <summary>
    /// Defines a mode of animating a shape on regular interval.
    /// </summary>
    [Flags]
    public enum AnimationMode
    {
        /// <summary>
        /// No animation!
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Animates an object by applying different fill mode that GWS provides on sequential manner -i.e. one by one and in cycle.
        /// </summary>
        ByFillMode = 0x1,
        /// <summary>
        /// Animates an object by increasing or descring the size of stroke by unit specified and once reached max level, reversing it with the same unit. 
        /// </summary>
        ByStrokeSize = 0x2,
        /// <summary>
        /// Animates an object by offsetting its location by unit specified and once reached max level, reversing it with the same unit. 
        /// </summary>
        ByLocation = 0x4,
        /// <summary>
        /// Animates an object by applying a rotation by increasing or decreasing a value of an angle specified and once reached 360 - or 0 level, reversing it with the same unit. 
        /// </summary>
        ByRotation = 0x8,
        /// <summary>
        /// Animates an object by applying gradient styles one by one that GWS provides and once it reaches the last option, reversing it one by one. 
        /// </summary>
        ByGradient = 0x10,
        /// <summary>
        /// Animates an object by showing and hiding it on regular interval. 
        /// </summary>
        ByBlinking = 0x20,
        /// <summary>
        /// Animates an object by any other animation that user has specified on regular interval.The user has to provide animation routine here.
        /// </summary>
        UserDefined = 0x40,
    }

    /// <summary>
    /// Enum LineStyle
    /// </summary>
    public enum LineStyle
    {
        /// <summary>
        /// The flat
        /// </summary>
        Flat,
        /// <summary>
        /// The smooth black
        /// </summary>
        SmoothBlack,
        /// <summary>
        /// The absolute black
        /// </summary>
        AbsoluteBlack,
        /// <summary>
        /// The black
        /// </summary>
        Black,
        /// <summary>
        /// The half etched
        /// </summary>
        HalfEtched,
        /// <summary>
        /// The etched
        /// </summary>
        Etched,
        /// <summary>
        /// The color white
        /// </summary>
        ColorWhite,
        /// <summary>
        /// The white color
        /// </summary>
        WhiteColor,
        /// <summary>
        /// The raised smooth
        /// </summary>
        RaisedSmooth,
        /// <summary>
        /// The raised
        /// </summary>
        Raised,
        /// <summary>
        /// The sunken smooth
        /// </summary>
        SunkenSmooth,
        /// <summary>
        /// The bump
        /// </summary>
        Bump,
        /// <summary>
        /// The sunken
        /// </summary>
        Sunken,
        /// <summary>
        /// The no border
        /// </summary>
        NoBorder,
        /// <summary>
        /// The black white
        /// </summary>
        BlackWhite,
        /// <summary>
        /// The white black
        /// </summary>
        WhiteBlack
    }

    public enum FigureCut
    {
        Inner,
        Outer,
    }
    [Flags]
    public enum Gradient
    {
        /// <summary>
        /// Solid fill with first color specified.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Fill changes colour along the horizontal.
        /// </summary>
        Horizontal = 0x1,
        /// <summary>
        /// Fill changes color along the vertical.
        /// </summary>
        Vertical = 0x2,
        /// <summary>
        /// Fill changes color along the forward diagonal
        /// </summary>
        ForwardDiagonal = 0x4,
        /// <summary>
        /// Fill changes color along the backward diagonal.
        /// </summary>
        BackwardDiagonal = 0x8,
        /// <summary>
        /// Fill changes color as it rotates around the centre of the rectangle enclosing the shape. 
        /// The color is symetric along the back diagonal.
        /// Make the start color equal the end color to get smooth transitions.
        /// </summary>
        Central = 0x10,
        /// <summary>
        /// Fill changes color as it rotates around the centre of the rectangle enclosing the shape. 
        /// The color is symetric along the back diagonal and cycles once in each of the for sectors made by the diagnonals.
        /// Make the start color equal the end color to get smooth transitions.
        /// </summary>
        CentralD = 0x20,
        /// <summary>
        /// Fill changes color from Left to right for the part of the shape in the top half of the enclosing rectangle.
        /// Fill goes right to left for bottom half of the enclosing rectangle.
        /// </summary>
        HorizontalSwitch = 0x40,
        /// <summary>
        /// Fill changes colour from left to central vertical and then from right to central vertical.
        /// </summary>
        HorizontalCentral = 0x60,
        /// <summary>
        /// Fill changes colour from top to central horizontal and then from bottom to central horizontal.
        /// </summary>
        VerticalCentral = 0x80,
        /// <summary>
        /// Fill changes colour from top left to central Forward diagonal and then from bottom right to central forward diagonal.
        /// </summary>
        DiagonalCentral = 0x100,
    }

    [Flags]
    public enum LineDraw
    {
        /// <summary>
        /// Line drawn is anti-aliased. !!!!rename to Anti-aliased!!!!
        /// </summary>
        AA = 0x1,
        /// <summary>
        /// Line drawn is not anti-aliased. !!!!rename to Not anti-aliased!!!!
        /// </summary>
        NonAA = 0x2,
        /// <summary>
        /// Line drawn is dotted; alternative pixels are drawn.
        /// </summary>
        Dotted = 0x8,
        /// <summary>
        /// Sets renderers Distinct property true to prevent any pixel in the line being redrawn.
        /// </summary>
        Distinct = 0x10,
        /// <summary>
        /// Exclusively use breshenam line with with integer arithmetic and no nti-aliasing. !!!!rename to BreshenamOnly!!!!
        /// </summary>
        HVBreshenham = 0x20,
    }

    /// <summary>
    /// Enum ImagePosition
    /// </summary>
    public enum ImagePosition
    {
        /// <summary>
        /// The before text
        /// </summary>
        BeforeText = 0x0,
        /// <summary>
        /// The after text
        /// </summary>
        AfterText = 0x1,
        /// <summary>
        /// The above text
        /// </summary>
        AboveText = 0x2,
        /// <summary>
        /// The below text
        /// </summary>
        BelowText = 0x4,
        /// <summary>
        /// The overlay
        /// </summary>
        Overlay = 0x8,
    }

    /// <summary>
    /// Enum ImageDraw
    /// </summary>
    public enum ImageDraw
    {
        /// <summary>
        /// The un scaled
        /// </summary>
        UnScaled = 0x1,
        /// <summary>
        /// The scaled
        /// </summary>
        Scaled = 0x2,
        /// <summary>
        /// The disabled
        /// </summary>
        Disabled = 0x4
    }
    [Flags]
    public enum DrawTextFlags
    {
        /// <summary>
        /// The calculate area
        /// </summary>
        Measure = 0x00000400,
        /// <summary>
        /// The word break
        /// </summary>
        WordBreak = 0x00000010,
        /// <summary>
        /// The text box control
        /// </summary>
        TextBoxControl = 0x00002000,
        /// <summary>
        /// The top
        /// </summary>
        Top = 0x00000000,
        /// <summary>
        /// The left
        /// </summary>
        Left = 0x00000000,
        /// <summary>
        /// The horizontal center
        /// </summary>
        HorizontalCenter = 0x00000001,
        /// <summary>
        /// The right
        /// </summary>
        Right = 0x00000002,
        /// <summary>
        /// The vertical center
        /// </summary>
        VerticalCenter = 0x00000004,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = 0x00000008,
        /// <summary>
        /// The single line
        /// </summary>
        SingleLine = 0x00000020,
        /// <summary>
        /// The expand tabs
        /// </summary>
        ExpandTabs = 0x00000040,
        /// <summary>
        /// The tab stop
        /// </summary>
        TabStop = 0x00000080,
        /// <summary>
        /// The no clipping
        /// </summary>
        NoClipping = 0x00000100,
        /// <summary>
        /// The external leading
        /// </summary>
        ExternalLeading = 0x00000200,
        /// <summary>
        /// The no prefix
        /// </summary>
        NoPrefix = 0x00000800,
        /// <summary>
        /// The internal
        /// </summary>
        Internal = 0x00001000,
        /// <summary>
        /// The path ellipsis
        /// </summary>
        PathEllipsis = 0x00004000,
        /// <summary>
        /// The end ellipsis
        /// </summary>
        EndEllipsis = 0x00008000,
        /// <summary>
        /// The word ellipsis
        /// </summary>
        WordEllipsis = 0x00040000,
        /// <summary>
        /// The modify string
        /// </summary>
        ModifyString = 0x00010000,
        /// <summary>
        /// The right to left
        /// </summary>
        RightToLeft = 0x00020000,
        /// <summary>
        /// The no full width character break
        /// </summary>
        NoFullWidthCharacterBreak = 0x00080000,
        /// <summary>
        /// The hide prefix
        /// </summary>
        HidePrefix = 0x00100000,
        /// <summary>
        /// The prefix only
        /// </summary>
        PrefixOnly = 0x00200000,
        /// <summary>
        /// The no padding
        /// </summary>
        NoPadding = 0x10000000,
    }

    public enum TextBreaker
    {
        None,
        Word,
        Line,
        SingleWord
    }

    /// <summary>
    /// Enum CaseConversion
    /// </summary>
    public enum CaseConversion
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The title
        /// </summary>
        Title,
        /// <summary>
        /// The sentence
        /// </summary>
        Sentence,
        /// <summary>
        /// The upper
        /// </summary>
        Upper,
        /// <summary>
        /// The lower
        /// </summary>
        Lower,
        /// <summary>
        /// The toggle
        /// </summary>
        Toggle
    }

    /// <summary>
    /// Enum FormatType
    /// </summary>
    public enum FormatType
    {
        /// <summary>
        /// The array
        /// </summary>
        Array,
        /// <summary>
        /// The normal
        /// </summary>
        Normal,
        /// <summary>
        /// The group
        /// </summary>
        Group,

        VirtualTextBox,

        TextBox
    }

    /// <summary>
    /// Enum WordSelection
    /// </summary>
    public enum WordSelection
    {
        /// <summary>
        /// The current
        /// </summary>
        Current,
        /// <summary>
        /// The previous
        /// </summary>
        Previous = -1,
        /// <summary>
        /// The next
        /// </summary>
        Next = 1,
    }

    /// <summary>
    /// Enum ValidateType
    /// </summary>
    public enum ValidateType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Allows date  dd-MM-yyyy format
        /// </summary>
        ddMMyyyy = 0x1,
        /// <summary>
        /// Allows date  dd-MM-yy format
        /// </summary>
        ddMMyy = 0x2,
        /// <summary>
        /// Allows date  MM-dd-yyyy format
        /// </summary>
        MMddyyyy = 0x4,
        /// <summary>
        /// Allows date  MM-dd-yy format
        /// </summary>
        MMddyy = 0x8,
        /// <summary>
        /// Allows date  yyyy-MM-dd format
        /// </summary>
        yyyyMMdd = 0x10,
        /// <summary>
        /// Allows date  yy-MM-dd format
        /// </summary>
        yyMMdd = 0x20,
        /// <summary>
        /// Allows time  HH:mm format example 23:59
        /// </summary>
        HHmm = 0x40,
        /// <summary>
        /// Allows time  HH:mm format with AM/PM example 11:59 PM
        /// </summary>
        hhmm12 = 0x80,
        /// <summary>
        /// Allows time  HH:mm:SS format example 23:59:23
        /// </summary>
        HHmmss = 0x100,
        /// <summary>
        /// Allows time  HH:mm:SS format with AM/PM example 11:59:23 PM
        /// </summary>
        hhmmss12 = 0x200,
        /// <summary>
        /// Allows time  HH:mm:SS:MS with miliseconds format example 23:59:23:99
        /// </summary>
        HHmmssms = 0x400,
        /// <summary>
        /// The HHMMSSMS12
        /// </summary>
        hhmmssms12 = 0x800,
        /// <summary>
        /// The dd m myyyy h HMM
        /// </summary>
        ddMMyyyyHHmm = ddMMyyyy | HHmm,
        /// <summary>
        /// The dd m myyyyhhmm12
        /// </summary>
        ddMMyyyyhhmm12 = ddMMyyyy | hhmm12,
        /// <summary>
        /// The dd m myyyy h HMMSS
        /// </summary>
        ddMMyyyyHHmmss = ddMMyyyy | HHmmss,
        /// <summary>
        /// The dd m myyyyhhmmss12
        /// </summary>
        ddMMyyyyhhmmss12 = ddMMyyyy | hhmmss12,
        /// <summary>
        /// The dd m myy h HMM
        /// </summary>
        ddMMyyHHmm = ddMMyy | HHmm,
        /// <summary>
        /// The dd m myyhhmm12
        /// </summary>
        ddMMyyhhmm12 = ddMMyy | hhmm12,
        /// <summary>
        /// The dd m myy h HMMSS
        /// </summary>
        ddMMyyHHmmss = ddMMyy | HHmmss,
        /// <summary>
        /// The dd m myyhhmmss12
        /// </summary>
        ddMMyyhhmmss12 = ddMMyy | hhmmss12,
        /// <summary>
        /// The yyyy m MDD h HMM
        /// </summary>
        yyyyMMddHHmm = yyyyMMdd | HHmm,
        /// <summary>
        /// The yyyy m MDDHHMM12
        /// </summary>
        yyyyMMddhhmm12 = yyyyMMdd | hhmm12,
        /// <summary>
        /// The yyyy m MDD h HMMSS
        /// </summary>
        yyyyMMddHHmmss = yyyyMMdd | HHmmss,
        /// <summary>
        /// The yyyy m MDDHHMMSS12
        /// </summary>
        yyyyMMddhhmmss12 = yyyyMMdd | hhmmss12,
        /// <summary>
        /// Allows characters only. a to z or A to Z
        /// </summary>
        Character = 0x1000,
        /// <summary>
        /// Allows numbers only
        /// </summary>
        Number = 0x2000,
        /// <summary>
        /// Allows characters and numbers only  a to z or A to Z or 0 to 9
        /// </summary>
        CharacterAndNumber = Character | Number,
        /// <summary>
        /// Allows anything except numbers i.e. 0 to 9 can not be written
        /// </summary>
        NotNumber = 0x4000,
        /// <summary>
        /// Allows positive intiger numbers only
        /// </summary>
        PositiveNumber = 0x8000,
        /// <summary>
        /// Allows decimal number only
        /// </summary>
        DecimalNumber = 0x10000,
        /// <summary>
        /// Allows positive decimal numbers only
        /// </summary>
        PositiveDecimalNumber = PositiveNumber | DecimalNumber,
        /// <summary>
        /// Allows numbers with + or - signs
        /// </summary>
        NumberWithPlusAndMinus = 0x20000,
        /// <summary>
        /// Allows number with +,-,/,*,% signs
        /// </summary>
        NumberWithCalculatingSigns = 0x40000,
        /// <summary>
        /// Allows true, false, 0 and -1 only
        /// </summary>
        Boolean = 0x80000
    }

    public enum ContentAlignment
    {
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 4,
        MiddleLeft = 16,
        MiddleCenter = 32,
        MiddleRight = 64,
        BottomLeft = 256,
        BottomCenter = 512,
        BottomRight = 1024
    }
    /// <summary>
    /// Enum SignOption
    /// </summary>
    public enum SignOption
    {
        /// <summary>
        /// The CheckBox
        /// </summary>
        CheckBox = 0x1,
        /// <summary>
        /// The option box
        /// </summary>
        OptionBox = 0x2,
    }
    #endregion

    #region READ CONTEXT
    /// <summary>
    /// Provide type of entity in question - if it is of type reader such as pens, brushes, colors etc. or Writer such as graphics, surfaces, buffers etc.
    /// </summary>
    public enum Entity
    {
        /// <summary>
        /// Represents brushes, pens and colors which you can read data from.
        /// </summary>
        BufferPen, 
        /// <summary>
        /// Represents  graphics, surfaces, buffers which you can write data to.
        /// </summary>
        Buffer,
    }
    #endregion

    #region EVENTS
    /// <summary>
    /// Represents current status of how an event is handled by a given control placed in a hierarchy of controls which are part of a control collection.
    /// </summary>
    public enum EventUseStatus
    {
        /// <summary>
        /// Specifies that the event is not used by the control at all.
        /// </summary>
        NotUsed = 0x1,
        /// <summary>
        /// specifies that the event is used by the control.
        /// </summary>
        Used = 0x2,
        /// <summary>
        /// Specifies that although, it is a relevant event for the control, it was not able to use it for various reasons for example because it is disabled.
        /// </summary>
        NotAbleToUse = 0x4,
        /// <summary>
        /// Specifies that although the control is enabled to respond to the event, it is not relevent to the control on a functional level.
        /// </summary>
        CanUseButNotRelevant = 0x8,
        /// <summary>
        /// Specifies that although the event was used by the control, it does not indicates GWS to stop passing it on the next control in hierarchy.
        /// </summary>
        UsedButOkToReprocess = 0x10,
        /// <summary>
        /// Specifies that the control has indicated that there should be no further event has to be sent to it for variuos reasons say necause its invisible or indisposed or disabled.
        /// </summary>
        StopSendingMore = 0x20,
    }
    public enum MouseState
    {
        None = 0x0,
        Up = 0x1,
        Down = 0x2,
        Move = 0x4,
        Click = 0x8,
        DoubleClick = 0x10,
        Wheel = 0x20,
        Enter = 0x40,
        Leave = 0x80,
        DragBegin = 0x100,
        Drag = DragBegin | 0x200,
        DragEnd = Drag | 0x400
    }
    public enum KeyState
    {
        None = 0x0,
        Up = 0x1,
        Down = 0x2,
        Preview = Down | 0x4
    }
    public enum State
    {
        Released = 1,
        Pressed = 2,
    }
    public enum Key
    {
        Modifiers = -65536,
        None = 0,
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        XButton1 = 5,
        XButton2 = 6,
        Back = 8,
        Tab = 9,
        LineFeed = 10,
        Clear = 12,
        Return = 13,
        Enter = 13,
        ShiftKey = 16,
        ControlKey = 17,
        Menu = 18,
        Pause = 19,
        Capital = 20,
        CapsLock = 20,
        KanaMode = 21,
        HanguelMode = 21,
        HangulMode = 21,
        JunjaMode = 23,
        FinalMode = 24,
        HanjaMode = 25,
        KanjiMode = 25,
        Escape = 27,
        IMEConvert = 28,
        IMENonconvert = 29,
        IMEAccept = 30,
        IMEAceept = 30,
        IMEModeChange = 31,
        Space = 32,
        Prior = 33,
        PageUp = 33,
        Next = 34,
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LWin = 91,
        RWin = 92,
        Apps = 93,
        Sleep = 95,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LShiftKey = 160,
        RShiftKey = 161,
        LControlKey = 162,
        RControlKey = 163,
        LMenu = 164,
        RMenu = 165,
        BrowserBack = 166,
        BrowserForward = 167,
        BrowserRefresh = 168,
        BrowserStop = 169,
        BrowserSearch = 170,
        BrowserFavorites = 171,
        BrowserHome = 172,
        VolumeMute = 173,
        VolumeDown = 174,
        VolumeUp = 175,
        MediaNextTrack = 176,
        MediaPreviousTrack = 177,
        MediaStop = 178,
        MediaPlayPause = 179,
        LaunchMail = 180,
        SelectMedia = 181,
        LaunchApplication1 = 182,
        LaunchApplication2 = 183,
        OemSemicolon = 186,
        Oem1 = 186,
        Oemplus = 187,
        Oemcomma = 188,
        OemMinus = 189,
        OemPeriod = 190,
        OemQuestion = 191,
        Oem2 = 191,
        OemTilde = 192,
        Oem3 = 192,
        OemOpenBrackets = 219,
        Oem4 = 219,
        OemPipe = 220,
        Oem5 = 220,
        OemCloseBrackets = 221,
        Oem6 = 221,
        OemQuotes = 222,
        Oem7 = 222,
        Oem8 = 223,
        OemSlash = 225,
        OemBackslash = 226,
        Oem102 = 226,
        ProcessKey = 229,
        Packet = 231,
        Attn = 246,
        Crsel = 247,
        Exsel = 248,
        EraseEof = 249,
        Play = 250,
        Zoom = 251,
        NoName = 252,
        Pa1 = 253,
        OemClear = 254,
        KeyCode = 65535,
        Shift = 65536,
        Control = 131072,
        Alt = 262144,
        LAlt = Alt | 1000,
        RALT = Alt | 2000,
        LastKey = 196,
    }

    public enum MouseButton
    {
        None = 0,
        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left = 1,
        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle,
        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right,
        /// <summary>
        /// The first extra mouse button.
        /// </summary>
        Button1,
        /// <summary>
        /// The second extra mouse button.
        /// </summary>
        Button2,
        /// <summary>
        /// The third extra mouse button.
        /// </summary>
        Button3,
        /// <summary>
        /// The fourth extra mouse button.
        /// </summary>
        Button4,
        /// <summary>
        /// The fifth extra mouse button.
        /// </summary>
        Button5,
        /// <summary>
        /// The sixth extra mouse button.
        /// </summary>
        Button6,
        /// <summary>
        /// The seventh extra mouse button.
        /// </summary>
        Button7,
        /// <summary>
        /// The eigth extra mouse button.
        /// </summary>
        Button8,
        /// <summary>
        /// The ninth extra mouse button.
        /// </summary>
        Button9,
        /// <summary>
        /// Indicates the last available mouse button.
        /// </summary>
        LastButton
    }
    public enum WindowEventID : byte
    {
        NONE,
        SHOWN,
        HIDDEN,
        EXPOSED,
        MOVED,
        RESIZED,
        SIZE_CHANGED,
        MINIMIZED,
        MAXIMIZED,
        RESTORED,
        ENTER,
        LEAVE,
        FOCUS_GAINED,
        FOCUS_LOST,
        CLOSE,
    }
    public enum GwsEvent
    {
        FIRSTEVENT = 0,
        QUIT,
        WINDOWEVENT,
        SYSWMEVENT,
        KEYDOWN,
        KEYUP,
        TEXTINPUT,
        MOUSEMOTION,
        MOUSEBUTTONDOWN,
        MOUSEBUTTONUP,
        MOUSEWHEEL,

        JOYAXISMOTION,
        JOYBALLMOTION,
        JOYHATMOTION,
        JOYBUTTONDOWN,
        JOYBUTTONUP,
        JOYDEVICEADDED,
        JOYDEVICEREMOVED,

        CONTROLLERAXISMOTION,
        CONTROLLERBUTTONDOWN,
        CONTROLLERBUTTONUP,
        CONTROLLERDEVICEADDED,
        CONTROLLERDEVICEREMOVED,
        CONTROLLERDEVICEREMAPPED,

        FINGERDOWN,
        FINGERUP,
        FINGERMOTION,
        DOLLARGESTURE,
        DOLLARRECORD,
        MULTIGESTURE,
        CLIPBOARDUPDATE,
        DROPFILE,
        USEREVENT,

        DROPTEXT,
        DROPBEGIN,
        DROPCOMPLETE,
        AUDIODEVICEADDED,
        AUDIODEVICEREMOVED,
        RENDER_TARGETS_RESET,
        RENDER_DEVICE_RESET,

        SHOWN,
        HIDDEN,
        EXPOSED,
        MOVED,
        RESIZED,
        SIZE_CHANGED,
        MINIMIZED,
        MAXIMIZED,
        RESTORED,
        ENTER,
        LEAVE,
        FOCUS_GAINED,
        FOCUS_LOST,
        CLOSE,
        LASTEVENT,
    }

    /// <summary>
    /// Enumerates available <see cref="GamePad"/> types.
    /// </summary>
    public enum GamePadType
    {
        /// <summary>
        /// The <c>GamePad</c> is of an unknown type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The <c>GamePad</c> is an arcade stick.
        /// </summary>
        ArcadeStick,

        /// <summary>
        /// The <c>GamePad</c> is a dance pad.
        /// </summary>
        DancePad,

        /// <summary>
        /// The <c>GamePad</c> is a flight stick.
        /// </summary>
        FlightStick,

        /// <summary>
        /// The <c>GamePad</c> is a guitar.
        /// </summary>
        Guitar,

        /// <summary>
        /// The <c>GamePad</c> is a driving wheel.
        /// </summary>
        Wheel,

        /// <summary>
        /// The <c>GamePad</c> is an alternate guitar.
        /// </summary>
        AlternateGuitar,

        /// <summary>
        /// The <c>GamePad</c> is a big button pad.
        /// </summary>
        BigButtonPad,

        /// <summary>
        /// The <c>GamePad</c> is a drum kit.
        /// </summary>
        DrumKit,

        /// <summary>
        /// The <c>GamePad</c> is a game pad.
        /// </summary>
        GamePad,

        /// <summary>
        /// The <c>GamePad</c> is an arcade pad.
        /// </summary>
        ArcadePad,

        /// <summary>
        /// The <c>GamePad</c> is a bass guitar.
        /// </summary>
        BassGuitar,
    }

    /// <summary>
    /// Enumerates discrete positions for a joystick hat.
    /// </summary>
    public enum HatLocation : byte
    {
        /// <summary>
        /// The hat is  its centered (neutral) position
        /// </summary>
        Centered = 0,
        /// <summary>
        /// The hat is  its top position.
        /// </summary>
        Up,
        /// <summary>
        /// The hat is  its top-right position.
        /// </summary>
        UpRight,
        /// <summary>
        /// The hat is  its right position.
        /// </summary>
        Right,
        /// <summary>
        /// The hat is  its bottom-right position.
        /// </summary>
        DownRight,
        /// <summary>
        /// The hat is  its bottom position.
        /// </summary>
        Down,
        /// <summary>
        /// The hat is  its bottom-left position.
        /// </summary>
        DownLeft,
        /// <summary>
        /// The hat is  its left position.
        /// </summary>
        Left,
        /// <summary>
        /// The hat is  its top-left position.
        /// </summary>
        UpLeft,
    }

    /// <summary>
    /// The type of the input device.
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// Device is a keyboard.
        /// </summary>
        Keyboard,
        /// <summary>
        /// Device is a mouse.
        /// </summary>
        Mouse,
        /// <summary>
        /// Device is a Human Interface Device. Joysticks, joypads, pens
        /// and some specific usb keyboards/mice fall into this category.
        /// </summary>
        Hid
    }

    /// <summary>
    /// Defines available Joystick hats.
    /// </summary>
    public enum JoystickHat
    {
        /// <summary>
        /// The first hat of the Joystick device.
        /// </summary>
        Hat0,
        /// <summary>
        /// The second hat of the Joystick device.
        /// </summary>
        Hat1,
        /// <summary>
        /// The third hat of the Joystick device.
        /// </summary>
        Hat2,
        /// <summary>
        /// The fourth hat of the Joystick device.
        /// </summary>
        Hat3,
        /// <summary>
        /// The last hat of the Joystick device.
        /// </summary>
        Last = Hat3
    }

    public enum GameControllerAxis : byte
    {
        Invalid = 0xff,
        LeftX = 0,
        LeftY,
        RightX,
        RightY,
        TriggerLeft,
        TriggerRight,
        Max
    }
    public enum GameControllerButton : byte
    {
        INVALID = 0xff,
        A = 0,
        B,
        X,
        Y,
        BACK,
        GUIDE,
        START,
        LEFTSTICK,
        RIGHTSTICK,
        LEFTSHOULDER,
        RIGHTSHOULDER,
        DPAD_UP,
        DPAD_DOWN,
        DPAD_LEFT,
        DPAD_RIGHT,
        Max
    }
    public enum JoystickPowerLevel
    {
        Unknown = -1,
        Empty,
        Low,
        Medium,
        Full,
        Wired,
        Max
    }

    public enum JoystickType
    {
        Unknown,
        GameController,
        Wheel,
        Stick,
        FlightStick,
        DancePad,
        Guitar,
        DrumKit,
        ArcadePad
    }
    #endregion

    #region FONT
    [Flags]
    public enum FontStyle
    {
        Regular = 0x00,
        StrikeThrough = 0x01,
        Underline = 0x02,
        Bold = 0x04,
        Italic = 0x08,
        Oblique = 0x10,
    }

    public enum FontHint
    {
        Normal,
        Light,
        Mono,
        None
    }

    /// <summary>
    /// Specifies various font weights.
    /// </summary>
    public enum FontWeight
    {
        /// <summary>
        /// The weight is unknown or unspecified.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Very thin.
        /// </summary>
        Thin = 100,

        /// <summary>
        /// Extra light.
        /// </summary>
        ExtraLight = 200,

        /// <summary>
        /// Light.
        /// </summary>
        Light = 300,

        /// <summary>
        /// Normal.
        /// </summary>
        Normal = 400,

        /// <summary>
        /// Medium.
        /// </summary>
        Medium = 500,

        /// <summary>
        /// Somewhat bold.
        /// </summary>
        SemiBold = 600,

        /// <summary>
        /// Bold.
        /// </summary>
        Bold = 700,

        /// <summary>
        /// Extra bold.
        /// </summary>
        ExtraBold = 800,

        /// <summary>
        /// Extremely bold.
        /// </summary>
        Black = 900
    }

    /// <summary>
    /// Specifies the font stretching level.
    /// </summary>
    public enum FontStretch
    {
        /// <summary>
        /// The stretch is unknown or unspecified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Ultra condensed.
        /// </summary>
        UltraCondensed,

        /// <summary>
        /// Extra condensed.
        /// </summary>
        ExtraCondensed,

        /// <summary>
        /// Condensed.
        /// </summary>
        Condensed,

        /// <summary>
        /// Somewhat condensed.
        /// </summary>
        SemiCondensed,

        /// <summary>
        /// Normal.
        /// </summary>
        Normal,

        /// <summary>
        /// Somewhat expanded.
        /// </summary>
        SemiExpanded,

        /// <summary>
        /// Expanded.
        /// </summary>
        Expanded,

        /// <summary>
        /// Extra expanded.
        /// </summary>
        ExtraExpanded,

        /// <summary>
        /// Ultra expanded.
        /// </summary>
        UltraExpanded
    }

    [Flags]
    public enum TextStyle
    {
        None,
        Strikeout = FontStyle.StrikeThrough,
        Underline = FontStyle.Underline,
        Both = Strikeout | Underline,
        OutLine = 0x10,
    }

    public enum BreakDelimiter
    {
        None,
        Character,
        Word
    }
    #endregion

    #region IMAGE - CANVAS - TEXTURE
    [Flags]
    public enum GraphicsMode
    {
        SingleBuffer = 0x1,
        DoubleBuffer = 0x2,
        DiscardChanges = 0x4,
        CommitChanges = 0x8,
    }

    [Flags]
    public enum RendererFlags : uint
    {
        Default = 0x0000000,
        Software = 0x00000001,
        Accelarated = 0x00000002,
        PresentSync = 0x00000004,
        Texture = 0x00000008
    }

    [Flags]
    public enum RendererFlip
    {
        None = 0x00000000,
        Horizontal = 0x00000001,
        Vertical = 0x00000002
    }

    public enum RenderingHint
    {
        Nearest = 0,
        Liner = 1,
        Best = 2,
    }

    [Flags]
    public enum BlendMode
    {
        None = 0x00000000,
        Blend = 0x00000001,
        Add = 0x00000002,
        Mod = 0x00000004,
        Invalid = 0x7FFFFFFF
    }

    public enum TextureAccess
    {
        Static,
        Streaming,
        Target,
    }

    [Flags]
    public enum TextureModulate
    {
        None = 0x00000000,
        Horizontal = 0x00000001,
        Vertical = 0x00000002
    }

    public enum Interpolation
    {
        NearestNeighbor,
        BiLinear,
        Bicubic,
    }
    #endregion

    #region IMAGE PROCESSING
    public enum ImageFormat
    {
        JPG = 2,
        BMP = 0,
        HDR = 3,
        TGA = 4,
    }
    #endregion

    #region PRIMITIVES
    [Flags]
    public enum LineRotation
    {
        Normal = 0x0,
        Positive = 0x1,
        MaintainLength = Positive | 0x2,
    }
    public enum Ends
    {
        NoDraw = 0x0,
        Draw = 0x1,
    }

    [Flags]
    public enum LineSkip
    {
        None = 0x0,
        NonSteep = 0x1,
        Steep = 0x2,
        Both = NonSteep | Steep
    }
    #endregion

    #region SHAPES
    [Flags]
    public enum ShapeType
    {
        Rectangle = 0x0,
        Triangle = 0x1,
        Trapazoid = 0x2,

        Polygon = 0x4,
        Ellipse = 0x8,
        Circle = Ellipse | 0x10,
        Pie = 0x20,
        Arc = 0x40,
        BezierArc = 0x80,
        BezierPie = 0x100,
        Bezier = 0x200,
        Line = 0x400
    }
    public enum BezierType
    {
        Cubic = 0x0,
        Multiple = 0x1,
    }
    public enum SideDraw
    {
        None = 0,
        First = 0x1,
        Second = 0x2,
        Third = 0x4,
        Fourth = 0x8,
        All = First | Second | Third | Fourth,
    }

    [Flags]
    public enum CurveType
    {
        Full = 0x0,
        Arc = 0x1,
        Pie = 0x2,
        NoSweepAngle = 0x4,
        NegativeMotion = 0x8,
        ClosedArc = Arc | 0x10,
        FillCorrect = Pie | 0x20,
        CrossStroke = 0x40,
        RotateCut = 0x80
    }
    #endregion

    #region WINDOWS - VIRTUAL CONTROLS
    [Flags]
    public enum GwsWindowFlags
    {
        None = 0x0,
        Resizable = 0x1,
        FullScreen = 0x2,
        OpenGL = 0x4,
        Shown = 0x8,
        Hidden = 0x10,
        NoBorders = 0x20,
        Minimized = 0x40,
        Maximized = 0x80,
        GrabInput = 0x100,
        InputFocus = 0x200,
        MouseFocus = 0x400,
        FullScreenDesktop = 0x800,
        Foreign = 0x1000,
        HighDPI = 0x2000,
    }

    [Flags]
    public enum OS
    {
        None = 0x0,
        Windows = 0x1,
        Android = 0x2,
        IPhone = 0x4,
        Linux = 0x8,
        MacOsx = 0x10,
        IOS = 0x20,
        X86 = 0x40,
        X64 = 0x80,
        Arm = 0x100,
    }

    [Flags]
    public enum CaretState
    {
        Right = 0x0,
        Left = 0x1,
        Mouse = 0x2,
        Key = 0x4,
        Selection = 0x8,
        WordSelection = 0x10,
        Backward = 0x20,
        Forward = 0x40,
        Horizontal = 0x80,
        Vertical = 0x100,
        SelectionClear = 0x200,

        MouseDrag = Mouse | 0x400,
        MouseProxy = Mouse | 0x800,
        MouseDirect = Mouse | 0x1000,

        KeyLeft = Key | Horizontal | Backward | 0x2000,
        KeyRight = Key | Horizontal | Forward | 0x4000,
        KeyUp = MouseProxy | Vertical | Backward | 0x8000,
        KeyDn = MouseProxy | Vertical | Forward | 0x10000,
        KeyPgUp = MouseProxy | Vertical | Backward | 0x20000,
        KeyPgDn = MouseProxy | Vertical | Forward | 0x40000,
        KeyHome = Key | Vertical | Backward | 0x80000,
        KeyEnd = Key | Vertical | Forward | 0x100000,

        XForward = Mouse | Horizontal | Forward,
        XBackward = Mouse | Horizontal | Backward,
        YForward = Mouse | Vertical | Forward,
        YBackward = Mouse | Vertical | Backward,
    }
    public enum InvokeType
    {
        Method,
        Event
    }
    public enum ElementType
    {
        Button,
        Object,
        Expander,
        Control,
        Container,
        Form
    }
    public enum Bar
    {
        PageBar,
        DataBar,
        ScrollBar,
        UpDown,
        MenuBar,
        CustomBar,
        Frame,
    }
    public enum RTL
    {
        //
        // Summary:
        //     The text reads from left to right. This is the default.

        No = 0,
        //
        // Summary:
        //     The text reads from right to left.

        Yes = 1,
        //
        // Summary:
        //     The direction the text read is inherited from the parent control.

        Inherit = 2
    }
    public enum ProgressBarStyle
    {
        // Summary:
        //     Indicates progress by increasing the number of segmented blocks  a ProgressBar.
        Blocks = 0,
        //
        // Summary:
        //     Indicates progress by increasing the size of a smooth, continuous bar in
        //     a ProgressBar.
        Continuous = 1,
        //
        // Summary:
        //     Indicates progress by continuously scrolling a block across a ProgressBar
        //      a marquee fashion.
        Marquee = 2,
        MarqeeBlocks = 3,
    }
    public enum DateLimitBehevior
    {
        Fix,
        Floating
    }
    public enum CalendarButton
    {
        Month,
        Year,
        Day,
        Date,
        ToDay,
        Holiday,
        Schedule,
        Week
    }
    public enum DateElement
    {
        Previous,
        Min,
        Max
    }
    public enum Interval
    {
        Daily,
        Monthly,
        Yearly
    }
    public enum DateBands
    {
        Date,
        Day,
        Month,
        Year,
        Hour,
        Minute,
        Second
    }
    public enum DateFormat
    {
        // Summary:
        //     The MnM.VcFramework.DatePicker control displays the date/time value
        //      the long date format set by the user's operating system.
        Long = 1,
        //
        // Summary:
        //     The MnM.VcFramework.DatePicker control displays the date/time value
        //      the short date format set by the user's operating system.
        Short = 0,
        //
        // Summary:
        //     The MnM.VcFramework.DatePicker control displays the date/time value
        //      the time format set by the user's operating system.

        ShortTime = 4,

        LongTime = 5,

        Scientific = 6,
        //
        // Summary:
        //     The MnMGUI.Controls.Calendar control displays the date/time value
        //      a custom format. For more information, see MnMGUI.Controls.Calendar.CustomFormat.

        Custom = 8,
    }
    public enum BoxType
    {
        CheckBox,
        OptionBox,
    }
    public enum BoxState
    {
        None,
        True,
        False
    }
    public enum UpdownStyle
    {
        Both,
        UpDown,
        Slider,
    }
    public enum Docking
    {
        //
        // Summary:
        //     The control is not docked.
        None = 0,
        //
        // Summary:
        //     The control's top edge is docked to the top of its containing control.
        Top = 1,
        //
        // Summary:
        //     The control's bottom edge is docked to the bottom of its containing control.
        Bottom = 2,
        //
        // Summary:
        //     The control's left edge is docked to the left edge of its containing control.
        Left = 3,
        //
        // Summary:
        //     The control's right edge is docked to the right edge of its containing control.
        Right = 4,
        //
        // Summary:
        //     All the control's edges are docked to the all edges of its containing control
        //     and sized appropriately.
        Fill = 5
    }
    public enum Anchoring
    {
        //
        // Summary:
        //     The control is not anchored to any edges of its container.
        None = 0,
        //
        // Summary:
        //     The control is anchored to the top edge of its container.
        Top = 1,
        //
        // Summary:
        //     The control is anchored to the bottom edge of its container.
        Bottom = 2,
        //
        // Summary:
        //     The control is anchored to the left edge of its container.
        Left = 4,
        //
        // Summary:
        //     The control is anchored to the right edge of its container.
        Right = 8
    }
    public enum GridEditMode
    {
        EntryForm,
        InLine,
    }
    public enum GridFillOption
    {
        Page,
        Row,
        Cell,
    }
    public enum GridColumnType
    {
        EditBox,
        Legend,
        Date,
        List,
        CheckBox,
        Button,
        ProgressBar,
        UpDown,
        Numeric,
        Float,
        Image,
        HyperLink,
        Chart,
    }
    public enum ScrollButtons
    {
        First,
        Previous,
        Next,
        Last,
        Bar,
        Slider
    }
    public enum ScrollState
    {
        None,
        Continuous
    }
    public enum PageButtons
    {
        Previous = -1,
        None = 0,
        Next = 1,
        First,
        Last,
        Bar,
        Add = -2,
        Edit = -3,
        Delete = -4,
        Ok = -5,
        Cancel = -6,
        Refresh = -7,
        Requery = -8
    }
    public enum Navigation
    {
        None,
        Up = Key.Up,
        Down = Key.Down,
        Left = Key.Left,
        Right = Key.Right,
        Home = Key.Home,
        End = Key.End,
        PageUp = Key.PageUp,
        PageDown = Key.PageDown,
    }
    public enum TextBinding
    {
        TextFile,
        DataTable,
        None,
    }
    public enum LineBorder
    {
        None,
        All,
        Blinking,
        Highlight
    }
    public enum CaretType
    {
        HairLine,
        Rectangle,
    }
    public enum TextEditMode
    {
        Free,
        Programatically,
        ReadOnly,
    }
    public enum ValidateMode
    {
        Single,
        Multiple
    }
    public enum ResizeBehaviour
    {
        All,
        Single,
        None
    }
    public enum DragDropEffect
    {
        //
        // Summary:
        //     The drop target does not accept the data.
        None = 0,
        //
        // Summary:
        //     The data from the drag source is copied to the drop target.
        Copy = 1,
        //
        // Summary:
        //     The data from the drag source is moved to the drop target.
        Move = 2,
        //
        // Summary:
        //     The data from the drag source is linked to the drop target.
        Link = 4,
    }
    public enum DragType
    {
        ContentMove,
        ObjecMove,
        ObjectResize,
    }
    public enum DDCStyle
    {
        None,
        DDCEditable,
        DDCReadOnly,
    }
    public enum ItemDrawState
    {
        Default,
        AlternateDefault,
        Selected,
        Checked,
        UnChecked
    }
    public enum StylePattern
    {

        Default = 0x1,

        Alternate = 0x2,

        Both = Default | Alternate,
    }
    public enum LongItemView
    {

        ToolTip,

        ScrollSingle,

        ScrollAll
    }
    public enum ScrollMode
    {
        Page = 0,
        HalfPage = 1,
        OneThird = 2,
        OneFourth = 3,
    }
    public enum CheckStatus
    {
        //
        // Summary:
        //     The control is unchecked.

        Unchecked = 0,
        //
        // Summary:
        //     The control is checked.

        Checked = 1,
        //
        // Summary:
        //     The control is indeterminate. An indeterminate control generally has a shaded
        //     appearance.

        Indeterminate = 2
    }
    public enum TextChangeType
    {
        Replace,
        Remove,
        Add,
        Clear,
    }

    public enum PopupPosition
    {
        None,
        Bottom,
        Top
    }
    public enum CloseReason
    {
        AppFocusChange = 0,
        AppClicked = 1,
        ItemClicked = 2,
        Keyboard = 3,
        CloseCalled = 4,
    }
    public enum Wrapping
    {
        Line,
        NewPage,
        Separator
    }
    public enum ImageLook
    {
        Normal = 0x1,
        Selected = 0x2,
        Disabled = 0x4,
        Hovered = 0x8,
        Alternate = 0x10,
    }
    public enum VariableImages
    {
        Checked = 0x1,
        UnChecked = 0x2,
        Select = 0x4,
        UnSelect = 0x8,
        First = 0x10,
        Previous = 0x20,
        Next = 0x40,
        Last = 0x80,
    }
    public enum Visibility
    {
        Yes,
        No,
        Hidden,
        Displayed,
    }
    public enum ContentDisplay
    {
        Text = 0x1,
        Image = 0x2,
        Both = Text | Image,
        TextAlways = Text | 0x4,
        ImageAlways = Image | 0x8,
        BothAlways = Both | 0x10,
    }
    public enum ControlGroup
    {
        UnSpecified = 0,
        DataButtons,
        ScrollButtons,
    }
    public enum ButtonState
    {
        Default = 0x0,
        Clicked = 0x1,
        Hover = 0x2,
    }
    public enum ControlJob
    {
        UnSpecified = 0,
        ScrollPositioning,
        ScrollSingle,
        ScrollContinuous,
        DataEdit,
        DataSubmit,
        DataFilter,
        DataRequery,
        YesNo,
        DateChange,
        ValueChange,
        TextChange,
        ItemSelection,
    }
    public enum Choose
    {
        /// <summary>
        /// The first
        /// </summary>
        First,
        /// <summary>
        /// The second
        /// </summary>
        Second
    }
    /// <summary>
    /// Enum Combine
    /// </summary>
    public enum Combine
    {
        /// <summary>
        /// The horizontally
        /// </summary>
        Horizontally,
        /// <summary>
        /// The vertically
        /// </summary>
        Vertically
    }

    /// <summary>
    /// Enum Corner
    /// </summary>
    public enum Corner
    {
        /// <summary>
        /// The top left
        /// </summary>
        TopLeft,
        /// <summary>
        /// The bottom left
        /// </summary>
        BottomLeft,
        /// <summary>
        /// The top right
        /// </summary>
        TopRight,
        /// <summary>
        /// The bottom right
        /// </summary>
        BottomRight,
    }
    [Flags]
    public enum Position
    {
        None = 0x0,
        Left = 0x1,
        Top = 0x2,
        Right = 0x4,
        Bottom = 0x8,
        All = Left | Top | Right | Bottom
    }

    /// <summary>
    /// Enum ElementDraw
    /// </summary>
    public enum ElementDraw
    {
        /// <summary>
        /// The transparent
        /// </summary>
        Transparent = 0x0,
        /// <summary>
        /// The fill
        /// </summary>
        Fill = 0x1,
        /// <summary>
        /// The border
        /// </summary>
        Border = 0x2,
        /// <summary>
        /// All
        /// </summary>
        All = Fill | Border,
    }

    /// <summary>
    /// Enum RectMovement
    /// </summary>
    public enum RectMovement
    {
        /// <summary>
        /// Up
        /// </summary>
        Up = 38, Down = 40,
        /// <summary>
        /// The left
        /// </summary>
        Left = 37, Right = 39
    }

    /// <summary>
    /// Enum Direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// The vertical
        /// </summary>
        Vertical,
        /// <summary>
        /// The horizontal
        /// </summary>
        Horizontal,
    }

    /// <summary>
    /// Enum AreaOf
    /// </summary>
    public enum AreaOf
    {
        /// <summary>
        /// The caption box
        /// </summary>
        CaptionBox = 0,
        /// <summary>
        /// The text box
        /// </summary>
        PlaceHolder = 1,
        /// <summary>
        /// The drop down
        /// </summary>
        DropDown = 2,
        /// <summary>
        /// The canvas
        /// </summary>
        Canvas = 3,

        TextBox = 4,
        /// <summary>
        /// The scroll bar
        /// </summary>
        ScrollBar = 5,
        /// <summary>
        /// The custom
        /// </summary>
        Custom = 6,
        /// <summary>
        /// The wipe zone
        /// </summary>
        WipeZone = 7
    }

    /// <summary>
    /// Enum FillStyleOf
    /// </summary>
    public enum FillStyleOf
    {
        /// <summary>
        /// The caption box
        /// </summary>
        CaptionBox,
        /// <summary>
        /// The text box
        /// </summary>
        TextBox,
        /// <summary>
        /// The place holder
        /// </summary>
        PlaceHolder,
        /// <summary>
        /// The drop down
        /// </summary>
        DropDown,
        /// <summary>
        /// The scroll bar
        /// </summary>
        ScrollBar,
        /// <summary>
        /// The canvas
        /// </summary>
        Canvas,
        /// <summary>
        /// The hover
        /// </summary>
        Hover,
        /// <summary>
        /// The alternate
        /// </summary>
        Alternate,
        /// <summary>
        /// The selection
        /// </summary>
        Selection,
    }

    /// <summary>
    /// Enum HorizontalAlignment
    /// </summary>
    public enum HAlignment
    {
        /// <summary>
        /// The left
        /// </summary>
        Left,
        /// <summary>
        /// The right
        /// </summary>
        Right,
        /// <summary>
        /// The center
        /// </summary>
        Center,
    }

    /// <summary>
    /// Enum VerticalAlignment
    /// </summary>
    public enum VAlignment
    {
        /// <summary>
        /// The top
        /// </summary>
        Top,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom,
        /// <summary>
        /// The middle
        /// </summary>
        Middle
    }

    /// <summary>
    /// Enum PointShift
    /// </summary>
    public enum PointShift
    {
        /// <summary>
        /// The backward
        /// </summary>
        Backward,
        /// <summary>
        /// The forward
        /// </summary>
        Forward
    }

    public enum WindowBorder
    {
        /// <summary>
        /// The window has a resizable border. A window with a resizable border can be resized by the user or programmatically.
        /// </summary>
        Resizable = 0,
        /// <summary>
        /// The window has a fixed border. A window with a fixed border can only be resized programmatically.
        /// </summary>
        Fixed,
        /// <summary>
        /// The window does not have a border. A window with a hidden border can only be resized programmatically.
        /// </summary>
        Hidden
    }
    public enum WindowState
    {
        /// <summary>
        /// The window is  its normal state.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The window is minimized to the taskbar (also known as 'iconified').
        /// </summary>
        Minimized,
        /// <summary>
        /// The window covers the whole working area, which includes the desktop but not the taskbar and/or panels.
        /// </summary>
        Maximized,
    }
    public enum BenchmarkUnit
    {
        MilliSecond,
        Tick,
        Second,
        MicroSecond
    }
    public enum ResizeMode
    {
        /// <summary>
        /// The none
        /// </summary>
        None = Position.None,

        // Individual styles.
        /// <summary>
        /// The left
        /// </summary>
        Left = Position.Left,
        /// <summary>
        /// The top
        /// </summary>
        Top = Position.Top,
        /// <summary>
        /// The right
        /// </summary>
        Right = Position.Right,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = Position.Bottom,

        // Combined styles.
        /// <summary>
        /// All
        /// </summary>
        All = (Top | Left | Bottom | Right),
        /// <summary>
        /// The top left
        /// </summary>
        TopLeft = (Top | Left),
        /// <summary>
        /// The top right
        /// </summary>
        TopRight = (Top | Right),
        /// <summary>
        /// The bottom left
        /// </summary>
        BottomLeft = (Bottom | Left),
        /// <summary>
        /// The bottom right
        /// </summary>
        BottomRight = (Bottom | Right),
    }
    public enum CursorType
    {
        /// <summary>
        /// The arrow
        /// </summary>
        Arrow,
        /// <summary>
        /// The default
        /// </summary>
        Default,
        /// <summary>
        /// The application starting
        /// </summary>
        AppStarting,
        /// <summary>
        /// The cross
        /// </summary>
        Cross,
        /// <summary>
        /// The hand
        /// </summary>
        Hand,
        /// <summary>
        /// The help
        /// </summary>
        Help,
        /// <summary>
        /// The h split
        /// </summary>
        HSplit,
        /// <summary>
        /// The i beam
        /// </summary>
        IBeam,
        /// <summary>
        /// The no
        /// </summary>
        No,
        /// <summary>
        /// The no move2 d
        /// </summary>
        NoMove2D,
        /// <summary>
        /// The no move horiz
        /// </summary>
        NoMoveHoriz,
        /// <summary>
        /// The no move vert
        /// </summary>
        NoMoveVert,
        /// <summary>
        /// The pan east
        /// </summary>
        PanEast,
        /// <summary>
        /// The pan ne
        /// </summary>
        PanNE,
        /// <summary>
        /// The pan north
        /// </summary>
        PanNorth,
        /// <summary>
        /// The pan nw
        /// </summary>
        PanNW,
        /// <summary>
        /// The pan se
        /// </summary>
        PanSE,
        /// <summary>
        /// The pan south
        /// </summary>
        PanSouth,
        /// <summary>
        /// The pan sw
        /// </summary>
        PanSW,
        /// <summary>
        /// The pan west
        /// </summary>
        PanWest,
        /// <summary>
        /// The size all
        /// </summary>
        SizeAll,
        /// <summary>
        /// The size nesw
        /// </summary>
        SizeNESW,
        /// <summary>
        /// The size ns
        /// </summary>
        SizeNS,
        /// <summary>
        /// The size nwse
        /// </summary>
        SizeNWSE,
        /// <summary>
        /// The size we
        /// </summary>
        SizeWE,
        /// <summary>
        /// Up arrow
        /// </summary>
        UpArrow,
        /// <summary>
        /// The v split
        /// </summary>
        VSplit,
        /// <summary>
        /// The wait cursor
        /// </summary>
        WaitCursor,

        Custom,
    }

    public enum Usage
    {
        /// <summary>
        /// Value of Field marked with this usage will get Inserted and Updated but will not be used as
        /// Criteria  Update or Delete operations.
        /// </summary>
        NonKey,
        /// <summary>
        /// Value of Field marked with this usage will get Inserted.
        /// However, the Field will only be used as Criteria for Update
        /// and Delete operations
        /// </summary>
        Key,
        /// <summary>
        /// Value of Field marked with this usage is used  Update or Delete operations
        /// as Criteria only. This field will be ignored  Insert operation.
        /// </summary>
        CriteriaOnly,
        /// <summary>
        /// Value of Field marked with this usage is used  Update or Delete operations
        /// as Criteria only. AutoIncrement Field will have this usage.
        /// </summary>
        Identity,
        /// <summary>
        /// Field marked with this usage is both AutoIncremental as well as Primary Key.
        /// Only be used  Update or Delete Operations
        /// </summary>
        IdentityKey,
        /// <summary>
        /// Value of Field marked with this usage will completely ignored  all
        /// operations. Computed field which does not exist  table can have
        /// this usage.
        /// </summary>
        None,
    }
    public enum Images
    {
        None,
        Add,
        Application,
        ArrowIn,
        ArrowInout,
        ASC,
        Blog,
        Calendar,
        Chart,
        ChartOrg,
        ChartStyle,
        Checked,
        Close,
        Colour,
        ComboBox,
        Connect,
        Copy,
        Cross,
        Cut,
        Database,
        Delete,
        DESC,
        DeSelection,
        DisComboBox,
        DisDynamicControls,
        Down,
        DynamicControls,
        DyTable,
        Edit,
        Edit1,
        Edit2,
        Edit3,
        Filter,
        Firefox,
        First,
        FirstV,
        Folder,
        Form,
        GlassFind,
        HeightMinus,
        HeightPlus,
        hot,
        HScrollBar,
        Next,
        NextX,
        LastH,
        LastV,
        Left,
        Right,
        Left_right,
        List,
        ListRefresh,
        Master,
        Max,
        Maxval,
        Min,
        minus,
        Minval,
        MnM250,
        MnMWaterMark,
        MOVE,
        Multiply,
        OK,
        OPEN,
        Option,
        Page,
        Paste,
        Pie,
        Pivot,
        Plus,
        ProgressBar,
        Properties,
        RecCount,
        Redo,
        Refresh,
        Report,
        Save,
        ScrollBar,
        Selection,
        Settings,
        StopRefresh,
        Store,
        Sum,
        Table,
        TableAdd,
        TableCommit,
        TableDelete,
        TableEdit,
        TextBottom,
        TextCenter,
        TextJustify,
        TextLeft,
        TextMiddle,
        TextRight,
        TextTop,
        TextAllCaps,
        TextDropCaps,
        TextBullet,
        TextLowerCase,
        TextSmallCaps,
        TextCase,
        TextField,
        Tick,
        UnChecked,
        Undo,
        Up,
        WidthMinus,
        WidthPlus,
        XL,
        Zones
    }
    #endregion

    #region DATA OPERATION
    public enum Criteria
    {
        /// <summary>
        /// The equal
        /// </summary>
        Equal = 0,
        /// <summary>
        /// The greater than
        /// </summary>
        GreaterThan = 1,
        /// <summary>
        /// The less than
        /// </summary>
        LessThan = 2,
        /// <summary>
        /// The occurs
        /// </summary>
        Occurs = 3,
        /// <summary>
        /// The begins with
        /// </summary>
        BeginsWith = 4,
        /// <summary>
        /// The ends with
        /// </summary>
        EndsWith = 5,
        /// <summary>
        /// The occurs no case
        /// </summary>
        OccursNoCase = 6,
        /// <summary>
        /// The begins with no case
        /// </summary>
        BeginsWithNoCase = 7,
        /// <summary>
        /// The ends with no case
        /// </summary>
        EndsWithNoCase = 8,
        /// <summary>
        /// The string equal
        /// </summary>
        StringEqual = 9,
        /// <summary>
        /// The string equal no case
        /// </summary>
        StringEqualNoCase = 10,
        /// <summary>
        /// The string number greater than
        /// </summary>
        StringNumGreaterThan = 11,
        /// <summary>
        /// The string number less than
        /// </summary>
        StringNumLessThan = 12,
        /// <summary>
        /// The not equal
        /// </summary>
        NotEqual = -1,
        /// <summary>
        /// The not greater than
        /// </summary>
        NotGreaterThan = -2,
        /// <summary>
        /// The not less than
        /// </summary>
        NotLessThan = -3,
        /// <summary>
        /// The not occurs
        /// </summary>
        NotOccurs = -4,
        /// <summary>
        /// The not begins with
        /// </summary>
        NotBeginsWith = -5,
        /// <summary>
        /// The not ends with
        /// </summary>
        NotEndsWith = -6,
        /// <summary>
        /// The not occurs no case
        /// </summary>
        NotOccursNoCase = -7,
        /// <summary>
        /// The not begins with no case
        /// </summary>
        NotBeginsWithNoCase = -8,
        /// <summary>
        /// The not ends with no case
        /// </summary>
        NotEndsWithNoCase = -9,
        /// <summary>
        /// The not string equal
        /// </summary>
        NotStrEqual = -10,
        /// <summary>
        /// The not string equal no case
        /// </summary>
        NotStrEqualNoCase = -11,
        /// <summary>
        /// The not string greater than
        /// </summary>
        NotStringGreaterThan = -12,
        /// <summary>
        /// The not string less than
        /// </summary>
        NotStringLessThan = -13
    }

    /// <summary>
    /// Enum MultCriteria
    /// </summary>
    public enum MultCriteria
    {
        /// <summary>
        /// The between
        /// </summary>
        Between = 0,
        /// <summary>
        /// The not between
        /// </summary>
        NotBetween = -1,
    }

    /// <summary>
    /// Enum CriteriaMode
    /// </summary>
    public enum CriteriaMode
    {
        /// <summary>
        /// The include
        /// </summary>
        Include,
        /// <summary>
        /// The exclude
        /// </summary>
        Exclude
    }

    #endregion

    #region SOUND
    public enum SoundStatus
    {
        /// <summary>Sound is not playing</summary>
        Stopped,

        /// <summary>Sound is paused</summary>
        Paused,

        /// <summary>Sound is playing</summary>
        Playing
    }
    #endregion
}
