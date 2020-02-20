using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public abstract class GwsSound : ISound
    {
        public bool Loop { get; set; }
        public abstract void Load(string file);
        public abstract bool Play();
        public bool IsDisposed { get; protected set; }

        public abstract void Pause();
        public abstract void Stop();

        public abstract void Dispose();
        public abstract void Quit();
    }

}
