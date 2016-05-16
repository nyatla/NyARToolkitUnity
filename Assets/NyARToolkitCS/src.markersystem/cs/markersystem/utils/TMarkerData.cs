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
namespace jp.nyatla.nyartoolkit.cs.markersystem.utils
{


    /**
     * マーカ情報を格納するためのクラスです。
     */
    public class TMarkerData
    {
        /** 最後に認識したタイムスタンプ。*/
        public long time_stamp;
        /** ライフ値
         * マーカ検出時にリセットされ、1フレームごとに1づつインクリメントされる値です。
         */
        public long life;
        /** MK情報。マーカのオフセット位置。*/
        public readonly NyARRectOffset marker_offset = new NyARRectOffset();
        /** 検出した矩形の格納変数。理想形二次元座標を格納します。*/
        public SquareStack.Item sq;
        /** 検出した矩形の格納変数。マーカの姿勢行列を格納します。*/
	    public readonly NyARDoubleMatrix44 tmat=new NyARDoubleMatrix44();
	    /** */
        public readonly NyARTransMatResultParam last_param = new NyARTransMatResultParam();
        /** 矩形の検出状態の格納変数。 連続して見失った回数を格納します。*/
        public int lost_count = int.MaxValue;
        /** トラッキングログ用の領域*/
        public NyARIntPoint2d[] tl_vertex = NyARIntPoint2d.createArray(4);
        public NyARIntPoint2d tl_center = new NyARIntPoint2d();
        public int tl_rect_area;
        protected TMarkerData()
        {
            this.life = 0;
        }
    }
}
