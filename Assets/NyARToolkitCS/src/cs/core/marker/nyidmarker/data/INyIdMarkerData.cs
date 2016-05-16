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
     * このインタフェイスは、エンコード済みマーカデータ格納クラスの共通関数を定義します。
     */
    public interface INyIdMarkerData
    {
        /**
         * この関数は、i_targetのマーカデータとインスタンスのデータを比較します。
         * @param i_target
         * 比較するマーカオブジェクト
         * @return
         * 内容が等しいかの真偽値。等しければtrue
         */
        bool isEqual(INyIdMarkerData i_target);
        /**
         * この関数は、i_sourceからインスタンスにマーカデータをコピーします。
         * @param i_source
         * コピー元のオブジェクト。
         */
        void copyFrom(INyIdMarkerData i_source);
    }
}
