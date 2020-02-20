using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public struct SimplePopupItem
    {
        public readonly static SimplePopupItem Empty = new SimplePopupItem();
        public SimplePopupItem(string text): this(text, true)
        {
            Text = text;
        }
        public SimplePopupItem(string text, bool enabled): this()
        {
            Text = text;
            Enabled = enabled;
        }
        public SimplePopupItem(string text, bool enabled, Rect area)
        {
            Text = text;
            Enabled = enabled;
            Bounds = area;
        }
        public SimplePopupItem(SimplePopupItem item, int x, int y, int width, int height)
        {
            Text = item.Text;
            Enabled = item.Enabled;
            Bounds = new Rect(x, y, width, height);
        }
        public SimplePopupItem(SimplePopupItem item, int x, int y)
        {
            Text = item.Text;
            Enabled = item.Enabled;
            Bounds = new Rect(x, y, item.Bounds.Width, item.Bounds.Height);
        }
        public string Text { get; private set; }
        public bool Enabled { get; private set; }
        public Rect Bounds { get; private set; }

        public static implicit operator SimplePopupItem(string text)=>
            new SimplePopupItem(text);
        public override string ToString()
        {
            return Text;
        }
    }
}
