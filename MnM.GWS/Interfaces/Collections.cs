using System;
using System.Collections;
using System.Collections.Generic;

namespace MnM.GWS
{
    /// <summary>
    /// Readonly object containing a List
    /// </summary>
    public interface IReadOnlyList : IEnumerable
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        /// Returns the object at index in the list.
        /// </summary>
        /// <param name="index">Position in list.</param>
        /// <returns></returns>
        object this[int index] { get; }
    }
    /// <summary>
    /// Object containing list of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyList<T> : IReadOnlyList, IEnumerable<T>
    {
        /// <summary>
        /// Add new object of type T to list at index position.
        /// </summary>
        /// <param name="index">Position to add new object.</param>
        /// <returns></returns>
        new T this[int index] { get; }
    }
    /// <summary>
    /// Object containing a collection of objects of type T.
    /// </summary>
    /// <typeparam name="T">Type of Object used in collection.</typeparam>
    public interface IObjectCollection<T> : IEnumerable<T> where T : IStoreable
    {
        /// <summary>
        /// Returns number of objects in collection.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Returns object from collection with given ID.
        /// </summary>
        /// <param name="id">ID of object to return.</param>
        /// <returns></returns>
        T this[string id] { get; }

        /// <summary>
        /// Tests for existence of object T in collection.
        /// </summary>
        /// <param name="popupObject">Object to test for.</param>
        /// <returns>True if object present in collection.</returns>
        bool Contains(T popupObject);
        /// <summary>
        /// Tests objects in the collection for matching object ID 
        /// </summary>
        /// <param name="popupObjectID">ID of object required.</param>
        /// <returns>True if object present.</returns>
        bool Contains(string popupObjectID);
        /// <summary>
        /// Add object to collection (if allowed).
        /// </summary>
        /// <param name="popupObject">Object to add to collection.</param>
        void Add(T popupObject);
        /// <summary>
        /// Remove specified object from collection (if allowed).
        /// </summary>
        /// <param name="popupObject">Object to remove.</param>
        void Remove(T popupObject);
        /// <summary>
        /// Remove all values from the collection.
        /// </summary>
        void Clear();
    }
    public interface IBufferCollection : IEnumerable<IBuffer>, IDisposable
    {
        /// <summary>
        /// Gets the number of buffers present in the collection.
        /// </summary>
        int Count { get; }
        IBuffer Current { get; }

        /// <summary>
        /// Gets index of currently active buffer or sets a buffer on a given index in the collection active. 
        /// If -1 is specified then main buffer gets activated as current buffer.
        /// </summary>
        int CurrentIndex { get; }

        /// <summary>
        /// Adds new buffer in the collection.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        int Add();
      
        /// <summary>
        /// Removes buffer from the collection at a given buffer index.
        /// </summary>
        /// <param name="bufferIndex"></param>
        void Remove(int index);

        /// <summary>
        /// Lets user to switch to a buffer at a given index. i.e. to make that buffer currently active buffer for the purpose of performing pixel operations.
        /// </summary>
        /// <param name="index"></param>
        void SwithTo(int index);

        /// <summary>
        /// Lets user to switch to the primary buffer which actually not part of the collection itself.
        /// i.e. to make the primary buffer currently active buffer for the purpose of performing pixel operations.
        /// </summary>
        void SwitchToMain();

        /// <summary>
        /// Rsets the buffer at a given index by replacing its memory with memory held in primary buffer. 
        /// And also updates the pending updates collection of the primary buffer to let it be known to perform
        /// update operarion on screen on areas affected by the buffer being reset.
        /// </summary>
        /// <param name="index">Index of the buffer requested in the collection</param>
        void Reset(int index);
        /// <summary>
        /// Clear all pending updates marking of a buffer at a given index in this collection.
        /// </summary>
        /// <param name="index">Index of the buffer requested in the collection</param>
        void Clear(int index);
        /// <summary>
        /// Clear a specific pending update marking  reprented by the specified area of a buffer at a given index in this collection.
        /// </summary>
        /// <param name="index">Index of the buffer requested in the collection</param>
        /// <param name="rectangle">Specific are to remove from pending updates marking</param>
        void Clear(int index, IRectangle rectangle);

        /// <summary>
        /// Resizes all buffers to match the size of primary buffer.
        /// </summary>
        void ResizeAll();

        /// <summary>
        /// Lets user to change the primary buffer held in this object. Please note that it is not a part of collection itself.
        /// </summary>
        /// <param name="primary">The buffer intended to be the primary buffer for this collection</param>
        void ChangePrimary(IBuffer primary);

        /// <summary>
        /// Removes all buffers from the collection except the main buffer.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Transfers pending updates marking of the buffer at a given index to the primary buffer.
        /// </summary>
        /// <param name="index">Index of the buffer requested in the collection</param>
        /// <param name="removeOriginal">Lets user to decide if original marking should be removed from the buffer from where the transfer originates</param>
        void TransferToMain(int index, bool removeOriginal);
    }
}
