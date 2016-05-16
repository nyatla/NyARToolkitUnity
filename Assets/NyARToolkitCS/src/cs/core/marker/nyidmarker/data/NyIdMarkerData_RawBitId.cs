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
namespace jp.nyatla.nyartoolkit.cs.nyidmarker
{


    /**
     * このクラスは、{@link NyIdMarkerDataEncoder_RawBitId}の出力するデータを
     * 格納します。
     */
    public class NyIdMarkerData_RawBitId : INyIdMarkerData
    {
        /** RawbitドメインのNyIdから作成したID値です。*/
        public long marker_id;
        /**
         * この関数は、i_targetのマーカデータとインスタンスのデータを比較します。
         * 引数には、{@link NyIdMarkerData_RawBitId}型のオブジェクトを指定してください。
         */
        public bool isEqual(INyIdMarkerData i_target)
        {
            NyIdMarkerData_RawBitId s = (NyIdMarkerData_RawBitId)i_target;
            return s.marker_id == this.marker_id;
        }
        /**
         * この関数は、i_sourceからインスタンスにマーカデータをコピーします。
         * 引数には、{@link NyIdMarkerData_RawBit}型のオブジェクトを指定してください。
         */
        public void copyFrom(INyIdMarkerData i_source)
        {
            NyIdMarkerData_RawBitId s = (NyIdMarkerData_RawBitId)i_source;
            this.marker_id = s.marker_id;
            return;
        }
    }
}
