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


    /**
     * このインタフェイスは、ラスタから射影変換画像を取得するインタフェイスを提供します。
     *
     */
    public interface INyARPerspectiveCopy
    {
        /**
         * この関数は、i_outへパターンを出力します。
         * @param i_vertex
         * @param i_edge_x
         * @param i_edge_y
         * @param i_resolution
         * @param i_out
         * @return
         * @
         */
        bool copyPatt(NyARIntPoint2d[] i_vertex, int i_edge_x, int i_edge_y, int i_resolution, INyARRgbRaster i_out);
        bool copyPatt(NyARDoublePoint2d[] i_vertex, int i_edge_x, int i_edge_y, int i_resolution, INyARRgbRaster i_out);
        bool copyPatt(double i_x1, double i_y1, double i_x2, double i_y2, double i_x3, double i_y3, double i_x4, double i_y4, int i_edge_x, int i_edge_y, int i_resolution, INyARRgbRaster i_out);
    }
}
