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
using jp.nyatla.nyartoolkit.cs.core;
namespace jp.nyatla.nyartoolkit.cs.markersystem
{


    /**
     * MarkerSystemの入力データを管理するベースクラスです。生データのスナップショット管理を行います。
     */
    public class NyARSensor
    {
        protected NyARHistogram _gs_hist;
        protected INyARRgbRaster _ref_raster = null;
        protected INyARGrayscaleRaster _gs_raster;
        protected long _src_ts;
        protected long _gs_id_ts;
        protected long _gs_hist_ts;
        public NyARSensor(NyARIntSize i_size)
        {
            this._gs_raster = NyARGrayscaleRaster.createInstance(i_size.w, i_size.h, NyARBufferType.INT1D_GRAY_8, true);
            this._gs_hist = new NyARHistogram(256);
            this._src_ts = 0;
            this._gs_id_ts = 0;
            this._gs_hist_ts = 0;
            this._hist_drv = (INyARHistogramFromRaster)this._gs_raster.createInterface(typeof(INyARHistogramFromRaster));
        }


        /**
         * キャッシュしている射影変換ドライバを返します。
         * この関数は、内部処理向けの関数です。
         * @return
         * [readonly]
         */
        public INyARPerspectiveCopy getPerspectiveCopy()
        {
            return this._pcopy;
        }
        private INyARHistogramFromRaster _hist_drv = null;
        private INyARPerspectiveCopy _pcopy;
        private INyARRgb2GsFilter _rgb2gs = null;
        /**
         * この関数は、入力画像を元に、インスタンスの状態を更新します。
         * この関数は、タイムスタンプをインクリメントします。
         * @param i_input
         * @ 
         */
        public virtual void update(INyARRgbRaster i_input)
        {
            //ラスタドライバの準備
            if (this._ref_raster != i_input)
            {
                this._rgb2gs = (INyARRgb2GsFilter)i_input.createInterface(typeof(INyARRgb2GsFilter));
                this._pcopy = (INyARPerspectiveCopy)i_input.createInterface(typeof(INyARPerspectiveCopy));
                this._ref_raster = i_input;
            }
            //ソースidのインクリメント
            this._src_ts++;
        }
        /**
         * この関数は、タイムスタンプを強制的にインクリメントします。
         */
        public void updateTimeStamp()
        {
            this._src_ts++;
        }
        /**
         * 現在のタイムスタンプを返します。
         * @return
         */
        public long getTimeStamp()
        {
            return this._src_ts;
        }

        /**
         * この関数は、グレースケールに変換した現在の画像を返します。
         * @return
         * @ 
         */
        public INyARGrayscaleRaster getGsImage()
        {
            //必要に応じてグレースケール画像の生成
            if (this._src_ts != this._gs_id_ts)
            {
                this._rgb2gs.convert(this._gs_raster);
                this._gs_id_ts = this._src_ts;
            }
            return this._gs_raster;
            //
        }
        /**
         * この関数は、現在のGS画像のﾋｽﾄｸﾞﾗﾑを返します。
         * @ 
         */
        public NyARHistogram getGsHistogram()
        {
            //必要に応じてヒストグラムを生成
            if (this._gs_id_ts != this._gs_hist_ts)
            {
                this._hist_drv.createHistogram(4, this._gs_hist);
                this._gs_hist_ts = this._gs_id_ts;
            }
            return this._gs_hist;
        }
        /**
         * 現在の入力画像の参照値を返します。
         * @return
         */
        public INyARRgbRaster getSourceImage()
        {
            return this._ref_raster;
        }

        /**
         * 任意の4頂点領域を射影変換して取得します。
         * @param i_x1
         * @param i_y1
         * @param i_x2
         * @param i_y2
         * @param i_x3
         * @param i_y3
         * @param i_x4
         * @param i_y4
         * @return
         * @ 
         */
        public INyARRgbRaster getPerspectiveImage(
            int i_x1, int i_y1,
            int i_x2, int i_y2,
            int i_x3, int i_y3,
            int i_x4, int i_y4,
            INyARRgbRaster i_raster)
        {
            this._pcopy.copyPatt(i_x1, i_y1, i_x2, i_y2, i_x3, i_y3, i_x4, i_y4, 0, 0, 1, i_raster);
            return i_raster;
        }
        /**
         * 任意の4頂点領域を射影変換して取得します。
         * @param i_x1
         * @param i_y1
         * @param i_x2
         * @param i_y2
         * @param i_x3
         * @param i_y3
         * @param i_x4
         * @param i_y4
         * @param i_raster
         * @return
         * @
         */
        public INyARRgbRaster getPerspectiveImage(
                double i_x1, double i_y1,
                double i_x2, double i_y2,
                double i_x3, double i_y3,
                double i_x4, double i_y4,
                INyARRgbRaster i_raster)
        {
            this._pcopy.copyPatt(i_x1, i_y1, i_x2, i_y2, i_x3, i_y3, i_x4, i_y4, 0, 0, 1, i_raster);
            return i_raster;
        }
    }
}
