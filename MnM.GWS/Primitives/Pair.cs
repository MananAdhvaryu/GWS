using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public struct Pair<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;
        public static Pair<T1, T2> Empty = new Pair<T1, T2>();

        public Pair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override string ToString()
        {
            return "Item1: " + Item1 + ", Item2: " + Item2;
        }
    }
    public struct Pair<T1, T2, T3>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public static Pair<T1, T2, T3> Empty = new Pair<T1, T2, T3>();

        public Pair(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
        public override string ToString()
        {
            return "Item1: " + Item1 + ", Item2: " + Item2 + ", Item3: " + Item3;
        }
    }
    public struct Pair<T1, T2, T3, T4>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public static Pair<T1, T2, T3, T4> Empty = new Pair<T1, T2, T3, T4>();

        public Pair(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
        public override string ToString()
        {
            return "Item1: " + Item1 + ", Item2: " + Item2 + ", Item3: " + Item3 + ", Item4: " + Item4;
        }
    }
    public struct Pair<T1, T2, T3, T4, T5>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;

        public static Pair<T1, T2, T3, T4, T5> Empty = new Pair<T1, T2, T3, T4, T5>();

        public Pair(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }
        public override string ToString()
        {
            return "Item1: " + Item1 + ", Item2: " + Item2 + ", Item3: " + Item3 + ", Item4: " + Item4 + ", Item5: " + Item5;
        }
    }

    public static class Pair
    {
        public static Pair<T1, T2> Create<T1, T2>(T1 item1, T2 item2) =>
            new Pair<T1, T2>(item1, item2);
        public static Pair<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) =>
            new Pair<T1, T2, T3>(item1, item2, item3);

        public static Pair<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) =>
            new Pair<T1, T2, T3, T4>(item1, item2, item3, item4);

        public static Pair<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) =>
            new Pair<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }
}
