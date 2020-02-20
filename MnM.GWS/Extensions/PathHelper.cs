using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using static MnM.GWS.Geometry;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public static class PathHelper
    {
        #region BEIGIN FLOOD FILL
        public static void Begin(IFloodFill Fill, FillAction action, IElement shape) =>
            Fill.Begin(action, shape.Bounds);
        public static void Begin(this IFloodFill Fill, FillAction action, IRectangleF Area) =>
            Fill.Begin(action, Area.X, Area.Y, Area.Width, Area.Height);
        public static void Begin(this IFloodFill Fill, FillAction action, IRectangle Area) =>
            Fill.Begin(action, Area.X, Area.Y, Area.Width, Area.Height);
        #endregion

        #region READ PIXEL
        /// <summary>
        /// Reads a pixel after applying a translation to get the correct co-ordinate.
        /// </summary>
        /// <param name="pen">buffer pen where to read pixel from</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <returns>Pixel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadPixel(this IBufferPen pen, int val, int axis, bool horizontal)
        {
            var ix = horizontal ? val : axis;
            var iy = horizontal ? axis : val;

            if (Renderer.IsRotated(Entity.BufferPen))
            {
                Renderer.GetRotatedXY(Entity.BufferPen, val, axis, horizontal, out float x, out float y);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                ix = (int)x;
                iy = (int)y;
            }

            int i = pen.IndexOf(ix, iy, true);
            if (i == -1)
                return 0;
            return Renderer.ReadPixel(pen, i);
        }

        /// <summary>
        /// Reads a pixel after applying a translation to get the correct co-ordinate.
        /// </summary>
        /// <param name="pen">The stored drawing so far. </param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <returns>Pixel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadPixel(this IBufferPen pen, float val, float axis, bool horizontal)
        {
            var x = horizontal ? val : axis;
            var y = horizontal ? axis : val;

            if (Renderer.IsRotated(Entity.BufferPen))
            {
                Renderer.GetRotatedXY(Entity.BufferPen, val, axis, horizontal, out x, out y);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
            }

            var left = (int)x;
            var top = (int)y;
            var i = pen.IndexOf(left, top, true);

            if (i == -1)
                return 0;

            if (left - x == 0 && y - top == 0)
                return Renderer.ReadPixel(pen, i);

            var lt = Renderer.ReadPixel(pen, i);
            var rt = Renderer.ReadPixel(pen, i + 1);
            if (y - top == 0)
                return Colours.Blend(lt, rt);
            var j = i + pen.Width;

            var lb = Renderer.ReadPixel(pen, j);
            ++j;
            var rb = Renderer.ReadPixel(pen, j);
            return Colours.Blend(lt, rt, lb, rb, x - left, y - top);
        }
        #endregion

        #region COMPARE PIXEL OF BUFFER WITH THE PEN IT WAS DRAWN BY
        /// <summary>
        /// Compare existing pixel on buffer for specified x and y coordinates to the corresponding pixel on buffer pen.
        /// </summary>
        /// <param name="buffer">buffer where to read pixel from</param>
        /// <param name="x">Value of X cordinate of location where to read value from</param>
        /// <param name="y">Value of Y cordinate of location where to read value from</param>
        /// <param name="pen">buffer pen which to compare pixel with</param>
        /// <returns>True if pixels are identical otherwise false</returns>
        public static bool ComparePixel(this IBuffer buffer, int x, int y, IBufferPen pen) =>
            Renderer.ComparePixel(buffer, x, y, pen);
        #endregion

        #region WRITE PIXEL X, Y
        /// <summary>
        /// Write pixels to the specified target location using specified parameters.
        /// Please note that this method does not take into account the rotated state of buffer.
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="x">X cordinate on 2d buffer memory block</param>
        /// <param name="y">Y cordinate on 2d buffer memory block</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, float x, float y, int color, bool blend = true)
        {
            var x0 = (int)x;
            var y0 = (int)y;

            int i = buffer.IndexOf(x0, y0, true);

            if (i == -1 || !Renderer.IsPixelWriteable(i))
                return;

            if (x0 == x && y0 == y)
            {
                Renderer.RenderPixel(buffer, i, color, blend);
                return;
            }
            var alpha1 = x - x0;
            var invAlpha1 = 1 - alpha1;

            var alpha2 = y - y0;
            var invAlpha2 = 1 - alpha2;

            var x1 = x.Ceiling();
            var y1 = y.Ceiling();

            Renderer.RenderPixel(buffer, i, color, blend, invAlpha1 * invAlpha2);

            if (x1 != x0)
            {
                ++i;
                Renderer.RenderPixel(buffer, i, color, blend, alpha1 * invAlpha2);
                --i;
            }
            if (y1 != y0)
            {
                i += buffer.Width;
                Renderer.RenderPixel(buffer, i, color, blend, invAlpha1 * alpha2);
            }
            ++i;
            Renderer.RenderPixel(buffer, i, color, blend, alpha1 * alpha2);
        }

        /// <summary>
        /// Write pixels to the specified target location using specified parameters
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, float val, float axis, bool horizontal, int color, bool blend = true)
        {
            var x = horizontal ? val : axis;
            var y = horizontal ? axis : val;

            if (Renderer.IsRotated(Entity.Buffer))
                Renderer.GetRotatedXY(Entity.Buffer, val, axis, horizontal, out x, out y);

            WritePixel(buffer, x, y, color, blend);
        }

        /// <summary>
        /// Write pixels to the specified target location using specified parameters
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, float val, int axis, bool horizontal, int color, bool blend = true)
        {
            if (Renderer.IsRotated(Entity.Buffer))
            {
                Renderer.GetRotatedXY(Entity.Buffer, val, axis, horizontal, out float x, out float y);
                WritePixel(buffer, x, y, color, blend);
                return;
            }

            int i = buffer.IndexOf((int)val, axis, horizontal);
            if (i == -1 || !Renderer.IsPixelWriteable(i))
                return;

            var delta = val - (int)val;

            if (delta == 0)
                Renderer.RenderPixel(buffer, i, color, blend);
            else
            {
                var incr = horizontal ? 1 : buffer.Width;
                if (delta < 0)
                {
                    delta = -delta;
                    incr = -incr;
                }
                Renderer.RenderPixel(buffer, i, color, blend, 1 - delta);
                i += incr;
                Renderer.RenderPixel(buffer, i, color, blend, delta);
            }
        }

        /// <summary>
        /// Write pixels to the specified target location using specified parameters
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        ///<param name="alpha">Vale by which blending should happen if at all it is supplied</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, int val, int axis, bool horizontal, int color, bool blend = true, float? alpha = null)
        {
            if (Renderer.IsRotated(Entity.Buffer))
            {
                Renderer.GetRotatedXY(Entity.Buffer, val, axis, horizontal, out float x, out float y);
                WritePixel(buffer, x, y, color, blend);
                return;
            }
            var i = buffer.IndexOf(val, axis, horizontal);
            if (i == -1 || !Renderer.IsPixelWriteable(i))
                return;
            Renderer.RenderPixel(buffer, i, color, blend, alpha);
        }

        /// <summary>
        /// Write pixel to the specified buffer using specified parameters
        /// </summary>
        /// <param name="buffer">buffer which to render pixel on</param>
        /// <param name="index">Position on buffer where to render pixel</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value</param>
        ///<param name="alpha">Vale by which blending should happen if at all it is supplied</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, int index, int color, bool blend, float? alpha = null) =>
            Renderer.RenderPixel(buffer, index, color, blend, alpha);

        /// <summary>
        /// Write pixels to the specified target location using specified parameters
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="pen">buffer pen where to read pixel from</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, float val, int axis, bool horizontal, IBufferPen pen, bool blend)
        {
            buffer.WritePixel(val, axis, horizontal, pen.ReadPixel((int)val, axis, horizontal), blend);
        }

        /// <summary>
        /// Write pixels to the specified target location using specified parameters
        /// </summary>
        /// <param name="buffer">buffer to write pixels on</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="pen">buffer pen where to read pixel from</param>
        /// <param name="blend">True if Pixel should be blended with current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePixel(this IBuffer buffer, int val, int axis, bool horizontal, IBufferPen pen, bool blend, float? alpha = null)
        {
            if (alpha == null || !blend)
                buffer.WritePixel(val, axis, horizontal, pen.ReadPixel(val, axis, horizontal), blend);
            else
                buffer.WritePixel(val, axis, horizontal, pen.ReadPixel(val, axis, horizontal), blend, alpha);
        }
        #endregion

        #region WRITE LINE
        /// <summary>
        /// Writes an axial line (either horizontal or vertical) to the specified buffer target using specified parameters
        /// </summary>
        /// <param name="buffer">buffer which to render an axial line on</param>
        /// <param name="pen">buffer pen which to read pixel from</param>
        /// <param name="destVal">Axial position on target buffer: X coordinate if horizontal otherwise Y</param>
        /// <param name="destAxis">>Axial position on target buffer: Y coordinate if horizontal otherwise X</param>
        /// <param name="start">Start position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Position of reading from buffer pen on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        ///<param name="alpha">Vale by which blending should happen if at all it is supplied</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLine(this IBuffer buffer, IBufferPen source, int destVal, int destAxis, int val1, int val2, int axis, bool horizontal, float? delta = null) =>
            Renderer.RenderLine(buffer, source, destVal, destAxis, val1, val2, axis, horizontal, delta);

        /// <summary>
        /// Writes an axial line (either horizontal or vertical) to the specified buffer target using specified parameters
        /// </summary>
        /// <param name="buffer">buffer which to render an axial line on</param>
        /// <param name="start">Start position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Position of reading from buffer pen on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        /// <param name="pen">buffer pen which to read pixel from</param>
        /// <param name="ends">Ends enum determines whether to draw end points or not/param>
        /// <param name="drawEndsOnly">If true it only draws ends and skips the line draw</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLine(this IBuffer buffer, float start, float end, int axis, bool horizontal, IBufferPen pen, Ends ends, bool drawEndsOnly = false)
        {
            if (buffer == null || (float.IsNaN(start) && float.IsNaN(end)))
                return;
            bool AA = Renderer.AntiAlised;
            MathHelper.Order(ref start, ref end);
            bool isPoint = start == end;

            if (ends.HasFlag(Ends.Draw))
                buffer.WritePixel(start, axis, horizontal, pen, AA);

            if (ends.HasFlag(Ends.Draw) && !isPoint)
                buffer.WritePixel(end, axis, horizontal, pen, AA);

            if (isPoint || drawEndsOnly)
                return;

            Renderer.RenderLine(buffer, pen, start.Ceiling(), axis, start.Ceiling(), end.Ceiling(), axis, horizontal, null);
        }

        /// <summary>
        /// Writes an axial line (either horizontal or vertical) to the specified buffer target using specified parameters
        /// </summary>
        /// <param name="buffer">buffer which to render an axial line on</param>
        /// <param name="start">Start position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Position of reading from buffer pen on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        /// <param name="pen">buffer pen which to read pixel from</param>
        ///<param name="alpha">Vale by which blending should happen if at all it is supplied</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLine(this IBuffer buffer, int start, int end, int axis, bool horizontal, IBufferPen pen, float? alpha = null)
        {
            if (buffer == null || (start == int.MinValue && end == int.MinValue))
                return;
            bool AA = Renderer.AntiAlised;
            MathHelper.Order(ref start, ref end);
            bool isPoint = start == end;

            if (isPoint)
            {
                buffer.WritePixel(start, axis, horizontal, pen, AA, alpha);
                return;
            }

            Renderer.RenderLine(buffer, pen, start, axis, start, end, axis, horizontal, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">buffer which to render an axial line on</param>
        /// <param name="lineData"></param>
        /// <param name="axis">Position of reading from buffer pen on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        /// <param name="pen">buffer pen which to read pixel from</param>
        /// <param name="fillType">Polyfill enum determines type of filling operation mainly OddEven or FloodFill</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteLine(this IBuffer buffer, ICollection<float> lineData, int axis, bool horizontal, IBufferPen pen, PolyFill fillType = PolyFill.OddEven)
        {
            if (buffer == null || lineData.Count == 0)
                return;

            if (fillType.Excludes(PolyFill.Flood))
                fillType |= PolyFill.OddEven;

            float[] valList;
            if (lineData is float[])
                valList = lineData as float[];
            else
                valList = lineData.ToArray();

            bool aaEnds = fillType.Includes(PolyFill.AAEnds);
            bool drawEndsOnly = fillType.Includes(PolyFill.DrawEndsOnly);
            bool applySorting = fillType.Excludes(PolyFill.NoSorting) && (!aaEnds || valList.Length > 2);

            if (applySorting)
                Array.Sort(valList);

            var even = valList.Length % 2 == 0;

            fixed (float* list = valList)
            {
                if (aaEnds)
                {
                    if (valList.Length == 1)
                    {
                        buffer.WritePixel(list[0], axis, horizontal, pen, Renderer.AntiAlised);
                        return;
                    }
                    else if (valList.Length == 2)
                    {
                        buffer.WriteLine(list[0], list[1], axis, horizontal, pen, Ends.Draw, drawEndsOnly);
                        return;
                    }
                    for (int i = 0; i < valList.Length - 1; i++)
                    {
                        buffer.WriteLine(list[i], list[i + 1], axis, horizontal, pen, Ends.Draw, drawEndsOnly);
                        if (even)
                            ++i;
                    }
                }
                else
                {
                    for (int i = 0; i < valList.Length - 1; i++)
                    {
                        if (list[i + 1].Ceiling() - list[i].Floor() > 1)
                            buffer.WriteLine(list[i].Ceiling(), list[i + 1].Ceiling(), axis, horizontal, pen);
                        else
                            buffer.WritePixel(list[i], axis, horizontal, pen, true);
                        if (even)
                            ++i;
                    }
                }
            }
        }
        #endregion

        #region DRAW - ADD SHAPE AT LOCATION
#if AdvancedVersion
        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// </summary>
        /// <param name="buffer">buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void Draw(this IBuffer buffer, IElement shape, IPenContext context) =>
            Renderer.Render(buffer, shape, context, null, null);

        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        public static void Draw(this IBuffer buffer, IElement shape) =>
            Renderer.Render(buffer, shape, null, null, null);

        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="drawX">X coordinate of a location where draw should take place</param>
        /// <param name="drawY">Y coordinate of a location where draw should take place</param>
        public static void Draw(this IBuffer buffer, IElement shape, int? drawX, int? drawY) =>
            Renderer.Render(buffer, shape, null, drawX, drawY);

        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="drawX">X coordinate of a location where draw should take place</param>
        /// <param name="drawY">Y coordinate of a location where draw should take place</param>
        public static void Draw(this IBuffer buffer, IElement shape, IPenContext context, int? drawX, int? drawY) =>
            Renderer.Render(buffer, shape, context, drawX, drawY);

        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        /// <typeparam name="T">Any object which implements IElement</typeparam>
        /// <param name="shape">A shape object to be added of type specifie by TShape</param>
        /// <param name="context">The drawing context for the shape i.e a pen or color or a brush or even an another graphics or buffer object from which a data can be read.</param>
        /// <returns>the same Shape object which is added to collection. 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red, null, null);
        /// </returns>
        public static T Add<T>(this IElementCollection collection, T shape, IPenContext context) where T : IElement =>
            collection.Add(shape, context, null, null);

        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        /// <typeparam name="T">Any object which implements IElement</typeparam>
        /// <param name="shape">A shape object to be added of type specifie by TShape</param>
        /// <returns>the same Shape object which is added to collection. 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red, null, null);
        /// </returns>
        public static T Add<T>(this IElementCollection collection, T shape) where T : IElement =>
            collection.Add(shape, null, null, null);

        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        /// <typeparam name="T">Any object which implements IElement</typeparam>
        /// <param name="shape">A shape object to be added of type specifie by TShape</param>
        /// <param name="drawX">X coordinate of a location where draw should take place</param>
        /// <param name="drawY">Y coordinate of a location where draw should take place</param>
        /// <returns>the same Shape object which is added to collection. 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red, null, null);
        /// </returns>
        public static T Add<T>(this IElementCollection collection, T shape, int? drawX, int? drawY) where T : IElement =>
            collection.Add(shape, null, drawX, drawY);
#else
        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// 
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void Draw(this IBuffer buffer, IElement shape, IPenContext context) =>
            Renderer.Render(buffer, shape,  context);

        /// <summary>
        /// Draws any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// 
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        public static void Draw(this IBuffer buffer, IElement shape) =>
            Renderer.Render(buffer, shape, null);

        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        ///<typeparam name="T">Any object which implements IElement</typeparam>
        /// <param name="shape">A shape object to be added of type specifie by TShape</param>
        /// <returns>Returns the same Shape object which is added . 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red);
        /// </returns>
        public static T Add<T>(this IElementCollection collection, T shape) where T : IElement =>
            collection.Add(shape, null);
#endif
        #endregion

        #region DRAW IMAGE
        /// <summary>
        /// Draws an image by taking an area from a 1D array representing a rectangele to the given destination.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">1D array interpreted as a 2D array of Pixels with specified srcW width</param>
        /// <param name="srcLen">Length of an entire source</param>
        /// <param name="srcW">Width of the entire source</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        public static unsafe void DrawImage(this IBuffer buffer, int* source, int srcLen, int srcW, int destX, int destY,
            int? copyX = null, int? copyY = null, int? copyW = null, int? copyH = null)
        {
            Renderer.RenderImage(buffer, source, srcLen, srcW, destX, destY, copyX, copyY, copyW, copyH);
        }

        /// <summary>
        /// Draws an image by taking an area from a source - capable of being copied to the given destination buffer.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">Source - capable of being copied to any buffer</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        public static unsafe void DrawImage(this IBuffer buffer, IBufferCopy source, int destX, int destY, int copyX, int copyY, int copyW, int copyH)
        {
            Renderer.RenderImage(buffer, source, destX, destY, copyX, copyY, copyW, copyH);
        }

        /// <summary>
        /// Draws an image by taking an area from a source - capable of being copied to the given destination buffer.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">Source - capable of being copied to any buffer</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyRc">An Area from source to be copied</param>
        public static void DrawImage(this IBuffer buffer, IBufferCopy source, int destX, int destY, IRectangle copyRc = null)
        {
            buffer.DrawImage(source, destX, destY, copyRc?.X ?? 0, copyRc?.Y ?? 0, copyRc?.Width ?? source.Width, copyRc?.Height ?? source.Height);
        }


        /// <summary>
        /// Draws an image by taking an area from a 1D array representing a rectangele to the given destination.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">1D array interpreted as a 2D array of Pixels with specified srcW width</param>
        /// <param name="srcW">Width of the entire source</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        public unsafe static void DrawImage(this IBuffer buffer, byte[] source, int srcW, int destX, int destY,
            int? copyX = null, int? copyY = null, int? copyW = null, int? copyH = null)
        {
            var srcLen = source.Length / 4;
            var rc = CompitibleRC(srcW, srcLen / srcW, copyX, copyY, copyW, copyH);
            int* src;
            fixed (byte* b = source)
            {
                src = (int*)(new IntPtr(b));
            }
            buffer.DrawImage(src, srcLen, srcW, destX, destY, rc.X, rc.Y, rc.Width, rc.Height);
        }

        /// <summary>
        /// Draws an image by taking an area from a 1D array representing a rectangele to the given destination.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">1D array interpreted as a 2D array of Pixels with specified srcW width</param>
        /// <param name="srcW">Width of the entire source</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        public unsafe static void DrawImage(this IBuffer buffer, int[] source, int srcW, int destX, int destY,
            int? copyX = null, int? copyY = null, int? copyW = null, int? copyH = null)
        {
            var rc = CompitibleRC(srcW, source.Length / srcW, copyX, copyY, copyW, copyH);
            var srcLen = source.Length;
            fixed (int* src = source)
            {
                buffer.DrawImage(src, srcLen, srcW, destX, destY, rc.X, rc.Y, rc.Width, rc.Height);
            }
        }

        /// <summary>
        /// Draws an image by taking an area from a 1D array representing a rectangele to the given destination.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">1D array interpreted as a 2D array of Pixels with specified srcW width</param>
        /// <param name="srcLen">Length of an entire source</param>
        /// <param name="srcW">Width of the entire source</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        public unsafe static void DrawImage(this IBuffer buffer, IntPtr source, int srcLen, int srcW, int destX, int destY,
           int? copyX = null, int? copyY = null, int? copyW = null, int? copyH = null)
        {
            var rc = CompitibleRC(srcW, srcLen / srcW, copyX, copyY, copyW, copyH);
            int* src = (int*)source;
            buffer.DrawImage(src, srcLen, srcW, destX, destY, rc.X, rc.Y, rc.Width, rc.Height);
        }
        #endregion

        #region DRAW LINE
        /// <summary>
        /// Renders a line segment using standard line algorithm between two points specified by x1, y1 and x2, y2.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        public static void DrawLine(this IBuffer buffer, float x1, float y1, float x2, float y2, IPenContext context, Angle angle = default(Angle)) =>
           Renderer.RenderLine(buffer, x1, y1, x2, y2, context, angle);

        /// <summary>
        /// Renders a line segment using standard line algorithm between two points specified by points p1 & p2.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="p1">Start point of line segment</param>
        /// <param name="p2">end point of line segment</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        public static void DrawLine(this IBuffer buffer, PointF p1, PointF p2, IPenContext context = null, Angle angle = default(Angle)) =>
            Renderer.RenderLine(buffer, p1.X, p1.Y, p2.X, p2.Y, context, angle);

        /// <summary>
        /// Renders a line segment using standard line algorithm between two points specified by points p1 & p2.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="p1">Start point of line segment</param>
        /// <param name="p2">end point of line segment</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        public static void DrawLine(this IBuffer buffer, Point p1, Point p2, IPenContext context = null, Angle angle = default(Angle)) =>
            Renderer.RenderLine(buffer, p1.X, p1.Y, p2.X, p2.Y, context, angle);

        /// <summary>
        /// Renders a line segment using standard line algorithm between two points specified by points p1 & x2, y2.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="p1">Start point of line segment</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        public static void DrawLine(this IBuffer buffer, PointF p1, float x2, float y2, IPenContext context = null, Angle angle = default(Angle)) =>
            Renderer.RenderLine(buffer, p1.X, p1.Y, x2, y2, context, angle);

        /// <summary>
        /// Renders a collection of line segments using standard line algorithm.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="lines">A collection of lines</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawLines(this IBuffer buffer, IEnumerable<ILine> lines, IPenContext context = null)
        {
            foreach (var l in lines)
                Renderer.RenderLine(buffer, l, context);
        }

        /// <summary>
        /// Renders line segments by first creating them from an array of integer values specified using standard line algorithm.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="connectEach">If true then each line segment will be connected to the previous and next one</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 of each line segment before rendering the line segment</param>
        /// <param name="values">An interger array of values.Each subsequent four elements get converted to a line segment
        /// For example if values are int[]{23, 56, 98, 205} creates Line(X1 = 23, Y1 = 56, X2 = 98,Y2 = 205) </param>
        public static void DrawLines(this IBuffer buffer, IPenContext context, bool connectEach, Angle angle, params int[] values)
        {
            var points = (values).ToPointsF();
            if (connectEach)
                points.Add(points[0]);

            if (connectEach)
            {
                for (int i = 1; i < points.Count; i++)
                    buffer.DrawLine(points[i - 1], points[i], context, angle);
                return;
            }
            for (int i = 1; i < points.Count; i += 2)
            {
                buffer.DrawLine(points[i - 1], points[i], context, angle);
            }
        }

        /// <summary>
        /// Renders line segments by first creating them from an array of float values specified using standard line algorithm.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="connectEach">If true then each line segment will be connected to the previous and next one</param>
        /// <param name="angle"></param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 of each line segment before rendering the line segment</param>
        /// <param name="values">A float array of values.Each subsequent four elements get converted to a line segment
        /// For example if values are int[]{23.33f, 56.67f, 98.45f, 205.21f} creates Line(X1 = 23.33f, Y1 = 56.67f, X2 = 98.45f,Y2 = 205.21f) </param>
        public static void DrawLines(this IBuffer buffer, IPenContext context, bool connectEach, Angle angle, params float[] values)
        {
            var points = values.ToPoints();
            if (connectEach)
                points.Add(points[0]);

            if (connectEach)
            {
                for (int i = 1; i < points.Count; i++)
                    buffer.DrawLine(points[i - 1], points[i], context, angle);
                return;
            }
            for (int i = 1; i < points.Count; i += 2)
            {
                buffer.DrawLine(points[i - 1], points[i], context, angle);
            }
        }

        /// <summary>
        /// Renders line segments by first creating them from an array of integer values specified using standard line algorithm.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="connectEach">If true then each line segment will be connected to the previous and next one</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 of each line segment before rendering the line segment</param>
        /// <param name="values">An interger array of values.Each subsequent four elements get converted to a line segment
        /// For example if values are int[]{23, 56, 98, 205} creates Line(X1 = 23, Y1 = 56, X2 = 98,Y2 = 205) </param>
        public static void DrawLines(this IBuffer buffer, bool connectEach, Angle angle, params int[] values) =>
            buffer.DrawLines(null, connectEach, angle, values);


        /// <summary>
        /// Renders line segments by first creating them from an array of float values specified using standard line algorithm.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="connectEach">If true then each line segment will be connected to the previous and next one</param>
        /// <param name="angle"></param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 of each line segment before rendering the line segment</param>
        /// <param name="values">A float array of values.Each subsequent four elements get converted to a line segment
        /// For example if values are int[]{23.33f, 56.67f, 98.45f, 205.21f} creates Line(X1 = 23.33f, Y1 = 56.67f, X2 = 98.45f,Y2 = 205.21f) </param>
        public static void DrawLines(this IBuffer buffer, bool connectEach, Angle angle, params float[] values) =>
            buffer.DrawLines(null, connectEach, angle, values);
        #endregion

        #region DRAW CIRCLE
        /// <summary>
        /// Draws a circle specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="x">X cordinate of a bounding area where the circle is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle is to be drawn -> circle's minor X axis = Width/2</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle</param>
        public static void DrawCircle(this IBuffer buffer, float x, float y, float width, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderCircleOrEllipse(buffer, x, y, width, width, context, angle);

        /// <summary>
        /// Draws a circle specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="x">X cordinate of a bounding area where the circle is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle is to be drawn -> circle's minor X axis = Width/2</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle</param>
        public static void DrawCircle(this IBuffer buffer, float x, float y, float width, Angle angle = default(Angle)) =>
            Renderer.RenderCircleOrEllipse(buffer, x, y, width, width, null, angle);

        /// <summary>
        /// Draws a circle specified by the center point and another point which provides a start location and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="a">A point on a circle which you want</param>
        /// <param name="center">Center of a circle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle</param>
        public static void DrawCircle(this IBuffer buffer, PointF a, PointF center, IPenContext context, Angle angle = default(Angle))
        {
            var w = center.DistanceFrom(a);
            var x = a.X;
            var y = a.Y;
            Renderer.RenderCircleOrEllipse(buffer, x, y, w, w, context, angle.AssignCenter(center));
        }

        /// <summary>
        /// Draws a circle specified by the center point and another point which provides a start location and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="a">A point which must on a circle</param>
        /// <param name="center">Center of a circle</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle</param>
        public static void DrawCircle(this IBuffer buffer, PointF a, PointF center, Angle angle = default(Angle)) =>
            buffer.DrawCircle(a, center, null, angle);
        #endregion

        #region DRAW ELLIPSE
        /// <summary>
        /// Draws an ellipse specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a ellipse on</param>
        /// <param name="x">X cordinate of a bounding area where the ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the ellipse is to be drawn -> ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the ellipse is to be drawn -> ellipse's minor Y axis = Height/2</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle/ellipse</param>
        public static void DrawEllipse(this IBuffer buffer, float x, float y, float width, float height, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderCircleOrEllipse(buffer, x, y, width, height, context, angle);

        /// <summary>
        /// Draws an ellipse specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a ellipse on</param>
        /// <param name="x">X cordinate of a bounding area where the ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the ellipse is to be drawn -> ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the ellipse is to be drawn -> ellipse's minor Y axis = Height/2</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle/ellipse</param>
        public static void DrawEllipse(this IBuffer buffer, float x, float y, float width, float height, Angle angle = default(Angle)) =>
            Renderer.RenderCircleOrEllipse(buffer, x, y, width, height, null, angle);
      
        /// <summary>
        /// Draws an ellipse passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="a">A point on an ellipse</param>
        /// <param name="b">Another point which must be on the ellipse</param>
        /// <param name="center">Center of the ellipse</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle/ellipse</param>
        public static void DrawEllipse(this IBuffer buffer, PointF a, PointF b, PointF center,  IPenContext context, Angle angle = default(Angle))
        {
            var w = center.DistanceFrom(a);
            var h = center.DistanceFrom(b);
            h = (float)(Math.Sqrt((b.Y - a.Y) * (b.Y - a.Y) + (b.X - a.X) * (b.X - a.X)));
            center = new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);

            var x = center.X - w / 2;
            var y = center.Y - h / 2;

            var Angle = angle.AssignCenter(center, true);
            Renderer.RenderCircleOrEllipse(buffer, x, y, w, h, context, Angle);
        }

        /// <summary>
        /// Draws an ellipse passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="a">A point on an ellipse</param>
        /// <param name="b">Another point which must be on the ellipse</param>
        /// <param name="center">Center of the ellipse</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle/ellipse</param>
        public static void DrawEllipse(this IBuffer buffer, PointF a, PointF b, PointF center, Angle angle = default(Angle)) =>
            buffer.DrawEllipse(center, a, b, null, angle);
        #endregion

        #region DRAW ARC
        /// <summary>
        /// Draws an arc specified by the bounding area and angle of rotation if supplied using various option supplied throuh CurveType enum.
        /// </summary>
        /// <param name="buffer">Buffer which to render an arc on</param>
        /// <param name="x">X cordinate of a bounding area where the arc is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the arc is to be drawn</param>
        /// <param name="width">Width of a bounding area where the arc is to be drawn -> arc's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the arc is to be drawn ->arc's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc</param>
        /// <param name="type"> Defines the type of an arc along with other supplimentary options on how to draw it</param>
        public static void DrawArc(this IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, IPenContext context, Angle angle = default(Angle), CurveType type = CurveType.Arc)
        {
            type = type & ~CurveType.Pie;
            type |= CurveType.Arc;
            Renderer.RenderArcOrPie(buffer, x, y, width, height, startAngle , endAngle, context, angle, type);
        }

        /// <summary>
        /// Draws an arc specified by the bounding area and angle of rotation if supplied using various option supplied throuh CurveType enum.
        /// </summary>
        /// <param name="buffer">Buffer which to render an arc on</param>
        /// <param name="x">X cordinate of a bounding area where the arc is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the arc is to be drawn</param>
        /// <param name="width">Width of a bounding area where the arc is to be drawn -> arc's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the arc is to be drawn ->arc's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc</param>
        /// <param name="type"> Defines the type of an arc along with other supplimentary options on how to draw it</param>
        public static void DrawArc(this IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, Angle angle = default(Angle), CurveType type = CurveType.Arc) =>
            Renderer.RenderArcOrPie(buffer, x, y, width, height, startAngle, endAngle, null, angle, type);

        /// <summary>
        /// Draws an arc passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render an arc on</param>
        /// <param name="a">A point on an arc</param>
        /// <param name="b">Another point which must be on the arc</param>
        /// <param name="center">Center of the arc</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc</param>
        public static void DrawArc(this IBuffer buffer, PointF a, PointF b, PointF center, IPenContext context, Angle angle = default(Angle), CurveType type = CurveType.Arc)
        {
            var w = (float)Math.Sqrt(center.DistanceSquared(a));
            var h = (float)Math.Sqrt(center.DistanceSquared(b));

            var x = center.X - w / 2;
            var y = center.Y - h / 2;

            var arcStart = GetAngle(a.X, a.Y, center);
            var arcEnd = GetAngle(b.X, b.Y, center);

            type = type & ~CurveType.Pie;
            type |= CurveType.Arc;
            Renderer.RenderArcOrPie(buffer, x, y, w, h, arcStart, arcEnd, context, angle, type);
        }

        /// <summary>
        /// Draws an arc passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render an arc on</param>
        /// <param name="a">A point on an arc</param>
        /// <param name="b">Another point which must be on the arc</param>
        /// <param name="center">Center of the arc</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc</param>
        public static void DrawArc(this IBuffer buffer, PointF center, PointF a, PointF b, Angle angle = default(Angle), CurveType type = CurveType.Arc) =>
            buffer.DrawArc(center, a, b, null, angle, type);
        #endregion

        #region DRAW PIE
        /// <summary>
        /// Draws a pie specified by the bounding area and angle of rotation if supplied using various option supplied throuh CurveType enum.
        /// </summary>
        /// <param name="buffer">Buffer which to render a pie on</param>
        /// <param name="x">X cordinate of a bounding area where the pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the pie is to be drawn -> pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the pie is to be drawn ->pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the pie</param>
        /// <param name="type"> Defines the type of an pie along with other supplimentary options on how to draw it</param>
        public static void DrawPie(this IBuffer buffer, float x, float y, float width, float height,
            float startAngle, float endAngle, IPenContext context, Angle angle = default(Angle), CurveType type = CurveType.Pie)
        {
            type = type & ~CurveType.Arc;
            type |= CurveType.Pie;
            Renderer.RenderArcOrPie(buffer, x, y, width, height, startAngle, endAngle, context, angle, type);
        }

        /// <summary>
        /// Draws a pie specified by the bounding area and angle of rotation if supplied using various option supplied throuh CurveType enum.
        /// </summary>
        /// <param name="buffer">Buffer which to render a pie on</param>
        /// <param name="x">X cordinate of a bounding area where the pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the pie is to be drawn -> pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the pie is to be drawn ->pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the pie</param>
        /// <param name="type"> Defines the type of an pie along with other supplimentary options on how to draw it</param>
        public static void DrawPie(this IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, Angle angle = default(Angle), CurveType type = CurveType.Pie) =>
            buffer.DrawPie(x, y, width, height, startAngle, endAngle, null, angle, type | CurveType.Pie);

        /// <summary>
        /// Draws a pie passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a pie on</param>
        /// <param name="a">A point on the pie</param>
        /// <param name="b">Another point which must be on the pie</param>
        /// <param name="center">Center of the pie</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the pie</param>
        public static void DrawPie(this IBuffer buffer, PointF a, PointF b,PointF center,  IPenContext context, Angle angle = default(Angle), CurveType type = CurveType.Pie)
        {
            var w = (float)Math.Sqrt(center.DistanceSquared(a));
            var h = (float)Math.Sqrt(center.DistanceSquared(b));

            var x = center.X - w / 2;
            var y = center.Y - h / 2;

            var arcStart = GetAngle(a.X, a.Y, center);
            var arcEnd = GetAngle(b.X, b.Y, center);

            type = type & ~CurveType.Arc;
            type |= CurveType.Pie;
            Renderer.RenderArcOrPie(buffer, x, y, w, h, arcStart, arcEnd, context, angle, type);
        }

        /// <summary>
        /// Draws a pie passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a pie on</param>
        /// <param name="a">A point on the pie</param>
        /// <param name="b">Another point which must be on the pie</param>
        /// <param name="center">Center of the pie</param>
        /// <param name="angle">Angle to apply rotation while rendering the pie</param>
        public static void DrawPie(this IBuffer buffer, PointF a, PointF b, PointF center,  Angle angle = default(Angle), CurveType type = CurveType.Pie) =>
            buffer.DrawPie(center, a, b, null, angle, type);
        #endregion

        #region DRAW BEZIER
        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawBezier(this IBuffer buffer, IPenContext context, params float[] points) =>
            Renderer.RenderBezier(buffer, points, BezierType.Cubic, context);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, IPenContext context, Angle angle, params float[] points) =>
            Renderer.RenderBezier(buffer, points, BezierType.Cubic, context, angle);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, IPenContext context, params float[] points) =>
            Renderer.RenderBezier(buffer, points, type, context);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, IPenContext context, Angle angle, params float[] points) =>
            Renderer.RenderBezier(buffer, points, type, context, angle);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        public static void DrawBezier(this IBuffer buffer, params float[] points) =>
            Renderer.RenderBezier(buffer, points, BezierType.Cubic, null);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, Angle angle, params float[] points) =>
            Renderer.RenderBezier(buffer, points, BezierType.Cubic, null, angle);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, params float[] points) =>
            Renderer.RenderBezier(buffer, points, type, null);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in float - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, Angle angle, params float[] points) =>
            Renderer.RenderBezier(buffer, points, type, null, angle);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in integers - each group of two subsequent values forms one point i.e x & y</param>
        public static void DrawBezier(this IBuffer buffer, params int[] points) =>
            Renderer.RenderBezier(buffer, points.Select(p => (float)p), BezierType.Cubic, null);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in integers - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, Angle angle, params int[] points) =>
            Renderer.RenderBezier(buffer, points.Select(p => (float)p), BezierType.Cubic, null, angle);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in integers - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, params int[] points) =>
            Renderer.RenderBezier(buffer, points.Select(p => (float)p), type, null);

        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier as values in integers - each group of two subsequent values forms one point i.e x & y</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        public static void DrawBezier(this IBuffer buffer, BezierType type, Angle angle, params int[] points) =>
            Renderer.RenderBezier(buffer, points.Select(p => (float)p), type, null, angle);
        #endregion

        #region DRAW BEZIER ARC
        /// <summary>
        /// Renders a bezier arc specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier bezier arc on</param>
        /// <param name="x">X cordinate of a bounding area where the bezier arc is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier arc is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier arc is to be drawn -> bezier arc's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier arc is to be drawn ->bezier arc's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc</param>
        /// <param name="noSweepAngle">Determines whether the end angle has to be sweeped or not. By default End Angle is sweeped, i.e End Angle += Start Angle</param>
        public static void DrawBezierArc(this IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, IPenContext context, Angle angle = default(Angle), bool noSweepAngle = false) =>
            Renderer.RenderBezierArcOrPie(buffer, x, y, width, height, startAngle, endAngle, true, context, angle, noSweepAngle);

        /// <summary>
        /// Renders a bezier arc specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier bezier arc on</param>
        /// <param name="x">X cordinate of a bounding area where the bezier arc is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier arc is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier arc is to be drawn -> bezier arc's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier arc is to be drawn ->bezier arc's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc</param>
        /// <param name="noSweepAngle">Determines whether the end angle has to be sweeped or not. By default End Angle is sweeped, i.e End Angle += Start Angle</param>
        public static void DrawBezierArc(this IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, Angle angle = default(Angle), bool noSweepAngle = false) =>
            buffer.DrawBezierArc(x, y, width, height, startAngle, endAngle, null, angle, noSweepAngle);

        /// <summary>
        /// Draws an arc passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier arc on</param>
        /// <param name="a">A point on the bezier arc</param>
        /// <param name="b">Another point which must be on the bezier arc</param>
        /// <param name="center">Center of the bezier arc</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc</param>
        public static void DrawBezierArc(this IBuffer buffer, PointF a, PointF b,PointF center,  IPenContext context, Angle angle = default(Angle), bool noSweepAngle = false)
        {
            var w = (float)Math.Sqrt(center.DistanceSquared(a));
            var h = (float)Math.Sqrt(center.DistanceSquared(b));

            var x = center.X - w / 2;
            var y = center.Y - h / 2;

            var arcStart = GetAngle(a.X, a.Y, center);
            var arcEnd = GetAngle(b.X, b.Y, center);

            Renderer.RenderBezierArcOrPie(buffer, x, y, w, h, arcStart, arcEnd, true, context, angle, noSweepAngle);
        }

        /// <summary>
        /// Draws an arc passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier arc on</param>
        /// <param name="a">A point on the bezier arc</param>
        /// <param name="b">Another point which must be on the bezier arc</param>
        /// <param name="center">Center of the bezier arc</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc</param>
        public static void DrawBezierArc(this IBuffer buffer, PointF a, PointF b,  PointF center, Angle angle = default(Angle), bool noSweepAngle = false) =>
            buffer.DrawBezierArc(center, a, b, null, angle, noSweepAngle);
        #endregion

        #region DRAW BEZIER PIE
        /// <summary>
        /// Renders a bezier pie specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier bezier pie on</param>
        /// <param name="x">X cordinate of a bounding area where the bezier pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier pie is to be drawn -> bezier pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier pie is to be drawn ->bezier pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier pie</param>
        /// <param name="noSweepAngle">Determines whether the end angle has to be sweeped or not. By default End Angle is sweeped, i.e End Angle += Start Angle</param>
        public static void DrawBezierPie(this IBuffer buffer, float x, float y, float width, float height,
            float startAngle, float endAngle, IPenContext context, Angle angle = default(Angle), bool noSweepAngle = false) =>
            Renderer.RenderBezierArcOrPie(buffer, x, y, width, height, startAngle, endAngle, false, context, angle, noSweepAngle);

        /// <summary>
        /// Renders a bezier pie specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier bezier pie on</param>
        /// <param name="x">X cordinate of a bounding area where the bezier pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier pie is to be drawn -> bezier pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier pie is to be drawn ->bezier pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier pie</param>
        /// <param name="noSweepAngle">Determines whether the end angle has to be sweeped or not. By default End Angle is sweeped, i.e End Angle += Start Angle</param>
        public static void DrawBezierPie(this IBuffer buffer, float x, float y, float width, float height,
            float startAngle, float endAngle, Angle angle = default(Angle), bool noSweepAngle = false) =>
            buffer.DrawBezierPie(x, y, width, height, startAngle, endAngle, null, angle, noSweepAngle);

        /// <summary>
        /// Draws a pie passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier pie on</param>
        /// <param name="a">A point on the bezier pie</param>
        /// <param name="b">Another point which must be on the bezier pie</param>
        /// <param name="center">Center of the bezier pie</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier pie</param>
        public static void DrawBezierPie(this IBuffer buffer,PointF a, PointF b,  PointF center, IPenContext context, Angle angle = default(Angle), bool noSweepAngle = false)
        {
            var w = (float)Math.Sqrt(center.DistanceSquared(a));
            var h = (float)Math.Sqrt(center.DistanceSquared(b));

            var x = center.X - w / 2;
            var y = center.Y - h / 2;

            var arcStart = GetAngle(a.X, a.Y, center);
            var arcEnd = GetAngle(b.X, b.Y, center);

            Renderer.RenderBezierArcOrPie(buffer, x, y, w, h, arcStart, arcEnd, false, context, angle, noSweepAngle);
        }

        /// <summary>
        /// Draws a pie passing through two specified points a & b and has a center and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier pie on</param>
        /// <param name="a">A point on the bezier pie</param>
        /// <param name="b">Another point which must be on the bezier pie</param>
        /// <param name="center">Center of the bezier pie</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier pie</param>
        public static void DrawBezierPie(this IBuffer buffer, PointF a, PointF b, PointF center, Angle angle = default(Angle), bool noSweepAngle = false) =>
            buffer.DrawBezierPie(center, a, b, null, angle, noSweepAngle);
        #endregion

        #region DRAW TRIANGLE
        /// <summary>
        /// Renders a trianle formed by three points specified by x1,y1 & x2,y2 & x3,y3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="x1">X corodinate of the first point</param>
        /// <param name="y1">Y corodinate of the first point</param>
        /// <param name="x2">X corodinate of the second point</param>
        /// <param name="y2">Y corodinate of the second point</param>
        /// <param name="x3">X corodinate of the third point</param>
        /// <param name="y3">Y corodinate of the third point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the traingle</param>
        public static void DrawTriangle(this IBuffer buffer, float x1, float y1, float x2, float y2, float x3, float y3, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTriangle(buffer, x1, y1, x2, y2, x3, y3, context, angle);

        /// <summary>
        /// Renders a trianle formed by three points specified by x1,y1 & x2,y2 & x3,y3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="x1">X corodinate of the first point</param>
        /// <param name="y1">Y corodinate of the first point</param>
        /// <param name="x2">X corodinate of the second point</param>
        /// <param name="y2">Y corodinate of the second point</param>
        /// <param name="x3">X corodinate of the third point</param>
        /// <param name="y3">Y corodinate of the third point</param>
        /// <param name="angle">Angle to apply rotation while rendering the traingle</param>
        public static void DrawTriangle(this IBuffer buffer, float x1, float y1, float x2, float y2, float x3, float y3, Angle angle = default(Angle)) =>
            buffer.DrawTriangle(x1, y1, x2, y2, x3, y3, null, angle);

        /// <summary>
        /// Renders a trianle formed by three points specified by points p1, p2, p3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="p3">the third point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle"></param>
        public static void DrawTriangle(this IBuffer buffer, Point p1, Point p2, Point p3, IPenContext context, Angle angle = default(Angle)) =>
            buffer.DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, context, angle);

        /// <summary>
        /// Renders a trianle formed by three points specified by points p1, p2, p3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="p3">the third point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle"></param>
        public static void DrawTriangle(this IBuffer buffer, PointF p1, PointF p2, PointF p3, IPenContext context, Angle angle = default(Angle)) =>
            buffer.DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, context, angle);

        /// <summary>
        /// Renders a trianle formed by three points specified by points p1, p2, p3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="p3">the third point</param>
        /// <param name="angle"></param>
        public static void DrawTriangle(this IBuffer buffer, Point p1, Point p2, Point p3, Angle angle = default(Angle)) =>
        buffer.DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, null, angle);

        /// <summary>
        /// Renders a trianle formed by three points specified by points p1, p2, p3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="p3">the third point</param>
        /// <param name="angle"></param>
        public static void DrawTriangle(this IBuffer buffer, PointF p1, PointF p2, PointF p3, Angle angle = default(Angle)) =>
            buffer.DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, null, angle);
        #endregion

        #region DRAW SQUARE
        /// <summary>
        /// Renders a rectangle specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="width">Width  and also height of the rectangle/param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawSquare(this IBuffer buffer, float x, float y, float width, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, x, y, width, width, context, angle);

        /// <summary>
        /// Renders a rectangle specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="width">Width  and also height of the rectangle/param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawSquare(this IBuffer buffer, float x, float y, float width, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, x, y, width, width, null, angle);
        #endregion

        #region DRAW RECTANGLE
        /// <summary>
        /// Renders a rectangle specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="width">Width of the rectangle/param>
        /// <param name="height">Height the rectangle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, float x, float y, float width, float height, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, x, y, width, height, context, angle);

        /// <summary>
        /// Renders a rectangle specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="width">Width of the rectangle/param>
        /// <param name="height">Height the rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, float x, float y, float width, float height, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, x, y, width, height, null, angle);

        /// <summary>
        /// Renders a rectangle specified by x, y, rigth, bottom parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="r">Far right of the rectangle/param>
        /// <param name="b">Far bottom the rectangle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void RenderRectangleFromLTRB(float x, float y, float r, float b, IBuffer buffer, IPenContext context, float? Stroke = null, FillMode? fillMode = null)
        {
            var mode = fillMode ?? Renderer.FillMode;
            var stroke = Stroke ?? Renderer.Stroke;
            IBufferPen pen;
            if (stroke == 0 || mode == FillMode.Original || (stroke == 0 && mode == FillMode.DrawOutLine))
            {
                pen = Renderer.GetPen(Factory.RectangleFFromLTRB(x, y, r, b), context);
                if (mode == FillMode.DrawOutLine)
                {
                    mode = Renderer.FillMode;
                    Renderer.CopySettings(fillMode: FillMode.Original);

                    buffer.WriteLine(x.Round(), r.Round(), y.Round(), true, pen);
                    buffer.WriteLine(x.Round(), r.Round(), b.Round(), true, pen);
                    buffer.WriteLine(y.Round(), b.Round(), x.Round(), false, pen);
                    buffer.WriteLine(y.Round(), b.Round(), r.Round(), false, pen);
                    Renderer.CopySettings(fillMode: mode);
                }
                else
                {
                    for (int i = (int)y; i <= b; i++)
                        buffer.WriteLine(x, r, i, true, pen, Ends.NoDraw);
                }
                return;
            }

            float outer = (stroke / 2f);
            IRectangleF Area = Factory.RectangleFFromLTRB(x - outer, y - outer, r + outer, b + outer);
            pen = Renderer.GetPen(Area, context);
            Renderer.FreezeXY(Entity.BufferPen, true);

            switch (mode)
            {
                case FillMode.Outer:
                default:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.FillOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, x + outer, b + outer, buffer, pen, 0);
                    RenderRectangleFromLTRB(r - outer, y - outer, r + outer, b + outer, buffer, pen, 0);
                    RenderRectangleFromLTRB(x, y - outer, r, y + outer, buffer, pen, 0);
                    RenderRectangleFromLTRB(x, b - outer, r, b + outer, buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.Inner:
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.DrawOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, buffer, pen, 0);
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return; ;
                case FillMode.ExceptOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, buffer, pen, 0, FillMode.DrawOutLine);
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, buffer, pen, 0, FillMode.Original);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
            }
        }

        /// <summary>
        /// Renders a rectangle specified by x, y, rigth, bottom parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="r">Far right of the rectangle/param>
        /// <param name="b">Far bottom the rectangle</param>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="Stroke">Stroke to apply to a rectangle on</param>
        /// <param name="fillMode">Fill mode to be used to draw a rectangle on</param>
        public static void DrawRectangle(float x, float y, float r, float b, IBuffer buffer, float? Stroke = null, FillMode? fillMode = null) =>
            RenderRectangleFromLTRB(x, y, r, b, buffer, null, Stroke, fillMode);

        /// <summary>
        /// Renders a rectangle specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="r">Rectangle to draw</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, IRectangle r, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, r.X, r.Y, r.Width, r.Height, context, angle);

        /// <summary>
        /// Renders a rectangle specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="r">Rectangle to draw</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, IRectangleF r, IPenContext context, Angle angle = default(Angle)) =>
           Renderer.RenderRectangle(buffer, r.X, r.Y, r.Width, r.Height, context, angle);

        /// <summary>
        /// Renders a rectangle specified by r parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="r">Rectangle to draw</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, IRectangle r, Angle angle = default(Angle)) =>
            Renderer.RenderRectangle(buffer, r.X, r.Y, r.Width, r.Height, null, angle);

        /// <summary>
        /// Renders a rectangle specified by r parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="r">Rectangle to draw</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        public static void DrawRectangle(this IBuffer buffer, IRectangleF r, Angle angle = default(Angle)) =>
           Renderer.RenderRectangle(buffer, r.X, r.Y, r.Width, r.Height, null, angle);
        #endregion

        #region DRAW ROUNDED BOX
        /// <summary>
        /// Renders a rounded box specified by x, y, width, height parameters and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="x">X cordinate of the rounded box</param>
        /// <param name="y">Y cordinate of the rounded box</param>
        /// <param name="width">Width of the rounded box/param>
        /// <param name="height">Height the rounded box</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, float x, float y, float width, float height, float cornerRadius, IPenContext context, Angle angle = default(Angle)) =>
           Renderer.RenderRoundedBox(buffer, x, y, width, height, cornerRadius, context, angle);

        /// <summary>
        /// Renders a rounded box specified by x, y, width, height parameters and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="x">X cordinate of the rounded box</param>
        /// <param name="y">Y cordinate of the rounded box</param>
        /// <param name="width">Width of the rounded box/param>
        /// <param name="height">Height the rounded box</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, float x, float y, float width, float height, float cornerRadius, Angle angle = default(Angle)) =>
            Renderer.RenderRoundedBox(buffer, x, y, width, height, cornerRadius, null, angle);

        /// <summary>
        /// Renders a rounded box specified by r parameter and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="r">Base rectange to construct the rounded box from</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, IRectangleF r, float cornerRadius, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderRoundedBox(buffer, r.X, r.Y, r.Width, r.Height, cornerRadius, context, angle);

        /// <summary>
        /// Renders a rounded box specified by r parameter and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="r">Base rectange to construct the rounded box from</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, IRectangle r, float cornerRadius, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderRoundedBox(buffer, r.X, r.Y, r.Width, r.Height, cornerRadius, context, angle);

        /// <summary>
        /// Renders a rounded box specified by r parameter and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="r">Base rectange to construct the rounded box from</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, IRectangleF r, float cornerRadius, Angle angle = default(Angle)) =>
        Renderer.RenderRoundedBox(buffer, r.X, r.Y, r.Width, r.Height, cornerRadius, null, angle);

        /// <summary>
        /// Renders a rounded box specified by r parameter and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="r">Base rectange to construct the rounded box from</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        public static void DrawRoundedBox(this IBuffer buffer, IRectangle r, float cornerRadius, Angle angle = default(Angle)) =>
            Renderer.RenderRoundedBox(buffer, r.X, r.Y, r.Width, r.Height, cornerRadius, null, angle);
        #endregion

        #region DRAW RHOMBUS
        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, float x, float y, float width, float height, Angle angle, float? deviation, IPenContext context) =>
           Renderer.RenderRhombus(buffer, x, y, width, height, angle, deviation, context);

        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        public static void DrawRhombus(this IBuffer buffer, float x, float y, float width, float height, Angle angle, float? deviation) =>
           Renderer.RenderRhombus(buffer, x, y, width, height, angle, deviation, null);

        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        public static void DrawRhombus(this IBuffer buffer, float x, float y, float width, float height, Angle angle) =>
            Renderer.RenderRhombus(buffer, x, y, width, height, angle, null, null);

        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, float x, float y, float width, float height, Angle angle, IPenContext context) =>
            Renderer.RenderRhombus(buffer, x, y, width, height, angle, null, context);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangleF r, Angle angle, float? deviation, IPenContext context) =>
          Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, deviation, context);


        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangleF r, Angle angle, float? deviation) =>
          Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, deviation, null);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangleF r, Angle angle, IPenContext context) =>
          Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, null, context);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangleF r, Angle angle) =>
            Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, null, null);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangle r, Angle angle, float? deviation, IPenContext context) =>
            Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, deviation, context);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangle r, Angle angle, float? deviation) =>
            Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, deviation, null);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangle r, Angle angle, IPenContext context) =>
            Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, null, context);

        /// <summary>
        /// Renders a rhombus specified by r parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="r">Base rectangle to draw rhombus from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        public static void DrawRhombus(this IBuffer buffer, IRectangle r, Angle angle) =>
            Renderer.RenderRhombus(buffer, r.X, r.Y, r.Width, r.Height, angle, null, null);
        #endregion

        #region DRAW TRAPEZIUM
        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="baseLine">A line from where the trapezium start</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, ILine baseLine, float parallelLineDeviation, float parallelLineSizeDifference, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, baseLine, parallelLineDeviation, parallelLineSizeDifference, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, ILine baseLine, float parallelLineDeviation, float parallelLineSizeDifference, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, baseLine, parallelLineDeviation, parallelLineSizeDifference, angle, null);
        
        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, ILine baseLine, float parallelLineDeviation, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, baseLine, parallelLineDeviation, 0, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="baseLine">A line from where the trapezium start</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, ILine baseLine, float parallelLineDeviation, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, baseLine, parallelLineDeviation, 0, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by the first four values in values parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="values">An array of float values- first four of which forms a base line from where the trapezium start.
        /// Parallel line deviation should be the 5th i.e values[4] item in values parameter if values has length of 5 otherwise deemed as zero which results in a simple line draw.
        /// 6th item i.e values[5] in values would form parallel Line Size Difference value if the lenght of values is 6 otherwise zero.
        /// </param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, float[] values, IPenContext context, Angle angle = default(Angle))
        {
            if (values.Length < 4)
                return;

            var first = Factory.newLine(values[0], values[1], values[2], values[3]);
            float parallelLineDeviation = 30f;
            float parallelLineSizeDifference = 0;
            if (values.Length > 4)
                parallelLineDeviation = values[4];
            if (values.Length > 5)
                parallelLineSizeDifference = values[5];
            Renderer.RenderTrapezium(buffer, first, parallelLineDeviation, parallelLineSizeDifference, angle, context);
        }

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by the first four values in values parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="values">An array of int values- first four of which forms a base line from where the trapezium start.
        /// Parallel line deviation should be the 5th i.e values[4] item in values parameter if values has length of 5 otherwise deemed as zero which results in a simple line draw.
        /// 6th item i.e values[5] in values would form parallel Line Size Difference value if the lenght of values is 6 otherwise zero.
        /// </param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, int[] values, IPenContext context, Angle angle = default(Angle))
        {
            if (values.Length < 4)
                return;

            var first = Factory.newLine(values[0], values[1], values[2], values[3]);
            float parallelLineDeviation = 30f;
            float parallelLineSizeDifference = 0;
            if (values.Length > 4)
                parallelLineDeviation = values[4];
            if (values.Length > 5)
                parallelLineSizeDifference = values[5];
            Renderer.RenderTrapezium(buffer, first, parallelLineDeviation, parallelLineSizeDifference, angle, context);
        }

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by the first four values in values parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="values">An array of float values- first four of which forms a base line from where the trapezium start.
        /// Parallel line deviation should be the 5th i.e values[4] item in values parameter if values has length of 5 otherwise deemed as zero which results in a simple line draw.
        /// 6th item i.e values[5] in values would form parallel Line Size Difference value if the lenght of values is 6 otherwise zero.
        /// </param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, float[] values, Angle angle = default(Angle)) =>
            buffer.DrawTrapezium(values, null, angle);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by the first four values in values parameter and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="values">An array of int values- first four of which forms a base line from where the trapezium start.
        /// Parallel line deviation should be the 5th i.e values[4] item in values parameter if values has length of 5 otherwise deemed as zero which results in a simple line draw.
        /// 6th item i.e values[5] in values would form parallel Line Size Difference value if the lenght of values is 6 otherwise zero.
        /// </param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, int[] values, Angle angle = default(Angle)) =>
            buffer.DrawTrapezium(values, null, angle);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from parameters x1, y1, x2, y2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, float x1, float y1, float x2, float y2, float parallelLineDeviation, float parallelLineSizeDifference, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(x1, y1, x2, y2), parallelLineDeviation, parallelLineSizeDifference, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from parameters x1, y1, x2, y2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, float x1, float y1, float x2, float y2, float parallelLineDeviation, float parallelLineSizeDifference, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(x1, y1, x2, y2), parallelLineDeviation, parallelLineSizeDifference, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from parameters x1, y1, x2, y2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, float x1, float y1, float x2, float y2, float parallelLineDeviation, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(x1, y1, x2, y2), parallelLineDeviation, 0, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from parameters x1, y1, x2, y2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, float x1, float y1, float x2, float y2, float parallelLineDeviation, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(x1, y1, x2, y2), parallelLineDeviation, 0, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, PointF p1, PointF p2, float parallelLineDeviation, float parallelLineSizeDifference, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, parallelLineSizeDifference, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, Point p1, Point p2, float parallelLineDeviation, float parallelLineSizeDifference, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, parallelLineSizeDifference, angle, context);


        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, PointF p1, PointF p2, float parallelLineDeviation, float parallelLineSizeDifference, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, parallelLineSizeDifference, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, Point p1, Point p2, float parallelLineDeviation, float parallelLineSizeDifference, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, parallelLineSizeDifference, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, PointF p1, PointF p2, float parallelLineDeviation, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, 0, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawTrapezium(this IBuffer buffer, Point p1, Point p2, float parallelLineDeviation, IPenContext context, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, 0, angle, context);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, PointF p1, PointF p2, float parallelLineDeviation, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, 0, angle, null);

        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line formed from points p1 & p2, 
        /// parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="p1">A start point of a base line</param>
        /// <param name="p2">An end point of a base line</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawTrapezium(this IBuffer buffer, Point p1, Point p2, float parallelLineDeviation, Angle angle = default(Angle)) =>
            Renderer.RenderTrapezium(buffer, Factory.newLine(p1, p2), parallelLineDeviation, 0, angle, null);
        #endregion

        #region DRAW POLYGON
        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawPolygon(this IBuffer buffer, IPenContext context, Angle angle, params float[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints, context, angle);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon an each group of two subsequent values in polypoints forms a point x,y</param>
        public static void DrawPolygon(this IBuffer buffer, params float[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints, null);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawPolygon(this IBuffer buffer, IPenContext context, params float[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints, context);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawPolygon(this IBuffer buffer, Angle angle, params float[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints, null, angle);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon an each group of two subsequent values in polypoints forms a point x,y</param>
        public static void DrawPolygon(this IBuffer buffer, params int[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints.Select(p => (float)p), null);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        public static void DrawPolygon(this IBuffer buffer, IPenContext context, params int[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints.Select(p => (float)p), context);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawPolygon(this IBuffer buffer, IPenContext context, Angle angle, params int[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints.Select(p => (float)p), context, angle);

        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygom on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon  an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        public static void DrawPolygon(this IBuffer buffer, Angle angle, params int[] polyPoints) =>
            Renderer.RenderPolygon(buffer, polyPoints.Select(p => (float)p), null, angle);
        #endregion

        #region DRAW TEXT
        /// <summary>
        /// Draw text on a specified buffer using specified parameters.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="font">Font object to get glyphs collection for a given text</param>
        /// <param name="destX">X cordinate of destination point where text gets drawn</param>
        /// <param name="destY">Y cordinate of destination point where text gets drawn</param>
        /// <param name="text">A string of characters to draw</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="drawStyle">A draw style to be used to draw text</param>
        /// <returns>GlyphsData object which contains a draw result information such as glyphs, drawn area etc.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GlyphsData DrawText(this IBuffer buffer, IFont font, float destX, float destY, string text, IPenContext context, IDrawStyle drawStyle)
        {
            if (buffer == null || font == null || string.IsNullOrEmpty(text))
                return GlyphsData.Empty;
            var info = font.MeasureText(text, destX, destY, drawStyle);
            var pen = Renderer.GetPen(info, context);
            Renderer.RenderGlyphs(buffer, info.Glyphs, pen);
            return info;
        }

        /// <summary>
        /// Draw text on a specified buffer using specified parameters.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="font">Font object to get glyphs collection for a given text</param>
        /// <param name="destX">X cordinate of destination point where text gets drawn</param>
        /// <param name="destY">Y cordinate of destination point where text gets drawn</param>
        /// <param name="text">A string of characters to draw</param>
        /// <param name="drawStyle">A draw style to be used to draw text</param>
        /// <returns>GlyphsData object which contains a draw result information such as glyphs, drawn area etc.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GlyphsData DrawText(this IBuffer buffer, IFont font, float destX, float destY, string text, IDrawStyle drawStyle) =>
            buffer.DrawText(font, destX, destY, text, null, drawStyle);

        /// <summary>
        /// Draw text on a specified buffer using specified parameters.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="font">Font object to get glyphs collection for a given text</param>
        /// <param name="destX">X cordinate of destination point where text gets drawn</param>
        /// <param name="destY">Y cordinate of destination point where text gets drawn</param>
        /// <param name="text">A string of characters to draw</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <returns>GlyphsData object which contains a draw result information such as glyphs, drawn area etc.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GlyphsData DrawText(this IBuffer buffer, IFont font, float destX, float destY, string text, IPenContext context) =>
            buffer.DrawText(font, destX, destY, text, context, null);
        #endregion

        #region DRAW BOUNDING BOX OF SHAPE
        //public static void DrawBoundingBox(this IRectangleF shape, IWriter buffer, IReader pen)
        //{
        //    if (buffer.DrawBoundingBox)
        //    {
        //        var mode = buffer.FillMode;
        //        buffer.DrawBoundingBox = false;
        //        buffer.FillMode = FillMode.DrawOutLine;
        //        if (shape is IShape)
        //        {
        //            Renderer.render(shape as IShape, buffer, pen);
        //        }
        //        else
        //            buffer.DrawRectangle(shape.Bounds, pen);

        //        buffer.FillMode = mode;
        //        buffer.DrawBoundingBox = true;
        //    }
        //}
        #endregion
    }
}
