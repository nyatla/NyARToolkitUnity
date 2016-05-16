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
using System.Collections.Generic;
using jp.nyatla.nyartoolkit.cs.core;
namespace jp.nyatla.nyartoolkit.cs.markersystem.utils
{


    /**
     * このクラスは、複数の異なる解像度の比較画像を保持します。
     */
    public class MultiResolutionPattProvider
    {
        private class Item
        {
            public INyARRgbRaster _patt;
            public NyARMatchPattDeviationColorData _patt_d;
            public int _patt_edge;
            public int _patt_resolution;
            public Item(int i_patt_w, int i_patt_h, int i_edge_percentage)
            {
                int r = 1;
                //解像度は幅を基準にする。
                while (i_patt_w * r < 64)
                {
                    r *= 2;
                }
                this._patt = NyARRgbRaster.createInstance(i_patt_w, i_patt_h, NyARBufferType.INT1D_X8R8G8B8_32, true);
                this._patt_d = new NyARMatchPattDeviationColorData(i_patt_w, i_patt_h);
                this._patt_edge = i_edge_percentage;
                this._patt_resolution = r;
            }
        }
        /**
         * インスタンスのキャッシュ
         */
        private List<Item> items = new List<Item>();
        /**
         * [readonly]マーカにマッチした{@link NyARMatchPattDeviationColorData}インスタンスを得る。
         * @ 
         */
        public NyARMatchPattDeviationColorData getDeviationColorData(ARMarkerList.Item i_marker, INyARPerspectiveCopy i_pix_drv, NyARIntPoint2d[] i_vertex)
        {
            int mk_edge = i_marker.patt_edge_percentage;
            for (int i = this.items.Count - 1; i >= 0; i--)
            {
                Item ptr = this.items[i];
                if (!ptr._patt.getSize().isEqualSize(i_marker.patt_w, i_marker.patt_h) || ptr._patt_edge != mk_edge)
                {
                    //サイズとエッジサイズが合致しない物はスルー
                    continue;
                }
                //古かったら更新
                i_pix_drv.copyPatt(i_vertex, ptr._patt_edge, ptr._patt_edge, ptr._patt_resolution, ptr._patt);
                ptr._patt_d.setRaster(ptr._patt);
                return ptr._patt_d;
            }
            //無い。新しく生成
            Item item = new Item(i_marker.patt_w, i_marker.patt_h, mk_edge);
            //タイムスタンプの更新とデータの生成
            i_pix_drv.copyPatt(i_vertex, item._patt_edge, item._patt_edge, item._patt_resolution, item._patt);
            item._patt_d.setRaster(item._patt);
            this.items.Add(item);
            return item._patt_d;
        }

    }
}
