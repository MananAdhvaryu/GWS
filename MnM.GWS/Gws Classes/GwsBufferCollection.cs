using System;
using System.Collections;
using System.Collections.Generic;

namespace MnM.GWS
{
    public abstract class GwsBufferCollection: IBufferCollection
    {
        #region PROPERTIES
        protected abstract IBuffer this[int index] { get; set; }
        public IBuffer Current => this[CurrentIndex];
        public abstract int Count { get; }
        public bool SuspendLayout
        {
            get => false;
            set { }
        }
        public abstract int CurrentIndex { get; }
        protected IBuffer Primary { get; private set; }
        #endregion

        #region ADD - REMOVE
        public abstract int Add();
        public abstract void Remove(int index);
        public abstract void RemoveAll();
        #endregion

        #region ACTIVATE - DEACTIVATE BUFFER
        public abstract void SwithTo(int index);
        public abstract void SwitchToMain();
        #endregion

        #region RESET BUFFER
        public abstract void Reset(int index);
        #endregion

        #region CLEAR UPDATES
        public abstract void Clear(int index);
        public abstract void Clear(int index, IRectangle rectangle);
        #endregion

        #region TRANSFER UPDATES TO MAIN
        public abstract void TransferToMain(int index, bool removeOriginal);
        #endregion

        #region RESIZE
        public void ResizeAll()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Resize(Primary.Width, Primary.Height);
            }
        }
        #endregion

        #region CHANGE PRIMARY
        public void ChangePrimary(IBuffer primary)
        {
            Primary = primary;
            ResizeAll();
        }
        #endregion

        public virtual void Dispose()
        {
        }

        #region ENUMERATOR
        public IEnumerator<IBuffer> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
