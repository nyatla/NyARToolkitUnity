/* 
 * PROJECT: NyARToolkitCS
 * --------------------------------------------------------------------------------
 *
 * The NyARToolkitCS is C# edition NyARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 *
 * This work is based on the ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
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

    /**
     * このクラスは、入力サイズ制限の無いPerspectiveReaderです。
     *
     */
    public class NyARColorPatt_Perspective : NyARRgbRaster_INT1D_X8R8G8B8_32 , INyARColorPatt
    {
	    readonly private NyARIntPoint2d _edge=new NyARIntPoint2d();
	    /** サンプリング解像度*/
	    protected int _sample_per_pixel;

        /**
         * コンストラクタです。
         * エッジサイズ0,入力ラスタタイプの制限無しでインスタンスを作成します。
         *　高速化が必要な時は、入力ラスタタイプを制限するコンストラクタを使ってください。
         * @param i_width
         * 取得画像の解像度幅
         * @param i_height
         * 取得画像の解像度高さ
         * @param i_point_per_pix
         * 1ピクセルあたりの縦横サンプリング数。2なら2x2=4ポイントをサンプリングする。
         * @ 
         */
        public NyARColorPatt_Perspective(int i_width, int i_height, int i_point_per_pix):this(i_width,i_height,i_point_per_pix,0)
        {
            return;
        }
        /**
         * コンストラクタです。
         * エッジサイズ,入力ラスタタイプの制限を指定してインスタンスを作成します。
         * @param i_width
         * 取得画像の解像度幅
         * @param i_height
         * 取得画像の解像度高さ
         * @param i_point_per_pix
         * 1ピクセルあたりの解像度
         * @param i_edge_percentage
         * エッジ幅の割合(ARToolKit標準と同じなら、25)
         * @ 
         */
        public NyARColorPatt_Perspective(int i_width, int i_height, int i_point_per_pix, int i_edge_percentage):base(i_width,i_height,true)
        {
		    Debug.Assert(i_width>2 && i_height>2);
		    this._sample_per_pixel=i_point_per_pix;
            this._edge.setValue(i_edge_percentage, i_edge_percentage);
            return;
        }
        /**
         * 矩形領域のエッジ（枠）サイズを、割合で指定します。
         * @param i_x_percent
         * 左右のエッジの割合です。0から50の間の数で指定します。
         * @param i_y_percent
         * 上下のエッジの割合です。0から50の間の数で指定します。
         * @param i_sample_per_pixel
         * 1ピクセルあたりの縦横サンプリング数。2なら2x2=4ポイントをサンプリングする。
         */
        public void setEdgeSizeByPercent(int i_x_percent, int i_y_percent, int i_sample_per_pixel)
        {
            Debug.Assert(i_x_percent >= 0);
            Debug.Assert(i_y_percent >= 0);
            this._edge.setValue(i_x_percent, i_y_percent);
            this._sample_per_pixel = i_sample_per_pixel;
            return;
        }


        private INyARRgbRaster _last_input_raster = null;
        private INyARPerspectiveCopy _raster_driver;
        /**
         * この関数は、ラスタのi_vertexsで定義される四角形からパターンを取得して、インスタンスに格納します。
         */
        public bool pickFromRaster(INyARRgbRaster image, NyARIntPoint2d[] i_vertexs)
        {
            if (this._last_input_raster != image)
            {
                this._raster_driver = (INyARPerspectiveCopy)image.createInterface(typeof(INyARPerspectiveCopy));
                this._last_input_raster = image;
            }
            //遠近法のパラメータを計算
            return this._raster_driver.copyPatt(i_vertexs, this._edge.x, this._edge.y, this._sample_per_pixel, this);
        }

        override public object createInterface(Type iIid)
        {
            if (iIid == typeof(INyARPerspectiveCopy))
            {
                return NyARPerspectiveCopyFactory.createDriver(this);
            }
            if (iIid == typeof(NyARMatchPattDeviationColorData.IRasterDriver))
            {
                return NyARMatchPattDeviationColorData.RasterDriverFactory.createDriver(this);
            }
            throw new NyARRuntimeException();
        }
    }
}
