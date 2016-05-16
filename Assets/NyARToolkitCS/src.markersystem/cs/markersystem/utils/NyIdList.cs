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
using jp.nyatla.nyartoolkit.cs.nyidmarker;
namespace jp.nyatla.nyartoolkit.cs.markersystem.utils
{


    public class NyIdList : List<NyIdList.Item>
    {
	    public class Item : TMarkerData
	    {
		    /** MK_NyIdの情報。 反応するidの開始レンジ*/
		    public long nyid_range_s;
		    /** MK_NyIdの情報。 反応するidの終了レンジ*/
		    public long nyid_range_e;
		    /** MK_NyIdの情報。 実際のid値*/
		    public long nyid;
		    public int dir;
		    /**
		     * コンストラクタです。初期値から、Idマーカのインスタンスを生成します。
		     * @param i_range_s
		     * @param i_range_e
		     * @param i_patt_size
		     * @throws NyARException
		     */
		    public Item(long i_nyid_range_s,long i_nyid_range_e,double i_patt_size):base()
		    {
			    this.marker_offset.setSquare(i_patt_size);
			    this.nyid_range_s=i_nyid_range_s;
			    this.nyid_range_e=i_nyid_range_e;
			    return;
		    }		
	    }
        /**輪郭推定器*/
        private NyIdMarkerPickup _id_pickup;
        private readonly NyIdMarkerPattern _id_patt = new NyIdMarkerPattern();
        private readonly NyIdMarkerParam _id_param = new NyIdMarkerParam();
        private readonly NyIdMarkerDataEncoder_RawBitId _id_encoder = new NyIdMarkerDataEncoder_RawBitId();
        private readonly NyIdMarkerData_RawBitId _id_data = new NyIdMarkerData_RawBitId();
        public NyIdList()
        {
            this._id_pickup = new NyIdMarkerPickup();
        }
        public void prepare()
        {
            //nothing to do
            //sqはtrackingでnull初期化済み
        }
        public bool update(INyARGrayscaleRaster i_raster, SquareStack.Item i_sq)
        {
            if (!this._id_pickup.pickFromRaster(i_raster, i_sq.ob_vertex, this._id_patt, this._id_param))
            {
                return false;
            }
            if (!this._id_encoder.encode(this._id_patt, this._id_data))
            {
                return false;
            }
            //IDを検出
            long s = this._id_data.marker_id;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Item target = this[i];
                if (target.nyid_range_s > s || s > target.nyid_range_e)
                {
                    continue;
                }
                //既に認識済なら無視
                if (target.lost_count == 0)
                {
                    continue;
                }
                //一致したよー。
                target.nyid = s;
                target.dir = this._id_param.direction;
                target.sq = i_sq;
                return true;
            }
            return false;
        }
        public void finish()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Item target = this[i];
                if (target.sq == null)
                {
                    continue;
                }
                if (target.lost_count > 0)
                {
                    //参照はそのままで、dirだけ調整する。
                    target.lost_count = 0;
                    target.life++;
                    target.sq.rotateVertexL(4 - target.dir);
                    NyARIntPoint2d.shiftCopy(target.sq.ob_vertex, target.tl_vertex, 4 - target.dir);
                    target.tl_center.setValue(target.sq.center2d);
                    target.tl_rect_area = target.sq.rect_area;
                }
            }
        }
    }
}
