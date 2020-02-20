using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    class FloodFill: GwsFloodFill
    {
        private List<ScanEntry> List;
        byte[] Data;

        public override byte this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        protected override void BeginInternal()
        {
            Data = new byte[Length];
            List = new List<ScanEntry>(Width);
        }
        protected override void EndInternal()
        {
            List = null;
            Data = null;
        }

        #region CHILD STRUCT
        struct ScanEntry
        {
            const string tostr = "Idx1: {0}, Idx2: {1}, Above: {2}";
            public int Idx1, Idx2;
            public int Above;

            public unsafe ScanEntry(int idx, int idx2, int w, int start, int x)
            {
                Idx1 = idx;
                Idx2 = idx2;
                Above = idx - w + (Idx2 - Idx1 - 1) / 2;
            }

            public int Length => Idx2 - Idx1;

            public unsafe bool IsAboveFilled(byte* data) =>
                data[Above] != 0;

            public bool Contains(int idx) =>
                idx >= Idx1 && idx <= Idx2;

            public override string ToString() =>
                string.Format(tostr, Idx1, Idx2, Above);
        }
        #endregion
    }
}
