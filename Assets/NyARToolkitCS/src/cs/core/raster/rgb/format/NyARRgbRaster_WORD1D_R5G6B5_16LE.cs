using System;
using System.Diagnostics;
namespace jp.nyatla.nyartoolkit.cs.core
{


    public class NyARRgbRaster_WORD1D_R5G6B5_16LE : NyARRgbRaster
    {
        protected short[] _buf;
        public NyARRgbRaster_WORD1D_R5G6B5_16LE(int i_width, int i_height, bool i_is_alloc):base(i_width, i_height, i_is_alloc)
        {
            this._buf = i_is_alloc ? new short[i_width * i_height] : null;
        }
        sealed override public Object getBuffer()
        {
            return this._buf;
        }
        sealed override public int getBufferType()
        {
            return NyARBufferType.WORD1D_R5G6B5_16LE;
        }
        sealed override public void wrapBuffer(Object i_buf)
        {
            Debug.Assert(!this._is_attached_buffer);// バッファがアタッチされていたら機能しない。
            //ラスタの形式は省略。
            this._buf = (short[])i_buf;

        }
        sealed override public int[] getPixel(int i_x, int i_y, int[] o_rgb)
        {
            short[] buf = this._buf;
            int y = i_y;
            int idx = y * this._size.w + i_x;
            int pixcel = (int)(buf[idx] & 0xffff);

            o_rgb[0] = (int)((pixcel & 0xf800) >> 8);// R
            o_rgb[1] = (int)((pixcel & 0x07e0) >> 3);// G
            o_rgb[2] = (int)((pixcel & 0x001f) << 3);// B
            return o_rgb;
        }
        sealed override public int[] getPixelSet(int[] i_x, int[] i_y, int i_num, int[] o_rgb)
        {
            int stride = this._size.w;
            short[] buf = this._buf;

            for (int i = i_num - 1; i >= 0; i--)
            {
                int idx = i_y[i] * stride + i_x[i];

                int pixcel = (int)(buf[idx] & 0xffff);
                o_rgb[i * 3 + 0] = (int)((pixcel & 0xf800) >> 8);// R
                o_rgb[i * 3 + 1] = (int)((pixcel & 0x07e0) >> 3);// G
                o_rgb[i * 3 + 2] = (int)((pixcel & 0x001f) << 3);// B
            }
            return o_rgb;
        }
        sealed override public void setPixel(int i_x, int i_y, int i_r, int i_g, int i_b)
        {
            NyARRuntimeException.notImplement();
        }
        sealed override public void setPixel(int i_x, int i_y, int[] i_rgb)
        {
            NyARRuntimeException.notImplement();
        }
        sealed override public void setPixels(int[] i_x, int[] i_y, int i_num, int[] i_intrgb)
        {
            NyARRuntimeException.notImplement();
        }
    }
}
