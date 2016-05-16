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
namespace jp.nyatla.nyartoolkit.cs.core
{



    /**
     * 回転行列計算用の、3x3行列
     * 計算方法はARToolKitと同じだが、ARToolKitにある不要な行列から角度を逆算する
     * 処理を省略しているため、下位12桁目の計算値が異なる。
     *
     */
    public class NyARRotMatrix_ARToolKit_O2 : NyARRotMatrix_ARToolKit
    {
        /**
         * コンストラクタです。
         * 参照する射影変換オブジェクトを指定して、インスタンスを生成します。
         * @param i_matrix
         * 参照する射影変換オブジェクト
         * @
         */
        public NyARRotMatrix_ARToolKit_O2(NyARPerspectiveProjectionMatrix i_matrix):base(i_matrix)
        {
            return;
        }
        //override
        public override void setAngle(double i_x, double i_y, double i_z)
        {
            double sina = Math.Sin(i_x);
            double cosa = Math.Cos(i_x);
            double sinb = Math.Sin(i_y);
            double cosb = Math.Cos(i_y);
            double sinc = Math.Sin(i_z);
            double cosc = Math.Cos(i_z);
            // Optimize
            double CACA = cosa * cosa;
            double SASA = sina * sina;
            double SACA = sina * cosa;
            double SASB = sina * sinb;
            double CASB = cosa * sinb;
            double SACACB = SACA * cosb;

            this.m00 = CACA * cosb * cosc + SASA * cosc + SACACB * sinc - SACA * sinc;
            this.m01 = -CACA * cosb * sinc - SASA * sinc + SACACB * cosc - SACA * cosc;
            this.m02 = CASB;
            this.m10 = SACACB * cosc - SACA * cosc + SASA * cosb * sinc + CACA * sinc;
            this.m11 = -SACACB * sinc + SACA * sinc + SASA * cosb * cosc + CACA * cosc;
            this.m12 = SASB;
            this.m20 = -CASB * cosc - SASB * sinc;
            this.m21 = CASB * sinc - SASB * cosc;
            this.m22 = cosb;
            //angleを逆計算せずに直接代入
            this._angle.x = i_x;
            this._angle.y = i_y;
            this._angle.z = i_z;
            return;
        }

    }
}
