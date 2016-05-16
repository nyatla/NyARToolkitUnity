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
namespace jp.nyatla.nyartoolkit.cs.core
{


    public interface INyARRgb2GsFilter
    {
        /**
         * 元画像の指定範囲の矩形から、グレイスケール画像を生成して、i_rasterへコピーします。
         * 範囲が元画像の一部の場合、その部分だけをコピーします。
         * @param l
         * @param t
         * @param w
         * @param h
         * @param i_raster
         * @
         */
        void convertRect(int l, int t, int w, int h, INyARGrayscaleRaster i_raster);
        /**
         * 同一サイズの画像にグレースケール画像を生成します。
         * @param i_raster
         * @
         */
        void convert(INyARGrayscaleRaster i_raster);
    }
}
