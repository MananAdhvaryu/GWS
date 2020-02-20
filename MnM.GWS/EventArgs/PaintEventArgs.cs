using System;

namespace MnM.GWS
{
     internal class PaintEventArgs : EventArgs, IPaintEventArgs
    {
        public IBuffer Graphics { get; private set; }

        public PaintEventArgs(IBuffer graphics)
        {
            Graphics = graphics;
        }
        void IDisposable.Dispose()
        {
            Graphics = null;
        }
    }
}
