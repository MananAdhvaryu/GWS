using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public class InvalidateEventArgs
    {
        internal InvalidateEventArgs() { }
        public InvalidateEventArgs(IRectangle rectangle, bool addMode = false)
        {
            AddMode = addMode;
            Area = rectangle;
        }
        public bool AddMode { get; internal set; }
        public IRectangle Area { get; internal set; }
    }
}
