﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public class EventArgs<T>: EventArgs
    {
        protected internal EventArgs() { }
        public EventArgs(T args)
        {
            Args = args;
        }
        public T Args { get;  protected internal set; }
    }
    public class EventArgs<T1, T2> : EventArgs<T1>
    {
        protected internal EventArgs() { }
        public EventArgs(T1 args1, T2 args2):
            base(args1)
        {
            Args2 = args2;
        }

        public T2 Args2 { get; protected internal set; }
    }
    public class EventArgs<T1, T2, T3> : EventArgs<T1, T2>
    {
        protected internal EventArgs() { }
        public EventArgs(T1 args1, T2 args2, T3 args3) :
            base(args1, args2)
        {
            Args3 = args3;
        }
        public T3 Args3 { get; protected internal set; }
    }
}
