using System;
using System.Collections.Generic;

namespace MnM.GWS
{    /// <summary>
     /// Represnts the information for processing a particular shape to render.
     /// </summary>
    public interface ISlot
    {
        /// <summary>
        /// List of Points that forms an outline of the character.
        /// </summary>
        IList<PointF> Points { get; }
        /// <summary>
        /// List of curve contours
        /// </summary>
        IList<int> Contours { get; }
        /// <summary>
        /// This indicates whether this slot represents a font character or not.
        /// </summary>
        bool IsGlyph { get; }
    }
    /// <summary>
    /// Represents a font object to use for text drawing.
    /// </summary>
    public interface IFont : IStoreable
    {
        /// <summary>
        /// Gets or sets the size of the font to draw text.
        /// </summary>
        int Size { get; set; }
        /// <summary>
        /// Gets kerning information associated with the font.
        /// </summary>
        bool Kerning { get; }
        /// <summary>
        /// Gets or sets flag to enable or disable application of kerning while measuring and drawng text.
        /// </summary>
        bool EnableKerning { get; set; }
        /// <summary>
        /// Gives an information of this font such as line height, scale, horzontal and vertical bearings etc.
        /// </summary>
        IFontInfo Info { get; }
        /// <summary>
        /// Gives the kerning value for the given character in relation to previous character drawn or measured in sequential draw of given text.
        /// </summary>
        /// <param name="previous">Previous character measured or drawn</param>
        /// <param name="now">Current character for which the kerning is sought for</param>
        /// <returns></returns>
        int GetKerning(char previous, char now);
        /// <summary>
        /// Retrieves a glyph object exists for a given character which contains vital information on how to draw the character using the given font on screen.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        IGlyph GetGlyph(char character);
        /// <summary>
        /// Gets or sets a flag indication whether font hinting is to applied or not while drawing or measuring text.
        /// </summary>
        bool Hinting { get; set; }
    }
    /// <summary>
    /// Represents a glyph object which contains all the information on how a particual character it represents should be drawn on screen.
    /// </summary>
    public interface IGlyph : IDrawable, ICloneable, IOccupier
    {
        /// <summary>
        /// X cordinate of a location where the character is to be be drawn.
        /// </summary>
        int DrawX { get; }
        /// <summary>
        /// Y cordinate of a location where the character is to be be drawn.
        /// </summary>
        int DrawY { get; }
        /// <summary>
        /// The character which the glyph represents.
        /// </summary>
        char Character { get; }
        /// <summary>
        /// Gets or sets a flag indicating whether the character is to be drawn only in terms of outline and not to be filled or the otherwise.
        /// </summary>
        bool IsOutLine { get; set; }
        /// <summary>
        /// Holds an information in terms of scan lines and points of decomposed glyph to be used to draw the character on screen.
        /// </summary>
        IList<IAPoint> Data { get; }
        /// <summary>
        /// Rotates the glyph by a given angle
        /// </summary>
        /// <param name="angle"></param>
        void Rotate(Angle angle);
        /// <summary>
        /// Sets the X and Y coordintes of a location where the character is to be drawn.
        /// </summary>
        /// <param name="drawX"></param>
        /// <param name="DrawY"></param>
        void SetDrawXY(int? drawX, int? DrawY);
    }
    /// <summary>
    /// Represents information vital to draw a character for a given font.
    /// the information is directly fetched from the font object.
    /// </summary>
    public interface IGlyphSlot:  ISlot
    {
        /// <summary>
        /// Are of the slot - determines where character is to be drawn.
        /// </summary>
        IRectangleF Area { get; }
        /// <summary>
        /// The character this slot represents for drawing on screen.
        /// </summary>
        char Character { get; }
        /// <summary>
        /// XHeight of the slot.
        /// </summary>
        int XHeight { get; }
        /// <summary>
        /// Holds an information in terms of scan lines and points of decomposed glyph to be used to draw the character on screen.
        /// </summary>
        IList<IAPoint> Data { get; }

        /// <summary>
        /// Minimum of points which forms perimiter of the slot.
        /// </summary>
        PointF Min { get; }
        /// <summary>
        /// Maximum of points which forms the perimeter of the slot.
        /// </summary>
        PointF Max { get; }
        /// <summary>
        /// Indicates if the slot is initialzed and ready for the process or not.
        /// </summary>
        bool Initialized { get; }
    }
    /// <summary>
    /// Renders a given slot using the action supplied on a given context.
    /// </summary>
    public interface IGlyphRenderer : IDisposable
    {
        /// <summary>
        /// Process the glyph slot taking the action specified.
        /// </summary>
        /// <param name="target">The slot to be processed</param>
        /// <param name="action">the action to render result of the processing</param>
        /// <param name="width">Width of the area to be used for the  processing</param>
        /// <param name="height">Height of the area to be used for the  processing</param>
        void Process(ISlot target, GlyphFillAction action, int width, int height);
    }
    public interface IFontInfo
    {
        FontStyle Style { get; set; }
        float UnitsPerEm { get; set; }
        string FullName { get; set; }
        float CellAscent { get; set; }
        float CellDescent { get; set; }
        float LineHeight { get; set; }
        float XHeight { get; set; }
        float UnderlineSize { get; set; }
        float UnderlinePosition { get; set; }
        float StrikeoutSize { get; set; }
        float StrikeoutPosition { get; set; }
    }
    public interface ISlant
    {
        int X1 { get; }
        int X2 { get; }
        int Angle { get; }
    }
#if VCSupport
    public interface ICharPoint
    {
        IRectangle Bounds { get; }
        IBox RedrawBounds { get; }
        int Index { get; }
        CaretState State { get; }
        string Address { get; }
        Point UserPoint { get; }
        int X { get; }
        int Y { get; }
        int X1 { get; }
        int Y1 { get; }

        bool Forward { get; }
        bool Backward { get; }

        bool LeftAlign { get; }
        bool RightAlign { get; }

        bool KeyLeft { get; }
        bool KeyRight { get; }

        bool KeyUp { get; }
        bool KeyDn { get; }

        bool KeyPgUp { get; }
        bool KeyPgDn { get; }

        bool KeyHome { get; }
        bool KeyEnd { get; }

        bool ByMouse { get; }
        bool ByKey { get; }

        bool Selection { get; }

        bool SelectionClear { get; }

        bool MouseDrag { get; }
        bool MouseProxy { get; }
        bool MouseDirect { get; }

        bool HorizontalMove { get; }
        bool VerticalMove { get; }

        bool XForward { get; }
        bool XBackward { get; }

        bool YForward { get; }
        bool YBackward { get; }
    }
    public interface ITextMeasurement
    {
        IRectangle Bounds { get; set; }
        /// <summary>
        /// Gets or sets the text bounds.
        /// </summary>
        /// <value>The text bounds.</value>
        IRectangle TextBounds { get; set; }

        int CharIndex { get; set; }
        /// <summary>
        /// Gets or sets the index of the previous.
        /// </summary>
        /// <value>The index of the previous.</value>
        int PreviousIndex { get; set; }
        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>The character.</value>
        char Char { get; set; }
        /// <summary>
        /// Gets or sets the user point.
        /// </summary>
        /// <value>The user point.</value>
        Point UserPoint { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="tm">The tm.</param>
        void CopyFrom(ITextMeasurement tm);
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object Clone();

        void ResetBounds(Point p);
        /// <summary>
        /// Resets the bounds.
        /// </summary>
        /// <param name="lineX">The line x.</param>
        /// <param name="lineY">The line y.</param>
        void ResetBounds(int? lineX = null, int? lineY = null);
        /// <summary>
        /// Resets the empty bounds.
        /// </summary>
        /// <param name="rc">The rc.</param>
        void ResetEmptyBounds(IBox rc);
    }
#endif
}
