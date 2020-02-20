using MnM.GWS;
using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    /// <summary>
    /// Represents an object which has a place in GWS eco system.
    /// This is an entry point interface to be in the GWS eco system.
    /// A minimum required interface to inherit in order to make your shape work in the GWS.
    /// It must have an ID, a name Name and area to work upon.
    /// </summary>
    public interface IElement : IStoreable, IOccupier, IRecognizable { }

    /// <summary>
    /// An object which can be drawable to a given pixel target such as surface with a given pixel source.
    /// A smalll entities like point or entities which requires special drawing routine for example ISLine
    /// in herits this interface. So inherit this interface if your shape/element is special in terms of drawing routine.
    /// For the customized drawing, you can also have an option to override Renderer's RenderCustom method.
    /// So whichever, suits you.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws itself to the buffer using the buffer pen provided.
        /// </summary>
        /// <param name="Buffer">On which drawing shoould take place.</param>
        /// <param name="pen">buffer pen from which pixels will be read to draw this shape</param>
       void DrawTo(IBuffer Buffer, IBufferPen pen);
    }
   
    /// <summary>
    /// Represents an object which has certain bounds. Actually every shape has an area and that is the Bounds of it.
    /// </summary>
    public interface IOccupier
    {
        /// <summary>
        /// Area of this shape.
        /// </summary>
        IRectangleF Bounds { get; }
    }

    /// <summary>
    /// Represents an object which can be recognized by name in GWS.
    /// </summary>
    public interface IRecognizable
    {
        /// <summary>
        /// Name of this object.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Represents an object which can be redrawn on any parent window of which it is sort of part of. 
    /// i.e belonging to the control collection of that window.
    /// </summary>
    public interface IRefreshable : IStoreable, IElement, IDispose
#if AdvancedVersion
        , IVisible
#endif
    {
        /// <summary>
        /// Redraws itself using the drawsettings used when it is first added to the collection of parent window.
        /// </summary>
        void Refresh();
    }

    /// <summary>
    /// Represents an object which can be rotated. It must have bounds.
    /// </summary>
    public interface IRotatable : IOccupier
    {
        /// <summary>
        /// Angle of rotation by which it has to rotate.
        /// Angle.Empty can be used to specify no rotation.
        /// Angle with 0 or 360 degree value are considered an Empty Angle - offers no rotation.
        /// </summary>
        Angle Angle { get; }
    }
   
    /// <summary>
    /// Represents an object which can rotate, offer perimeters(surround area represented by points in sequential order) and has bounds.
    /// And of course, must also implement IElement - the gateway interface.
    /// </summary>
    public interface IShape : IRotatable, IEnumerable<PointF>, IElement { }

    /// <summary>
    /// Represent an object which is a polygon i.e made of a collection of straight lines.
    /// In GWS, having a collection of points arranged in a sequential manner - one after another, 
    /// defines a polygon. Bezier is a curve, but GWS breakes it into the straight lines so for
    /// the drawing purpose it becomes polygon without having close ends i.e first point joins the last one.
    /// All the shapes which offer closed area are in fact have the first point joined with the last.
    /// GWS, does not break the curves except bezier i.e Ellipse, Circle, Pie, Arc in straight lines and 
    /// that is why there is a separate drawing routine for them.
    /// </summary>
    public interface IPolygon : IShape { }

    /// <summary>
    /// Represents an object which have properties of bezier curve.
    /// For drawing purpose, GWS breakes the curve in straight line segments.
    /// In GWS, a bezier can be drawn by offering minimum 3 points. 
    /// However there isn't any specific number of points required except minimum 3 to draw a curve.
    /// </summary>
    public interface IBezier : IShape
    {
        /// <summary>
        /// Specified which option is used to interpret the points for accumulating bezier points.
        /// We have two options : Cubic (taking a group of 4 points) or Multiple (4, 7, 10, 13 ... so on).
        /// Please not that if only three points are provided then its a Quadratic Bezier.
        /// </summary>
        BezierType Option { get; }
    }

    /// <summary>
    /// Represent an object which has three points to offer.
    /// This object must have collection of three points.
    /// </summary>
    public interface ITriangle : IShape
    {
        /// <summary>
        /// A one among the several others center of this object.
        /// Center of a tringle is a tricky thing. We just picekd one way to calculate -
        /// i.e taking an average of x and y cordinates of all three points.
        /// Center = new PointF((a.X + b.X + c.X)/3f, (a.Y + b.Y + c.Y)/3f);
        /// Math is fun right!
        /// </summary>
        PointF Centre { get; }
    }

    /// <summary>
    /// Represents a trapezium (as defined in the British English) which has all four sides equal in length.
    /// Sides are represented in points consist of integer X & Y values.
    /// Also Oppsites sides have an agle of 90 degree between them.
    /// </summary>
    public interface ISquare : IShape 
    {
        /// <summary>
        /// Far left horizontal corodinate of this object.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Far top vertical corodinate of this object.
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Deviation from the far left horizontal corodinate (X) of this object.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        int Right { get; }
    }

    /// <summary>
    /// Represents a trapezium (as defined in the British English) which has all four sides equal in length.
    /// Sides are represented in points consist of float X & Y values.
    /// Also Oppsites sides have an agle of 90 degree between them.
    /// </summary>
    public interface ISquareF : IShape
    {
        /// <summary>
        /// Far left horizontal corodinate of this object.
        /// </summary>
        float X { get; }
        /// <summary>
        /// Far top vertical corodinate of this object.
        /// </summary>
        float Y { get; }
        /// <summary>
        /// Deviation from the far left horizontal corodinate (X) of this object.
        /// </summary>
        float Width { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        float Right { get; }
    }
    /// <summary>
    /// Represents a trapezium(as defined in the British English) which has parallel sides equal in length.
    /// Sides are represented in points consist of integer X & Y values.
    /// Also Oppsites sides have an agle of 90 degree between them.
    /// </summary>
    public interface IRectangle : IHandle
    {
        /// <summary>
        /// Far left horizontal corodinate of this object.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Far top vertical corodinate of this object.
        /// </summary>
        int Y { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Deviation from the far top vertical corodinate (Y) of this object.
        /// </summary>
        int Height { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        int Right { get; }
        /// <summary>
        /// far bottom horizontal corodinate (Y + Height) of this object.
        /// </summary>
        int Bottom { get; }
    }

    /// <summary>
    /// Represents a trapezium(as defined in the British English) which has parallel sides equal in length.
    /// Also Oppsites sides have an agle of 90 degree between them.
    /// Sides are represented in points consist of float X & Y values.
    /// </summary>
    public interface IRectangleF : IOccupier
    {
        /// <summary>
        /// Far left horizontal corodinate of this object.
        /// </summary>
        float X { get; }
        /// <summary>
        /// Far top vertical corodinate of this object.
        /// </summary>
        float Y { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        float Width { get; }
        /// <summary>
        /// Deviation from the far top vertical corodinate (Y) of this object.
        /// </summary>
        float Height { get; }
        /// <summary>
        /// far right horizontal corodinate (X + Width) of this object.
        /// </summary>
        float Right { get; }
        /// <summary>
        /// far bottom horizontal corodinate (Y + Height) of this object.
        /// </summary>
        float Bottom { get; }
    }

    /// <summary>
    /// Represents an object which is also a rectangle and shape as well.
    /// In GWS, this is a storeable version of a rectangle.
    /// </summary>
    public interface IBox : IShape, IRectangle { }

    /// <summary>
    /// Represents an object which is also a rectangleF and shape as well.
    /// In GWS, this is a storeable version of a rectangleF.
    /// </summary>
    public interface IBoxF : IShape, IRectangleF { }

    /// <summary>
    /// Represents a closed object (Quardilateral) which has four sides.
    /// This defination is in accordance with the British English and not the US one.
    /// </summary>
    public interface ITrapezoid : IShape, IRectangleF { }
    /// <summary>
    /// Represents a closed object (Quardilateral) which has four sides.
    /// At-least two sides are parallel.
    /// This defination is in accordance with the British English and not the US one.
    /// </summary>
    public interface ITrapezium : ITrapezoid { }

    /// <summary>
    /// Represents a trapezium(as defined in the British English) which has at-least two sides parallel.
    /// </summary>
    public interface IRhombus : ITrapezium { }

    /// <summary>
    /// Represents an object which has four sides just as IBox but has all corners rounded to a certain radius.
    /// </summary>
    public interface IRoundedBox : ITrapezoid
    {
        /// <summary>
        /// Radius of a circle at all four corner.
        /// </summary>
        float CornerRadius { get; }
    }
    /// <summary>
    /// Represents an ellipse which has its major and minor axis are of equal length.
    /// </summary>
    public interface ICircle: IShape
    {
        /// <summary>
        /// An object which represents information about this object.
        /// </summary>
        EllipseData Data { get; }
    }
    /// <summary>
    /// Represents an object with a curved line forming a closed loop, where the sum of the distances from two points (foci) to every point on the line is constant. 
    /// </summary>
    public interface IEllipse : ICircle { }

    /// <summary>
    /// Represents an object with a curve obtained by processing it as a quardlateral bezier curve.
    /// Plese note that mathametically, a bezier curve can not define an ellipse accurately and this
    /// is also true for arc and pie.
    /// </summary>
    public interface IBezierCurve : IEllipse 
    {
        /// <summary>
        /// Start angle from where a curve start.
        /// </summary>
        float StartAngle { get; }
        /// <summary>
        /// End Angle where a curve stops.
        /// </summary>
        float EndAngle { get; }
        /// <summary>
        /// Indicates whether or not this object is an arc.
        /// </summary>
        bool IsArc { get; }
        /// <summary>
        /// This Gets or sets a flag indicating if the end angle has to be sweeped or not.
        /// By default End Angle is sweeped, i.e End Angle += Start Angle.
        /// </summary>
        bool NoSweepAngle { get; set; }
        /// <summary>
        /// Gets or sets whether this curve is closed or not.
        /// </summary>
        bool Close { get; set; }
    }

    public interface IRing : IEllipse { }

    /// <summary>
    /// Represents an object which has a  one continious segment of curved line which does not form a closed loop, 
    /// where the sum of the distances from two points (foci) to every point on the line is constant.
    /// </summary>
    public interface ICurve: IEllipse
    {
        /// <summary>
        /// Start angle from where a curve start.
        /// </summary>
        float StartAngle { get; }
        /// <summary>
        /// End Angle where a curve stops.
        /// </summary>
        float EndAngle { get; }
        /// <summary>
        /// Defines the type of curve for example an arc or pie etc. along with other supplimentary options on how to draw it.
        /// </summary>
        CurveType Type { get; }
        /// <summary>
        /// Indicates if this object is in-fact forms a closed loop i.e an ellipse.
        /// This property indicates true if no start and end angle is provided i.e both of them are zero or one of them is 0 and othe one is 360 degree.
        /// </summary>
        bool Full { get; }
        /// <summary>
        /// Indicates a straight line between start and end points of this curve.
        /// </summary>
        ILine ArcLine { get; }
        /// <summary>
        /// Gets a collection of three points in the following order:
        /// a start point of curve,
        /// center point of the curve,
        /// an end point of the curve.
        /// </summary>
        IEnumerable<PointF> PieTriangle { get; }
        /// <summary>
        /// Indicates if any child curve is attached to this object or not.
        /// GWS uses this property to apply strokes to the original curve.
        /// </summary>
        ICurve AttachedCurve { get; }
        /// <summary>
        /// Gets the maximum vertical position (if horizontal) or horizontal position (if not horizontal) value from where the curve scan lines should be requested to fill the curve.
        /// </summary>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        float GetMaxPosition(bool horizontal);

        /// Gets the first vertical position (if horizontal) or first horizontal position (if not horizontal) value from where the curve points should be requested to draw points.
        float GetDrawStart(bool horizontal);

        /// Gets the last vertical position (if horizontal) or last horizontal position (if not horizontal) value from where the curve points should be requested to draw points.
        float GetDrawEnd(bool horizontal);

        /// <summary>
        /// Gets a collection of lines necessary to draw closing cut for this curve.
        /// </summary>
        /// <returns>
        /// If this curve is an arc and is is not closed then ..
        /// If it has no stroke then no lines returned.
        /// If it has stroke than two lines each obtained from joining correspoinding start and end points of inner and outer curve will be returned.
        /// If this curve is a pie then...
        /// If it has no stroke then two pie lines i.e one from start point to the center and another from end point to the center will be returned.
        /// If it has stroke than four pie lines from each curves  consists of inner and outer curve will be returned.
        /// </returns>
        IList<ILine> GetClosingLines();
        /// <summary>
        /// When this function is used for a given position two axially scanned lines (With a list of scanned values) are returned.
        /// Axial line means either horzontal or vertical straight line.
        /// </summary>
        /// <param name="position">Position from where the data in this cureve is requested</param>
        /// <param name="horizontal">Determines direction of an axial line either horizontal if true or vertical if false</param>
        /// <param name="forOutLinesOnly">Set to true if the interntion is not to fill but onle draw out line points</param>
        /// <param name="axis1">axis of the first axial line i.e Y coordinate if horizontal is true otherwise X cordinate</param>
        /// <param name="axis2">axis of the second axial line i.e Y coordinate if horizontal is true otherwise X cordinate</param>
        /// <returns>Collection constins of two arrays filled with scanned values for the given position</returns>
        ICollection<float> [] GetDataAt(float position, bool horizontal, bool forOutLinesOnly, out int axis1, out int axis2);
    }
    /// <summary>
    /// Represents an object which defines a line segment and its properties.
    /// </summary>
    public interface ILine : IShape, ICloneable
    {
        /// <summary>
        /// X cordinate of start point.
        /// </summary>
        float X1 { get; }
        
        /// <summary>
        /// Y cordinate of start point.
        /// </summary>
        float Y1 { get; }
        
        /// <summary>
        /// X cordinate of end point.
        /// </summary>
        float X2 { get; }
        
        /// <summary>
        /// Y cordinate of end point.
        /// </summary>
        float Y2 { get; }
       
        /// <summary>
        /// Start point of this segment;
        /// </summary>
        PointF Start { get; }
        
        /// <summary>
        /// Start point of this segment;
        /// </summary>
        PointF End { get; }
       
        /// <summary>
        /// Minimum of X coordinates of start and end points.
        /// </summary>
        float MinX { get; }
        
        /// <summary>
        /// Maximum of X coordinates of start and end points.
        /// </summary>
        float MaxX { get; }
        
        /// <summary>
        /// Minimum of Y coordinates of start and end points.
        /// </summary>
        float MinY { get; }
      
        /// <summary>
        /// Maximum of Y coordinates of start and end points.
        /// </summary>
        float MaxY { get; }

        /// <summary>
        /// Gets the slope for this line segment.
        /// </summary>
        float M { get; }
       
        /// <summary>
        /// Gets the Y intercept of this line segment.
        /// </summary>
        float C { get; }

        /// <summary>
        /// Difference between two X cordinates of start and end points.
        /// </summary>
        float DX { get; }

        /// <summary>
        /// Difference between two Y cordinates of start and end points.
        /// </summary>
        float DY { get; }

        /// <summary>
        /// For drawing purpose, this indicates to draw from X1 to X2 or Y1 to Y2.
        /// this value of true if absolute DY > absolute DX otherwise false.
        /// </summary>
        bool Steep { get; }

        /// <summary>
        /// Indicates if the segment is a horizoantal i.e Y1 equals Y2.
        /// </summary>
        bool IsHorizontal { get; }
        
        /// <summary>
        /// Indicates if the segment is a vertical i.e X1 equals X2.
        /// </summary>
        bool IsVertical { get; }

        /// <summary>
        /// Indicates if the segment is a in fact is a single point i.e X1 equals X2 and Y1 equals Y2.
        /// </summary>
        bool IsPoint { get; }

        /// <summary>
        /// Indicates if the segment is valid i.e has X1, X2, Y1, Y2 all are finite real numbers.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns a clonned copy of this object.
        /// </summary>
        /// <returns></returns>
        new ILine Clone();
    }

    /// <summary>
    /// Represents an object which has a capability to apply a cut to any axial line in order to fragement and omit an unwanted portion.
    /// </summary>
    public interface ICut 
    {
        /// <summary>
        /// Indicates if this object can not cut anything.
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// Gets the currently associated function to check if certain point is ok with in the context of this object.
        /// </summary>
        Func<float, float, bool, bool> Contains { get; }

        /// <summary>
        /// Draw aan axial line after applying a cut to it.
        /// </summary>
        /// <param name="surface">Buffer to draw a line to</param>
        /// <param name="pen">Buffer pen to be used to read from</param>
        /// <param name="list">A list to obtains and use for drawing consists of fragmented values after a cut is applied</param>
        /// <param name="val1">Axial start value i.e X coordinate if horizontal is true and Y if not</param>
        /// <param name="val2">Axial end value i.e Y coordinate if horizontal is true and X if not</param>
        /// <param name="axis">Axial axis value i.e Y coordinate if horizontal is true and X if not</param>
        /// <param name="horizontal">Direction of an axial line if true horizontal otherwise vertical</param>
        void DrawLine(IBuffer surface, IBufferPen pen, ref ICollection<float> list, float val1, float val2, int axis, bool horizontal = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pen"></param>
        /// <param name="val"></param>
        /// <param name="axis"></param>
        /// <param name="horizontal"></param>
        void DrawPixel(IBuffer surface, IBufferPen pen, float val, int axis, bool horizontal = true);
    }
    public interface IArcCut : ICut
    {
        /// <summary>
        /// Start angle from where this cut starts.
        /// </summary>
        float StartAngle { get; }

        /// <summary>
        /// Start angle from where this cut ends.
        /// </summary>
        float EndAngle { get; }

        /// <summary>
        /// Indicates if this cut represents an arc or pie.
        /// </summary>
        CurveType Option { get; }

        /// <summary>
        /// Gives a line between two arc cut points.
        /// </summary>
        ILine ArcLine { get; }

        /// <summary>
        /// Only avaible when this cut is a pie cut.
        /// This is a line from center point of this cut to the start point of the cut.
        /// </summary>
        ILine Line1 { get; }

        /// <summary>
        /// Only avaible when this cut is a pie cut.
        /// This is a line from center point of this cut to the end point of the cut.
        /// </summary>
        ILine Line2 { get; }

        /// <summary>
        /// Gets lines of child cuts as well if attached any.
        /// </summary>
        IList<ILine> Extra { get; }
    }
    /// <summary>
    /// Scans any buffer and accumulates Apoints with axial points information alongwith alpha values.
    /// </summary>
    public interface IScanner: IElement, ICloneable, IDrawable
    {
        /// <summary>
        /// Result of a scan performed on a given buffer.
        /// </summary>
        IList<IAPoint> Data { get; }
    }
    /// <summary>
    /// Represents an object which represents a text string in a drawing context.
    /// This also has a colleciton of glyphs.
    /// </summary>
    public interface IText : IRotatable, IEnumerable<IGlyph>, IElement
    {
        /// <summary>
        /// Gets the draw style attached to this object.
        /// </summary>
        IDrawStyle DrawStyle { get; }
        /// <summary>
        /// Indicates if any of the vital parameters such as font or text is changed in this object or not.
        /// </summary>
        bool Changed { get; }
        /// <summary>
        /// Gets the child glyph at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IGlyph this [int index] { get; }

        /// <summary>
        /// Idicates number of glyphs present in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Measures current text string and accumulates all the glyphs in an internal collection.
        /// </summary>
        /// <returns></returns>
        IText Measure();

        /// <summary>
        /// Measures current text string with a goven draw style.
        /// If no draws tyle provided or is null then the current draw style will be used.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        IRectangleF MeasureText(IDrawStyle style = null);

        /// <summary>
        /// Gets the kerning information available with font object for a character at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetKerning(int index);

        /// <summary>
        /// Changes internal font object with new font.
        /// </summary>
        /// <param name="newFont"></param>
        void ChangeFont(IFont newFont);

        /// <summary>
        /// Changes internal text string with new text.
        /// </summary>
        /// <param name="text"></param>
        void ChangeText(string newText);

        /// <summary>
        /// Chnages internal draw style object with new draw style.
        /// </summary>
        /// <param name="drawStyle"></param>
        /// <param name="temporary"></param>
        void ChangeDrawStyle(IDrawStyle newDrawStyle, bool temporary = true);

        /// <summary>
        /// Restores the current draw style to revious default .
        /// </summary>
        void RestoreDrawStyle();
    }
}

