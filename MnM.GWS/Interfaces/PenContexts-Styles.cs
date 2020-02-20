using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    /// <summary>
    /// Represents an object which can be converted to a buffer pen of required width and height.
    /// </summary>
    public interface IPenContext 
    {
        /// <summary>
        /// Convert or creates this object to a buffer pen with given width and size.
        /// If both width and height are null, if this objectnis already abuffer pen then it will return itself.
        /// otherwise if it is a bufferpen it will create a resized copy of itself as buffer pen.
        /// And if none of the above, it must have a capapbility to create a bufferpen of any sort.
        /// </summary>
        /// <param name="width">Width required for a pen, if null then it is deemed as the width of this object</param>
        /// <param name="height">Height required for a pen, if null then it is deemed as the height of this object</param>
        /// <returns></returns>
        IBufferPen ToPen(int? width = null, int? height = null);
    }
    /// <summary>
    /// Represnts an RGBA color.
    /// </summary>
    public interface IRGBA : IEquatable<IRGBA>, IPenContext
    {
        /// <summary>
        /// Red component of this color.
        /// </summary>
        byte R { get; }
        /// <summary>
        /// Green component of this color.
        /// </summary>
        byte G { get; }
        /// <summary>
        /// Blue component of this color.
        /// </summary>
        byte B { get; }
        /// <summary>
        /// Alpha component of this color.
        /// </summary>
        byte A { get; }
        /// <summary>
        /// Overall value of this color.
        /// </summary>
        int Value { get; }
    }    
    /// <summary>
    /// Represents an object which provides a certain list of colors to form a spectrum using a specified stop positions.
    /// </summary>
    public interface IFillStyle : IEquatable<IFillStyle>, ICloneable, IPenContext
    {
        /// <summary>
        /// Kind of gradient that colors should generate.
        /// </summary>
        Gradient Gradient { get; }
        /// <summary>
        /// Calculated color at a given index.
        /// </summary>
        /// <param name="index">Index to get a calculated color</param>
        /// <returns></returns>
        int this[int index] { get; }
        /// <summary>
        /// numenr of colors in this style.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Number of positions.
        /// </summary>
        int PositionCount { get; }
        /// <summary>
        /// Gets the last color in list.
        /// </summary>
        int EndColor { get; }
        /// <summary>
        /// Key which represents the ID of this style.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets a position at a given index. It can then be used to get a calculated color at a given position.
        /// </summary>
        /// <param name="index">Index in a position array</param>
        /// <returns></returns>
        int Position(int index);

        /// <summary>
        /// Gets the key calculated using the gradient value in this style for a particular width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns> string value of key</returns>
        string GetBrushKey(int width, int height);
        /// <summary>
        /// Gives a new style with changed gradient.
        /// </summary>
        /// <param name="g">A new gradient for which the stle is required for.</param>
        /// <returns></returns>
        IFillStyle Change(Gradient g);
        /// <summary>
        /// Gives a new bruh for a given width and height constructed using this style.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        IBrush newBrush(int width, int height);
        /// <summary>
        /// gives a new copy of this style but with inverted order of colors.
        /// </summary>
        /// <returns></returns>
        IFillStyle Invert();
        /// <summary>
        /// Get a color on a given position assuming the size of spectrum specified by the size parameter.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size">Size to be used to calculate color</param>
        /// <param name="invert">specifies if calculation should be done in reverse order.</param>
        /// <returns></returns>
        int GetColor(float position, float size, bool invert = false);
        /// <summary>
        /// Get twin colors for a given position and total size of the spectrum.
        /// </summary>
        /// <param name="position">Position at which the twins are sought</param>
        /// <param name="size">Size of the spectrum for which calculated twins are required</param>
        /// <param name="c1">Color1 of the twins</param>
        /// <param name="c2">Color2 of the twins</param>
        void GetTweenColorsByStops(ref float position, ref float size, out int c1, out int c2);
        /// <summary>
        /// Gets the color enumerator for the spectrum.
        /// </summary>
        /// <returns></returns>
        IColorEnumerator GetEnumerator();
    }

    /// <summary>
    /// Provides the colour at a given position based on the colors provided for display when filling.
    /// </summary>
    public interface IColorEnumerator : IEnumerable<int>
    {
        /// <summary>
        /// Maximum number of color transitions.!!!!
        /// </summary>
        int MaxIteration { get; set; }
        int CurrentPosition { get; }
        /// <summary>
        /// If true then GetCurrent does not change.
        /// </summary>
        bool Freeze { set; }
        /// <summary>
        /// Returns to the begining of the colour transitions.
        /// </summary>
        void Reset();
        /// <summary>
        /// Goes to the next color allowing for the Freeze state to be ignored.
        /// </summary>
        /// <param name="ignoreFreeze">If True then Moves next regardless of Freeze setting.</param>
        void MoveNext(bool ignoreFreeze = false);
        /// <summary>
        /// Returns array of colors.
        /// </summary>
        /// <param name="start">Start point in sequence</param>
        /// <param name="end">End point in color sequence</param>
        /// <param name="maxIteration">Number of transitions between start and end.</param>
        /// <param name="centerToLeftRight">Start on left or start on right of transition sequence when creating array from left to right.</param>
        /// <returns></returns>
        int[] ToArray(int start, int end, int? maxIteration = null, bool centerToLeftRight = false);
        /// <summary>
        /// Returns the current color 
        /// </summary>
        /// <param name="invert">Inverts the color sequence.</param>
        /// <returns>A color</returns>
        int GetCurrent(bool invert = false);
    }
    /// <summary>
    /// Represent  an object to provide drawing parameters to draw a text with image  on screen.
    /// </summary>
    public interface IDrawStyle : ICloneable
    {
        /// <summary>
        /// Image style to be used for placing an image it at all one is provided.
        /// </summary>
        IImageStyle ImageStyle { get; set; }
        /// <summary>
        /// Get or sets an angle by which the text should get rotated.
        /// </summary>
        Angle Angle { get; set; }
        /// <summary>
        /// Gets or sets thepPreffered size of the text bounds.
        /// </summary>
        Size PreferredSize { get; set; }
        /// <summary>
        /// Get or sets a flag to specifying whether any case conversion of characters in a text should take place before measuring or drawing.
        /// </summary>
        CaseConversion CaseConversion { get; set; }
        /// <summary>
        /// Gets or sets Alignment of a text in respect of actual bounds of measurement.
        /// </summary>
        ContentAlignment Position { get; set; }
        /// <summary>
        /// Gets or sets a line break option while measuring or drawing a text.
        /// </summary>
        TextBreaker Breaker { get; set; }
        /// <summary>
        /// Gets or sets a break delimiter  while measuring or drawing a text. 
        /// Text can be broke into characters or a word displaed on a single line.
        /// </summary>
        BreakDelimiter Delimiter { get; set; }
        /// <summary>
        /// Gets or sets a text style to be applied for example strike out , underline etc. while drawing a text.
        /// </summary>
        TextStyle TextStyle { get; set; }
        /// <summary>
        /// Preferred line height to be applied while placing charactes on next line.
        /// </summary>
        int LineHeight { get; set; }
        /// <summary>
        /// Draws glyps individually in terms of creating bufferpen fitting individual area of a glyh rather than taking a bufferpen covering an entire area of text.
        /// </summary>
        bool DrawGlyphIndividually { get; set; }
    }

    /// <summary>
    /// Represents an object to provide drawing parameters for a buffer to be drawn on screen.
    /// </summary>
    public interface IImageStyle : ICloneable
    {
        /// <summary>
        /// Get sor sets an alignment of a buffer in a bounding box.
        /// </summary>
        ImagePosition Alignment { get; set; }
        /// <summary>
        /// Gets or sets how buffer is drawn whether scalled or unscalled.
        /// </summary>
        ImageDraw Draw { get; set; }
        /// <summary>
        /// Gets or sets a Buffer image to be drawn to screen.
        /// </summary>
        IBuffer Image { get; set; }
    }
}
