using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public class SimplePopupItemEventArgs: EventArgs
    {
        public new static SimplePopupItemEventArgs Empty = new SimplePopupItemEventArgs();
        public SimplePopupItemEventArgs() { }
        public SimplePopupItemEventArgs(SimplePopupItem item, int index)
        {
            Item = item;
            Index = index;
        }
        public SimplePopupItem Item { get; set; }
        public int Index { get; set; }
    }
}
