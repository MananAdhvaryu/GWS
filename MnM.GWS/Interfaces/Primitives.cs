using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    /// <summary>
    /// Reprents an APoint structure which can also have a lenght to stretch.
    /// </summary>
    public interface IAPoint: IDrawable
    {
        /// <summary>
        /// Axial value : if IsHorizontal is trule then X otherwise Y.
        /// </summary>
        float Val { get; }
        /// Axial value : if IsHorizontal is trule then Y otherwise X.
        int Axis { get; }
        /// <summary>
        /// Specifies the direction of axis point stretch whether it is horizontal or not.
        /// </summary>
        bool IsHorizontal { get; }
        /// <summary>
        /// Integer portion of Val.
        /// </summary>
        int IVal { get; }
        /// <summary>
        /// Alpha value to draw entire point and up to the length specified.
        /// </summary>
        float? Alpha { get; }
        /// <summary>
        /// Tells if Val is infact real number or an interger one.
        /// </summary>
        bool IsFloat { get; }
        /// <summary>
        /// Length of stretch for this APoint.
        /// </summary>
        int Len { get; }

        /// <summary>
        /// Adds stroke to this point.
        /// </summary>
        /// <param name="stroke"></param>
        /// <returns></returns>
        IAPoint Add(float stroke);
        /// <summary>
        /// Clones this point.
        /// </summary>
        /// <returns>Clonned copy of this point</returns>
        IAPoint Clone();
    }
    /// <summary>
    /// Stores integer Width and Height of an object.
    /// </summary>
    public interface ISize
    {
        /// <summary>
        /// The Width of an object.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// The Height of an object.
        /// </summary>
        int Height { get; }
    }
    /// <summary>
    /// Represents a location.
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// Gets X coordinate.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Gets Y coordinate.
        /// </summary>
        int Y { get; }
    }
    /// <summary>
    /// Represents an object which has an axial line.
    /// </summary>
    public interface IXLine : IDrawable
    {
        /// <summary>
        /// Starting APoint
        /// </summary>
        IAPoint A { get; }
        /// <summary>
        /// Ending APoint.
        /// </summary>
        IAPoint B { get; }
        /// <summary>
        /// Length between A & B;
        /// </summary>
        int Len { get; }
        /// <summary>
        /// X coordinate of starting point.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y coordinate of starting point.
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Add stroke to this line.
        /// </summary>
        /// <param name="stroke"></param>
        /// <returns></returns>
        ISLine Add(float stroke);
        /// <summary>
        /// Test whether given x and y coordinates belongs to this line.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool Contains(float x, float y);
    }
    /// <summary>
    /// Represent a fragmented line with one continious axial line and one child axial line lies between 2 end points of the main line.
    /// this results in 3 fragments. 
    /// 1. From Main Start point to the Child start point.
    /// 2. Child segments.
    /// 3. From Child endpoit to the Main End Point.
    /// </summary>
    public interface ISLine: IXLine
    {
        /// <summary>
        /// Main continious line.
        /// </summary>
        IXLine Main { get; }
        /// <summary>
        /// Child line which lies between end points of Main line.
        /// </summary>
        IXLine Child { get; }
        /// <summary>
        /// 1 of the summary description.
        /// </summary>
        IXLine OutLine1 { get; }
        /// <summary>
        /// 3 of the summary description.
        /// </summary>
        IXLine OutLine2 { get; }
        /// <summary>
        /// Get if this object has any fragments or not.
        /// </summary>
        bool HasOutLines { get; }
    }
}
