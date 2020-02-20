using System;
using System.IO;

namespace MnM.GWS
{
    /// <summary>
    /// Represents an pen with solid color for drawing a shape on screen.
    /// </summary>
    public interface ISolidPen : IBufferPen
    { 
        /// <summary>
        /// the color this pen contains.
        /// </summary>
        int Colour { get; }
    }
    /// <summary>
    /// Represents a brush with certain fill style and gradient for drawin a shape on screen.
    /// </summary>
    public interface IBrush : IBufferPen, IBufferData
    {
        /// <summary>
        /// Gradient this brush currently represents.
        /// </summary>
        Gradient Gradient { get; }
    }
    public interface IGraphics : IBuffer, IBufferPen, ITransparent
    {
        /// <summary>
        /// Gets or sets a pixel in a 1D memory block by a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new int this[int index] { get; set; }
        /// <summary>
        /// Gives index - i.e. a location in 1D memory block.
        /// </summary>
        /// <param name="val">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "X" otherwise "Y"</param>
        /// <param name="axis">value of either X or Y coordinates depending upon which way you are looking: if horizontal val is "Y" otherwise "X"</param>
        /// <param name="horizontal">Specifies the directiion of pixel reading - to be specific it indicates if you are querying a pixel from horizontal line or vertical line</param>
        /// <returns></returns>
        new int IndexOf(int val, int axis, bool horizontal);
    }

#if Window
    public interface ISurface :  IGraphics, IHandle { }
    public interface IWindowSurface : ISurface, IUpdatable { }
    public interface ITexture : IHandle, IUploadable, IResizeable, IDisposable, IWindowHolder, IBufferCopy
    {
        bool IsPrimary { get; }
        Angle Angle { get; set; }
        RendererFlip Flip { get; set; }
        BlendMode Mode { get; set; }
        byte Alpha { get; set; }
        int ColorMode { get; set; }
        int Length { get; }
        void Bind();
        void Unbind();
        IGraphics ToCanvas(int? x, int? y, int? width = null, int? height = null);
    }
#endif 

    /// <summary>
    /// Represents an object to facilitate image data reading. GWS uses default image reader derived from STBImage. for more info on STBImage visit: https://github.com/nothings/stb
    /// </summary>
    public interface IImageReader
    {
        /// <summary>
        /// Reads a file located on a given path on disk or network drive and provides a processed data to be used for creating memory buffer. 
        /// </summary>
        /// <param name="path">Path of a file located on disk or network drive</param>
        /// <returns>
        /// Pair.Item1 - data in bytes array
        /// Pair.Item2 - Width information.
        /// Pair.Item3 - Height information.
        /// </returns>
        Pair<byte[], int, int> Read(string path);
        /// <summary>
        /// Reads a memory stream and providesa processed data to be used for creating memory buffer.
        /// </summary>
        /// <param name="stream">Strem to process</param>
        /// <returns>
        /// Pair.Item1 - data in bytes array
        /// Pair.Item2 - Width information.
        /// Pair.Item3 - Height information.
        /// </returns>
        Pair<byte[], int, int> Read(Stream stream);

        /// <summary>
        /// Reads a byte array and providesa processed data to be used for creating memory buffer.
        /// </summary>
        /// <param name="stream">Strem to process</param>
        /// <returns>
        /// Pair.Item1 - data in bytes array
        /// Pair.Item2 - Width information.
        /// Pair.Item3 - Height information.
        /// </returns>
        Pair<byte[], int, int> Read(byte[] stream);
    }

    /// <summary>
    /// Represents an object to facilitate image data writing and saving it on a disk file. 
    /// GWS uses default image writer derived from STBImage. for more info on STBImage visit: https://github.com/nothings/stb
    /// </summary>
    public interface IImageWriter
    {
        /// <summary>
        /// Writes a given memory block to a file on a given path.
        /// </summary>
        /// <param name="image">Memory block to write to disk file</param>
        /// <param name="path">Path of a file to create and write dat to</param>
        /// <param name="format">Format of the targeted image file</param>
        /// <param name="quality">Resolution quality of the tageted image file</param>
        void Write(IBufferData image, string path, ImageFormat format, int quality = 50);
        /// <summary>
        /// Writes a given memory block to a file on a given path.
        /// </summary>
        /// <param name="image">Memory block to write to disk file</param>
        /// <param name="path">Path of a file to create and write data to</param>
        /// <param name="format">Format of the targeted image file</param>
        /// <param name="quality">Resolution quality of the tageted image file</param>
        void Write(IBufferData image, Stream dest, ImageFormat format, int quality = 50);
    }
    /// <summary>
    /// Represents an object capable of saving its data to a disk image file.
    /// GWS uses default image saver derived from STBImage. for more info on STBImage visit: https://github.com/nothings/stb
    /// </summary>
    public interface IImageSaver
    {
        /// <summary>
        /// Saves entire data or a portion to a disk file in a specified image format.
        /// </summary>
        /// <param name="file">Path of a file to create and write data to</param>
        /// <param name="format">Format of the targeted image file</param>
        /// <param name="portion">Null represents a whole chunk of memory block. Other wise a prtion determined by location X, Y and size Width, Height of the portion rectangle</param>
        /// <param name="quality">Resolution quality of the tageted image file</param>
        void SaveAs(string file, ImageFormat format, IRectangle portion = null, int quality = 50);
    }
    /// <summary>
    /// Represents an object which holds animeted GIF image information.
    /// </summary>
    public interface IAnimatedGifFrame
    {
        /// <summary>
        /// Data of the image in byte array.
        /// </summary>
        byte[] Data { get; }
        /// <summary>
        /// Delay unit to be used to change a frame.
        /// </summary>
        int Delay { get; }
    }
}
