using System;

namespace MnM.GWS
{
    public abstract class GwsUploadable: IUploadable
    {
        protected GwsUploadable()
        {
            ID = Implementation.Factory.NewID(this);
        }

        #region PROPERTIES
        public string ID { get; private set; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        #endregion

        #region UPLOAD
        public abstract void Upload();
        public abstract void Upload(IRectangle rectangle);
        public abstract void Upload(IRectangle copyRc, IRectangle destRc);
        #endregion

        #region DOWNLOAD
        public abstract void Download();
        public abstract void Download(IRectangle rectangle);
        #endregion

        #region GET OUTPUT SIZE
        protected abstract void GetOutputSize(out int w, out int h);
        #endregion

        #region CORRECT REGIONS
        protected bool CorrectRegions(IRectangle sourceRc, IRectangle destRc, out IRectangle srcRc, out IRectangle dstRc)
        {
            var sw = sourceRc?.Width ?? Width;
            var sh = sourceRc?.Height ?? Height;

            sw = Math.Min(sw, Width);
            sh = Math.Min(sh, Height);

            GetOutputSize(out int rw, out int rh);

            var dw = destRc?.Width ?? Width;
            var dh = destRc?.Height ?? Height;

            dw = Math.Min(dw, Width);
            dh = Math.Min(dh, Height);

            srcRc = Geometry.CompitibleRC(Width, Height, sourceRc?.X, sourceRc?.Y, sw, sh);
            dstRc = Geometry.CompitibleRC(rw, rh, destRc?.X, destRc?.Y, dw, dh);

            if (destRc.Width == 0 || destRc.Height == 0 || srcRc.Width == 0 || srcRc.Height == 0)
                return false;
            return true;
        }
        #endregion

        void IStoreable.AssignIDIfNone() { }
    }
}
