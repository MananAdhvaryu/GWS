using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
    sealed class StbImageProcessor: IImageProcessor
    {
        internal StbImageProcessor() { }
        public void Dispose()
        {
             
        }

        public IImageReader Reader => STBImage.Reader;
        public IImageWriter Writer => STBImage.Writer;
    }
#if AllHidden
    }
#endif
}
