using System;

namespace MnM.GWS
{
    /// <summary>
    /// Lets an object to be able to paste a block of memory of it to some other destination pixels.
    /// </summary>
    public interface IBufferCopy : ISize
    {
        /// <summary>
        /// Provides a paste routine to paste the specified chunk of data to a given destination pointer on a given location.
        /// </summary>
        /// <param name="copyRc">Specifies the block to copy from</param>
        /// <param name="destination">Specifies a pointer where the block should get copied</param>
        /// <param name="destLen">Specifies the current length of the destination pointer</param>
        /// <param name="destW">Specifies the current width by which the pixel writing should be wrapped to the next line</param>
        /// <param name="destX">Specifies the X coordinate where the paste operation should commence</param>
        /// <param name="destY">specifies the Y coordinate from where the paste operation should commence</param>
        /// <returns></returns>
        unsafe IRectangle CopyTo(IRectangle copyRc, IntPtr destination, int destLen, int destW, int destX, int destY);
    }
    /// <summary>
    /// Specifies the size in terms of width, height and 1D array length of a memory block.
    /// </summary>
    public interface IBufferSize : ISize
    {
        /// <summary>
        /// Length of an inner 1D memory block.
        /// </summary>
        int Length { get; }
    }
    /// <summary>
    /// Represents an object which has a memory block to offer for pixel operations.
    /// </summary>
    public interface IBufferData : IBufferSize
    {
        /// <summary>
        /// A pointer to an inner 1D memory block.
        /// </summary>
        IntPtr Pixels { get; }
    }
    /// <summary>
    /// Specifies an object which can be used to copy and read data from for various pixel operations. Base interface for pens and brushes.
    /// </summary>
    public interface IBufferPen : IBufferSize, IBufferCopy, IStoreable, IPenContext, IDispose
    {
        /// <summary>
        /// Gives index - i.e. a location in 1D memory block.
        /// </summary>
        /// <param name="val">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "X" otherwise "Y"</param>
        /// <param name="axis">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "Y" otherwise "X"</param>
        /// <param name="horizontal">Specifies the directiion of pixel reading - to be specific it indicates if you are querying a pixel from horizontal line or vertical line</param>
        /// <returns></returns>
        int IndexOf(int val, int axis, bool horizontal);
        /// <summary>
        /// Gives a currently held value at a given index in 1D memory block.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int this[int index] { get; }
    }

    /// <summary>
    /// Represents an object which holds a memory buffer in 1D array but presented as 2D memory block. Base interface for Graphics and Surface object.
    /// </summary>
    public interface IBuffer : IBufferCopy, IBufferData, IDisposable, IStoreable, IImageSaver, IUpdateTracker, IResizeable, IBufferBackground
#if AdvancedVersion
        , IMultiBuffersProvider
#endif

    {
        /// <summary>
        /// Revelas an Unique ID of this object.
        /// </summary>
        new string ID { get; }
        /// <summary>
        /// Gets or sets a currently held value at a given index in 1D memory block.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int this[int index] { get; set; }

        /// <summary>
        /// Gives index - i.e. a location in 1D memory block.
        /// </summary>
        /// <param name="val">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "X" otherwise "Y"</param>
        /// <param name="axis">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "Y" otherwise "X"</param>
        /// <param name="horizontal">Specifies the directiion of pixel reading - to be specific it indicates if you are querying a pixel from horizontal line or vertical line</param>
        /// <returns></returns>
        int IndexOf(int val, int axis, bool horizontal);
        /// <summary>
        /// Since width and height is known, this method reveals an X and Y coordinates of a given index.
        /// GWS defines blocks as a 1D array represent as 2D array of certain width upto certain height.
        /// So the equation for finding given index is x + y * width.
        /// This is sort of another representation of that equation.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void XYOf(int index, out int x, out int y);
    }

    /// <summary>
    /// Represents an object which offer a memory block to perform pixel operations.
    /// </summary>
    public interface IBufferProvider
    {
        /// <summary>
        /// A memory block contained in an object. This can be a simple buffer, graphics or a surface.
        /// </summary>
        IBuffer Buffer { get; }
    }

    /// <summary>
    /// Represents an object which is capable of holding multi layered buffer object i.e collection of buffers.
    /// </summary>
    public interface IMultiBuffersProvider
    {
        /// <summary>
        /// A collection which holds and facilitates access to any desired buffer int it.
        /// </summary>
        IBufferCollection Buffers { get; }
    }

    /// <summary>
    /// Provides ways and means to apply and clear background of a buffer object - i.e Grpahics, Surfaces etc.
    /// </summary>
    public interface IBufferBackground
    {
        /// <summary>
        /// Indicates which Background pen is last used to apply background to an object.
        /// </summary>
        string BackgroundPen { get; }

        /// <summary>
        /// Applies back ground to an object. 
        /// </summary>
        /// <param name="context">A user can specifiy a variety of objects to paint the background which includes pens, brushes, buffers from which pixels can be read</param>
        void ApplyBackground(IPenContext context);

        /// <summary>
        /// Indicates if a pixel at gian axial coordinates is exactly the same pixel as the pixel of background of an object at the same given location.
        /// </summary>
        /// <param name="val">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "X" otherwise "Y"</param>
        /// <param name="axis">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "Y" otherwise "X"</param>
        /// <param name="horizontal">Specifies the directiion of pixel reading - to be specific it indicates if you are querying a pixel from horizontal line or vertical line</param>
        /// <returns></returns>
        bool IsBackgroundPixel(int val, int axis, bool horizontal);
        /// <summary>
        /// Clear the background of the object starting from the location x, y upto the width (w) and height(h).
        /// </summary>
        /// <param name="x">X coordinate of the start of clear operation</param>
        /// <param name="y">Y coordinate of the start of clear operation</param>
        /// <param name="w">starting from X, horizontal distance up to which clear operation expands</param>
        /// <param name="h">starting from Y, vertical distance up to which clear operation expands</param>
        void ClearBackground(int x, int y, int w, int h);

        /// <summary>
        /// Clear the background of the object starting from the location specified by X and Y cordinates of the area upto the Width and Height of the area.
        /// </summary>
        /// <param name="area">Area to perform clear operaion</param>
        void ClearBackground(IRectangle area);

        /// <summary>
        /// Clear the entire background of the object.
        /// </summary>
        void ClearBackground();

        /// <summary>
        /// Lets anyone subscribed to this event to know that a background of the object is changed.
        /// </summary>
        event EventHandler<IEventArgs> BackgroundChanged;
    }
}
