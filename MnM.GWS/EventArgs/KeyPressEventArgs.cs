using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public class KeyPressEventArgs: EventArgs, IKeyPressEventArgs
    {
        internal KeyPressEventArgs()
        {

        }
        public KeyPressEventArgs(char keyChar)
        {
            KeyChar = keyChar;
        }
        public char KeyChar { get; internal set; }
    }
}
