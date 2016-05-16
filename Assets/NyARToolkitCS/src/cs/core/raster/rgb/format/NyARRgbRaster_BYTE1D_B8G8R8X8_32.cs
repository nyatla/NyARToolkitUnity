using System;
using System.Diagnostics;
namespace jp.nyatla.nyartoolkit.cs.core
{


    public class NyARRgbRaster_BYTE1D_B8G8R8X8_32 : NyARRgbRaster
    {
        protected byte[] _buf;
        public NyARRgbRaster_BYTE1D_B8G8R8X8_32(int i_width, int i_height, bool i_is_alloc):base(i_width, i_height, i_is_alloc)
        {
            this._buf = i_is_alloc ? new byte[i_width * i_height * 4] : null;
        }
        sealed override public Object getBuffer()
        {
            return this._buf;
        }
        sealed override public int getBufferType()
        {
            return NyARBufferType.BYTE1D_B8G8R8X8_32;
        }
        sealed override public void wrapBuffer(Object i_buf)
        {
            Debug.Assert(!this._is_attached_buffer);// バッファがアタッチされていたら機能しない。
            //ラスタの形式は省略。
            this._buf = (byte[])i_buf;
        }
        sealed override public int[] getPixel(int i_x, int i_y, int[] o_rgb)
        {
            byte[] ref_buf = this._buf;
            int bp = (i_x + i_y * this._size.w) * 4;
            o_rgb[0] = (ref_buf[bp + 2] & 0xff);// R
            o_rgb[1] = (ref_buf[bp + 1] & 0xff);// G
            o_rgb[2] = (ref_buf[bp + 0] & 0xff);// B
            return o_rgb;
        }
        sealed override public int[] getPixelSet(int[] i_x, int[] i_y, int i_num, int[] o_rgb)
        {
            int bp;
            int width = this._size.w;
            byte[] ref_buf = this._buf;
            for (int i = i_num - 1; i >= 0; i--)
            {
                bp = (i_x[i] + i_y[i] * width) * 4;
                o_rgb[i * 3 + 0] = (ref_buf[bp + 2] & 0xff);// R
                o_rgb[i * 3 + 1] = (ref_buf[bp + 1] & 0xff);// G
                o_rgb[i * 3 + 2] = (ref_buf[bp + 0] & 0xff);// B
            }
            return o_rgb;
        }
        sealed override public void setPixel(int i_x, int i_y, int i_r, int i_g, int i_b)
        {
            byte[] ref_buf = this._buf;
            int bp = (i_x + i_y * this._size.w) * 4;
            ref_buf[bp + 2] = (byte)i_r;// R
            ref_buf[bp + 1] = (byte)i_g;// G
            ref_buf[bp + 0] = (byte)i_b;// B

        }
        sealed override public void setPixel(int i_x, int i_y, int[] i_rgb)
        {
            byte[] ref_buf = this._buf;
            int bp = (i_x + i_y * this._size.w) * 4;
            ref_buf[bp + 2] = (byte)i_rgb[0];// R
            ref_buf[bp + 1] = (byte)i_rgb[1];// G
            ref_buf[bp + 0] = (byte)i_rgb[2];// B
        }
        sealed override public void setPixels(int[] i_x, int[] i_y, int i_num, int[] i_intrgb)
        {
            NyARRuntimeException.notImplement();
        }

    }
}
