using System;
using System.Collections;
using System.Collections.Generic;

namespace MnM.GWS
{
    public abstract class GwsUpdateManager: IUpdateManager
    {
        #region VARIABLES
        readonly HashSet<IRectangle> list;
        readonly InvalidateEventArgs invalidate = new InvalidateEventArgs();
        #endregion

        #region CONSTRUCTORS
        protected GwsUpdateManager()
        {
            list = new HashSet<IRectangle>();
        }
        #endregion

        #region PROPERTIES
        public int Count => list.Count;
        public IRectangle Recent { get; internal set; }
        public bool SuspendRemoval { get; set; }
        #endregion

        #region METHODS
        public virtual void Invalidate(IRectangle item)
        {
            list.Add(item);
            Recent = item;

            if (Invalidated != null)
            {
                invalidate.Area = Recent;
                Invalidated.Invoke(this, invalidate);
            }
        }
        public void Invalidate(IEnumerable<IRectangle> rectangles)
        {
            foreach (var item in rectangles)
                Invalidate(item);
        }
        public virtual void Clear()
        {
            if (SuspendRemoval)
                return;
            list.Clear();
            Recent = null;
        }
        public virtual bool Clear(IRectangle item)
        {
            if (SuspendRemoval)
                return false;
            list.Remove(item);
            return true;
        }

        public virtual bool Contains(IRectangle item) =>
            list.Contains(item);
        public void TransferTo(IUpdateManager other, bool remove = true)
        {
            if (other is GwsUpdateManager)
            {
                var manager = other as GwsUpdateManager;
                foreach (var item in list)
                    manager.list.Add(item);
            }
            else
            {
                foreach (var item in this)
                    other.Invalidate(item);
            }
            if (remove)
                Clear();
        }
        #endregion

        #region ENUMERATOR
        public IEnumerator<IRectangle> GetEnumerator() =>
            list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() =>
            list.GetEnumerator();
        #endregion

        public event EventHandler<InvalidateEventArgs> Invalidated;
    }
}
