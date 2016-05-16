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
using System.Diagnostics;
using System.IO;
namespace jp.nyatla.nyartoolkit.cs.markersystem
{
    public class NyARMarkerSystemConfig : INyARMarkerSystemConfig
    {
	    /** ARToolkit v2互換のニュートン法を使った変換行列計算アルゴリズムを選択します。*/
	    public const int TM_ARTKV2=1;
	    /** NyARToolKitの偏微分を使った変換行列アルゴリズムです。*/
	    public const int TM_NYARTK=2;
	    /** ARToolkit v4に搭載されているICPを使った変換行列計算アルゴリズムを選択します。*/
    	public const int TM_ARTKICP=3;
        //
        readonly protected NyARSingleCameraView _cview;
    	private int _transmat_algo_type;
        /**
         * コンストラクタです。カメラパラメータにサンプル値(../Data/camera_para.dat)をロードして、コンフィギュレーションを生成します。
         * @param i_width
         * @param i_height
         * @
         */
        public NyARMarkerSystemConfig(NyARSingleCameraView i_view, int i_transmat_algo_type)
        {
            Debug.Assert(1 <= i_transmat_algo_type && i_transmat_algo_type <= 3);
            this._cview = i_view;
            this._transmat_algo_type = i_transmat_algo_type;
            return;
        }
        public NyARMarkerSystemConfig(NyARParam i_param, int i_transmat_algo_type)
            : this(new NyARSingleCameraView(i_param), i_transmat_algo_type)
        {
        }
        public NyARMarkerSystemConfig(NyARParam i_param)
            : this(i_param, TM_ARTKICP)
        {
        }
        public NyARMarkerSystemConfig(Stream i_ar_param_stream, int i_width, int i_height)
            : this(NyARParam.loadFromARParamFile(i_ar_param_stream, i_width, i_height))
        {
        }

        public NyARMarkerSystemConfig(int i_width, int i_height)
            : this(NyARParam.loadDefaultParams(i_width, i_height))
        {
        }

	    /**
	     * この値は、カメラパラメータのスクリーンサイズです。
	     */
	    public NyARIntSize getScreenSize()
	    {
		    return this._cview.getARParam().getScreenSize();
	    }
	
	    public NyARSingleCameraView getNyARSingleCameraView() {
		    return this._cview;
	    }
        public virtual INyARTransMat createTransmatAlgorism()
        {
            NyARParam params_=this._cview.getARParam();
            switch (this._transmat_algo_type)
            {
                case TM_ARTKV2:
                    return new NyARTransMat_ARToolKit(params_);
                case TM_NYARTK:
                    return new NyARTransMat(params_);
                case TM_ARTKICP:
                    return new NyARIcpTransMat(params_, NyARIcpTransMat.AL_POINT_ROBUST);
            }
            throw new NyARRuntimeException();
        }
        public virtual INyARHistogramAnalyzer_Threshold createAutoThresholdArgorism()
        {
            return new NyARHistogramAnalyzer_SlidePTile(15);
        }

    }
}
