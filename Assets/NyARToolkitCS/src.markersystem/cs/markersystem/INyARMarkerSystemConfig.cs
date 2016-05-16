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
     * このインタフェイスは、NyARMarkerSystemのコンフィギュレーションインタフェイスを定義します。
     *
     */
    public interface INyARMarkerSystemConfig
    {
        /**
         * 姿勢変換アルゴリズムクラスのオブジェクトを生成して返します。
         * @return
         * @
         */
        INyARTransMat createTransmatAlgorism();
        /**
         * 敷居値決定クラスを生成して返します。
         * @return
         * @
         */
        INyARHistogramAnalyzer_Threshold createAutoThresholdArgorism();
		/**
		 * このコンフィギュレーションのスクリーンサイズを返します。
		 * @return
		 * [readonly]
		 * 参照値です。
		 */
		NyARIntSize getScreenSize();
        NyARSingleCameraView getNyARSingleCameraView();
    }
}
