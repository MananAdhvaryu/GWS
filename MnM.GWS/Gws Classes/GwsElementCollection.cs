using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public abstract class ElementCollection<TInfo> : IElementCollection where TInfo : class, IDrawInfo3
    {
        #region VARIABLES
        readonly Dictionary<string, TInfo> dictionary = new Dictionary<string, TInfo>(100);
        protected readonly IBuffer Buffer;

        #endregion

        #region CONSTRUCTORS
        public ElementCollection(IParent window)
        {
            Window = window;
            if (window is IBuffer)
                Buffer = (window as IBuffer);
            else if (window is IBufferProvider)
                Buffer = (window as IBufferProvider).Buffer;
            else
                Buffer = Factory.newBuffer(window.Width, window.Height);
            ID = Factory.NewID(this);
        }
        #endregion

        #region PROPERTIES
        public IParent Window { get; private set; }

        public IElement this[string shapeID]
        {
            get
            {
                if (shapeID == null)
                    return null;
                if (!dictionary.ContainsKey(shapeID))
                    return null;
                return Factory.Get<IElement>(shapeID, ObjType.Element);
            }
        }
        public TInfo this[IElement shape]
        {
            get
            {
                if (shape.ID == null)
                    return Empty;

                if (!dictionary.ContainsKey(shape.ID))
                    return Empty;
                return dictionary[shape.ID];
            }
        }
        IDrawInfo3 IElementCollection.this[IElement shape] => this[shape];
        public string ID { get; protected set; }
        public bool AddMode { get; protected set; }
        public virtual int ItemCount => dictionary.Count;
        public virtual IEnumerable<IElement> Items => this;
        protected abstract TInfo Empty { get; }
        public string BufferID => Buffer.ID;
        #endregion

        #region IS DRAW POSSIBLE
        protected bool IsDrawPossible(IElement shape) =>
           Buffer != null && Renderer.ShapeBeingDrawn != shape.ID;
        #endregion

        #region ADD SHAPE
#if AdvancedVersion
        public abstract S Add<S>( S shape,  IPenContext context,  int? drawX,  int? drawY) where S : IElement;
#else
        public abstract S Add<S>(S shape, IPenContext context) where S : IElement;

#endif
        #endregion

        #region CONTAINS
        public bool Contains(IElement shape)
        {
            if (shape == null || shape.ID == null)
                return false;
            return dictionary.ContainsKey(shape.ID);
        }
        #endregion

        #region REMOVE
        public bool Remove(IElement shape)
        {
            if (!Contains(shape))
                return false;

            var info = dictionary[shape.ID];
            if (!RemoveItem(shape, ref info))
                return false;

            Factory.Remove(shape, ObjType.Element);
            return true;
        }
        #endregion

        #region REFRESH ALL
        public void RefreshAll()
        {
            if (Buffer == null)
                return;
            foreach (var item in Items)
                Buffer.Draw(item, null);
        }
        #endregion

        #region REMOVE ALL
        public void RemoveAll()
        {
            HideAll();
            ClearItems();
        }
        #endregion

        #region HIDE ALL
        public void HideAll()
        {
            if (Buffer == null)
                return;
            foreach (var item in Items)
            {
                if (item == null)
                    continue;
                var info = dictionary[item.ID];
                Buffer.ClearBackground(info.DrawnArea);
            }
        }
        #endregion

        #region NEW DRAWINFO
        public TInfo NewDrawInfo(IElement shape)
        {
            if (dictionary.ContainsKey(shape.ID))
                return dictionary[shape.ID];
            var info = newDrawInfo(shape);
            dictionary.Add(shape.ID, info);
            return info;
        }
        protected abstract TInfo newDrawInfo(IElement shape);
        IDrawInfo3 IElementCollection.NewDrawInfo(IElement shape) =>
             NewDrawInfo(shape);
        #endregion

        #region REFRESH
        public void Refresh(IElement shape)
        {
            if (Buffer == null || !Contains(shape))
                return;

            Buffer.Draw(shape, null);
            var info = dictionary[shape.ID];
            SetCurrentPage(info, true);

            foreach (var item in Items)
            {
                var i = dictionary[item.ID];
                if (i == null)
                    continue;
                if (IsDrawable(i, info))
                    Buffer.Draw(item, null);
            }
        }
        protected abstract void SetCurrentPage(TInfo info3, bool silent);
        protected abstract bool IsDrawable(TInfo item, TInfo compareWith);
        #endregion

        #region ADD - CLEAR - REMOVE ITEM
        protected virtual void ClearItems() =>
            dictionary.Clear();
        protected void RemoveItem(IElement shape)
        {
            dictionary.Remove(shape.ID);
            Factory.Remove(shape, ObjType.Element);
        }
        protected void RemoveItem(string shape) =>
            dictionary.Remove(shape);
        protected virtual bool RemoveItem(IElement shape, ref TInfo info)
        {
            RemoveItem(shape);
            Buffer.ClearBackground(info.DrawnArea);
            return true;
        }
        protected void AddItem(IElement shape)
        {
            if (dictionary.ContainsKey(shape.ID))
                return;
            dictionary.Add(shape.ID, NewDrawInfo(shape));
        }
        #endregion

        #region QUERY ITEMS
        protected IEnumerable<TInfo> GetItems(Predicate<TInfo> condition) =>
            dictionary.Values.Where(x => condition(x));
        protected IEnumerable<string> GetKeys() => dictionary.Keys;
        #endregion

        #region ITERATOR
        public virtual IEnumerator<IElement> GetEnumerator()
        {
            foreach (var key in dictionary.Keys)
                yield return this[key];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region DISPOSE
        public virtual void Dispose()
        {
            foreach (var item in dictionary.Keys)
                Factory.Remove(Factory.Get<IElement>(item, ObjType.Element), ObjType.Element);
            dictionary.Clear();
        }
        #endregion

        #region ADVANCED VERSION IMPLEMENTATION
#if AdvancedVersion
        public virtual int PageCount { get; protected set; }
        public virtual int CurrentPage { get; protected set; }
        public abstract bool IsMouseDragging { get; }
        public IEventUser ActiveObject { get; set; }

        public abstract EventUseStatus UseEvent(EventInfo e);
        public abstract IElement FindElement(int x, int y);
        public abstract void SetCurrentPage( int page,  bool silent = false);
        public abstract void SetPages( int noOfPages);
        public abstract void Move( IElement shape,  int? drawX = null,  int? drawY = null);
        public abstract void Resize( IElement shape,  float size,  bool clear = false);
        public abstract void Resize( IElement shape,  float width,  float height,  bool clear = false);
        public abstract bool Focus( IElement shape);
        public abstract void BringToFront( IElement shape);
        public abstract void SendToBack( IElement shape);
        public abstract void BringForward( IElement shape,  int numberOfPlaces = 1);
        public abstract void SendBackward( IElement shape,  int numberOfPlaces = 1);
        public abstract void Enable( IElement shape);
        public abstract void Disable( IElement shape);
        public abstract void Show( IElement shape);
        public abstract void Hide( IElement shape);
#endif
        #endregion

        void IStoreable.AssignIDIfNone() { }
    }
}
