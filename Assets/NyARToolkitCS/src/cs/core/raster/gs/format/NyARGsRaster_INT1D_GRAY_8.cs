/* 
 * PROJECT: NyARToolkit(Extension)
 * --------------------------------------------------------------------------------
 *
 * The NyARToolkit is Java edition ARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as publishe
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * For further information please contact.
 *	http://nyatla.jp/nyatoolkit/
 *	<airmail(at)ebony.plala.or.jp> or <nyatla(at)nyatla.jp>
 * 
 */
using System;
using System.Diagnostics;
namespace jp.nyatla.nyartoolkit.cs.core
{
    public class NyARGsRaster_INT1D_GRAY_8 : NyARGrayscaleRaster
    {
        protected int[] _buf;
        public NyARGsRaster_INT1D_GRAY_8(int i_width, int i_height, bool i_is_alloc):base(i_width, i_height, i_is_alloc)
        {
            this._buf = i_is_alloc ? new int[i_width * i_height] : null;
        }


        sealed override public Object getBuffer()
        {
            return this._buf;
        }
        sealed override public int getBufferType()
        {
            return NyARBufferType.INT1D_GRAY_8;
        }
        /**
         * この関数は、ラスタに外部参照バッファをセットします。
         * 外部参照バッファを持つインスタンスでのみ使用できます。内部参照バッファを持つインスタンスでは使用できません。
         */
        sealed override public void wrapBuffer(Object i_buf)
        {
            Debug.Assert(!this._is_attached_buffer);// バッファがアタッチされていたら機能しない。
            //ラスタの形式は省略。
            this._buf = (int[])i_buf;
        }
        sealed override public int[] getPixelSet(int[] i_x, int[] i_y, int i_n, int[] o_buf, int i_st_buf)
        {
            int bp;
            int w = this._size.w;
            int[] b = this._buf;
            for (int i = i_n - 1; i >= 0; i--)
            {
                bp = (i_x[i] + i_y[i] * w);
                o_buf[i_st_buf + i] = (b[bp]);
            }
            return o_buf;
        }
        sealed override public int getPixel(int i_x, int i_y)
        {
            int[] buf = (int[])this._buf;
            return buf[(i_x + i_y * this._size.w)];
        }
        sealed override public void setPixel(int i_x, int i_y, int i_gs)
        {
            this._buf[(i_x + i_y * this._size.w)] = i_gs;
        }
        sealed override public void setPixels(int[] i_x, int[] i_y, int i_num, int[] i_intgs)
        {
            int w = this._size.w;
            int[] r = this._buf;
            for (int i = i_num - 1; i >= 0; i--)
            {
                r[(i_x[i] + i_y[i] * w)] = i_intgs[i];
            }
        }
        /**
         * 全ピクセルの値の合計を得る
         * @return
         */
        public long allPixels()
        {
            long r = 0;
            for (int i = 0; i < 640 * 480; i++)
            {
                r += this._buf[i];
            }
            return r;
        }
    }
}
