using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
#if AdvancedVersion
    public interface IElementCollection : IDisposable, IStoreable, IEnumerable<IElement>, IEventUser, IIntercative
#else
    public interface IElementCollection : IDisposable, IStoreable, IEnumerable<IElement>
#endif
    {
        #region PROPERTIES
        /// <summary>
        /// Retrieves an element by ID
        /// </summary>
        /// <param name="id">ID by which the search is performed</param>
        /// <returns></returns>
        IElement this[string id] { get; }
        /// <summary>
        /// Retries drawing information such as last drawn area, fill mode, stroke mode etc. etc for a given element.
        /// </summary>
        /// <param name="shape">The element for which the drawing information is sought for</param>
        /// <returns></returns>
        IDrawInfo3 this[IElement shape] { get; }
        /// <summary>
        /// Indicates if the collection is currently adding an element or not.
        /// </summary>
        bool AddMode { get; }

        /// <summary>
        /// Gives number of items currently held in this collection.
        /// </summary>
        int ItemCount { get; }
        /// <summary>
        /// Gives enumearable of items held at the current page in this collection.
        /// Note: For standard version there is only one page so maintaing a something like a tab control is not possible in the version.
        /// Advanced version has support for multiple pages.
        /// </summary>
        IEnumerable<IElement> Items { get; }
        /// <summary>
        /// Gives an ID of a buffer object currently associated with this collection which provides support for pixel operations.
        /// </summary>
        string BufferID { get; }
        /// <summary>
        /// Gives a window current hosting this collection to maintain its controls hierarchy.
        /// </summary>
        IParent Window { get; }
        #endregion

        #region METHODS
#if AdvancedVersion
        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        /// <typeparam name="shape">A shape object to be added of type specifie by TShape</typeparam>
        /// <param name="context">The drawing context for the shape i.e a pen or color or a brush or even an another graphics or buffer object from which a data can be read.</param>
        /// <param name="drawX">Null or overrides the x co-ordinate of drawing location for the shape.</param>
        /// <param name="drawY">Null or overrides the y co-ordinate of drawing location of the shape.</param>
        /// <returns>Returns the same Shape object which is added . 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red, null, null);
        /// </returns>
        TShape Add<TShape>(TShape shape, IPenContext context, int? drawX, int? drawY) where TShape : IElement;
#else
        /// <summary>
        /// Adds a shape object to this collection.
        /// </summary>
        /// <typeparam name="shape">A shape object to be added of type specifie by TShape</typeparam>
        /// <param name="context">The drawing context for the shape i.e a pen or color or a brush or even an another graphics or buffer object from which a data can be read.</param>
        /// <returns>Returns the same Shape object which is added . 
        /// this lets user to pass something like new shape(....) and then used it further more.
        /// for example: var ellipse = Add(Factory.newEllipse(10,10,100,200), Colour.Red);
        /// </returns>
        TShape Add<TShape>( TShape shape,  IPenContext context) where TShape : IElement;
#endif
        /// <summary>
        /// Creates a new drawing information object for a given element after it is added in this collection to hold the current drawing information.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        IDrawInfo3 NewDrawInfo(IElement shape);
        /// <summary>
        /// Returns true if a given element is in this collection.
        /// </summary>
        /// <param name="item">The ellemnt to look for in this colleciton.</param>
        /// <returns></returns>
        bool Contains(IElement item);
        /// <summary>
        /// Removes a given element from this collection if it exist in here at all.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Remove(IElement item);
        /// <summary>
        /// Removes all existing elements from the current page of this collection.
        /// </summary>
        void RemoveAll();
        /// <summary>
        /// Re-display all existing elements of the current pageof this collection.
        /// </summary>
        void RefreshAll();
        /// <summary>
        /// Hides all existing elments of the current page of this collection.
        /// </summary>
        void HideAll();
        /// <summary>
        /// Re-display the given element in the current page of this collection.
        /// </summary>
        /// <param name="shape"></param>
        void Refresh(IElement shape);
        #endregion

        #region ADVANCED VERSION
#if AdvancedVersion
        /// <summary>
        /// Returns the number of pages available in this collection.
        /// </summary>
        int PageCount { get; }
        /// <summary>
        /// Returns the index of the current page of this collection.
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Sets a page as specified by the index to be the active page int this collection.
        /// </summary>
        /// <param name="page">Index of the page intended to be the current one.</param>
        /// <param name="silent">Specifies whether or not it should do the change without any notification</param>
        void SetCurrentPage(int page, bool silent = false);

        /// <summary>
        /// Sets number of pages to be available in this collection.
        /// </summary>
        /// <param name="noOfPages">Numer of pages to be available for use</param>
        void SetPages(int noOfPages);
        /// <summary>
        /// Finds an element from this collection if it exists on a given x and y coordinates.
        /// the test is applied on a last drawn area rather than an actual area of each element so if an element is not drawn yet, 
        /// it can be found!
        /// </summary>
        /// <param name="x">X coordinate to search for</param>
        /// <param name="y">Y coordinate to search for</param>
        /// <returns></returns>
        IElement FindElement(int x, int y);

        /// <summary>
        /// Moves a given element in the collection to new x,y co-ordinates.
        /// </summary>
        /// <param name="shape">IElement to move.</param>
        /// <param name="drawX">Null or new x co-ordinate.</param>
        /// <param name="drawY">Null or new y co-ordinate.</param>
        void Move(IElement shape, int? drawX = null, int? drawY = null);
        /// <summary>
        /// Resize a given element in the collection.
        /// </summary>
        /// <param name="shape">Element to resize.</param>
        /// <param name="size">New size - representing same scale for width and height</param>
        /// <param name="clear">If true then it applies the background and thus wiping the drawing with in.</param>
        void Resize(IElement shape, float size, bool clear = false);
        /// <summary>
        /// Resize a given element in the collection.
        /// </summary>
        /// <param name="shape">Element to resize.</param>
        /// <param name="width">New width</param>
        /// <param name="width">New height</param>
        /// <param name="clear">If true then it applies the background and thus wiping the drawing with in.</param>
        void Resize(IElement shape, float width, float height, bool clear = false);
        /// <summary>
        /// Sets focus to a giave element in this collection so that it can receive user inputs.
        /// </summary>
        /// <param name="shape">An element to get focus</param>
        /// <returns>False if the element can not be focused.</returns>
        bool Focus(IElement shape);
        /// <summary>
        /// Bring the shape to front of the IContext supplied to it.
        /// </summary>
        /// <param name="shape">IElement to bring to front.</param>
        void BringToFront(IElement shape);
        /// <summary>
        /// Sends the Shape to the Back of the drawings on the IContext supplied.
        /// </summary>
        /// <param name="shape"></param>
        void SendToBack(IElement shape);
        /// <summary>
        /// Brings the shape forward in the drawing a number of places.
        /// </summary>
        /// <param name="shape">IElement of shape to bring forward.</param>
        /// <param name="numberOfPlaces">Number of IElements to pass when bringing forward.</param>
        void BringForward(IElement shape, int numberOfPlaces = 1);
        /// <summary>
        /// Sends the shape backward in the drawing a number of places.
        /// </summary>
        /// <param name="shape">Shape to send backward.</param>
        /// <param name="numberOfPlaces">Number of IElements to pass when sending backward.</param>
        void SendBackward(IElement shape, int numberOfPlaces = 1);
        /// <summary>
        /// Enable the interactive properties of the given Shape.
        /// </summary>
        /// <param name="shape">Shape to be given user input.</param>
        void Enable(IElement shape);
        /// <summary>
        /// Disable user interaction with the given shape.
        /// </summary>
        /// <param name="shape">IElement of shape that has user interaction disabled.</param>
        void Disable(IElement shape);
        /// <summary>
        /// Draw the given shape in the provided IContext.
        /// </summary>
        /// <param name="shape">IElement of the shape to be displayed</param>
        void Show(IElement shape);
        /// <summary>
        /// Hides the given shape in the provided IContext.
        /// </summary>
        /// <param name="shape">IElement of the shape to be removed from the IContext.</param>
        void Hide(IElement shape);
#endif
        #endregion
    }

    /// <summary>
    /// Represents an object which holds a control collection to maintain its child controls.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// A collection that keeps all child controls to be maintained.
        /// </summary>
        IElementCollection Controls { get; }
    }
}
