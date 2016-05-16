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
     * このクラスは、{@link NyARLabeling_ARToolKit}クラスを用いた矩形検出器です。
     * 検出した矩形を、自己コールバック関数{@link #onSquareDetect}へ通知します。
     * 継承クラスで自己コールバック関数{@link #onSquareDetect}を実装する必要があります。
     */
    public abstract class NyARSquareContourDetector_ARToolKit : NyARSquareContourDetector
    {
        private const int AR_AREA_MAX = 100000;// #define AR_AREA_MAX 100000
        private const int AR_AREA_MIN = 70;// #define AR_AREA_MIN 70
        private readonly int _width;
        private readonly int _height;

        private readonly NyARLabeling_ARToolKit _labeling;

        private readonly NyARLabelingImage _limage;

        private readonly NyARLabelOverlapChecker<NyARLabelingLabel> _overlap_checker = new NyARLabelOverlapChecker<NyARLabelingLabel>(32);
        private readonly NyARContourPickup_ARToolKit _cpickup = new NyARContourPickup_ARToolKit();
        private readonly NyARCoord2SquareVertexIndexes _coord2vertex = new NyARCoord2SquareVertexIndexes();

        private readonly NyARIntCoordinates _coord;
        private readonly int[] __detectMarker_mkvertex = new int[4];
        /**
         * コンストラクタです。
         * 入力画像のサイズを指定して、インスタンスを生成します。
         * @param i_size
         * 入力画像のサイズ
         */
        public NyARSquareContourDetector_ARToolKit(NyARIntSize i_size)
        {
            this._width = i_size.w;
            this._height = i_size.h;
            this._labeling = new NyARLabeling_ARToolKit();
            this._limage = new NyARLabelingImage(this._width, this._height);

            // 輪郭の最大長は画面に映りうる最大の長方形サイズ。
            int number_of_coord = (this._width + this._height) * 2;

            // 輪郭バッファは頂点変換をするので、輪郭バッファの２倍取る。
            this._coord = new NyARIntCoordinates(number_of_coord);
            return;
        }
        /**
         * この関数は、ラスタから矩形を検出して、自己コールバック関数{@link #onSquareDetect}で通知します。
         * 実装クラスでは、矩形検出処理をして、結果を通知する処理を実装してください。
         * @param i_raster
         * 検出元のラスタ画像
         * @
         */
        public void detectMarker(INyARBinRaster i_raster, NyARSquareContourDetector.CbHandler i_cb)
        {
            NyARLabelingImage limage = this._limage;

            // ラベル数が0ならここまで
            int label_num = this._labeling.labeling(i_raster, this._limage);
            if (label_num < 1)
            {
                return;
            }

            NyARLabelingLabelStack stack = limage.getLabelStack();
            //ラベルをソートしておく
            stack.sortByArea();
            //
            NyARLabelingLabel[] labels = stack.getArray();

            // デカいラベルを読み飛ばし
            int i;
            for (i = 0; i < label_num; i++)
            {
                // 検査対象内のラベルサイズになるまで無視
                if (labels[i].area <= AR_AREA_MAX)
                {
                    break;
                }
            }
            int xsize = this._width;
            int ysize = this._height;
            NyARIntCoordinates coord = this._coord;
            int[] mkvertex = this.__detectMarker_mkvertex;

            NyARLabelOverlapChecker<NyARLabelingLabel> overlap = this._overlap_checker;

            //重なりチェッカの最大数を設定
            overlap.setMaxLabels(label_num);
            for (; i < label_num; i++)
            {
                NyARLabelingLabel label_pt = labels[i];
                int label_area = label_pt.area;
                // 検査対象サイズよりも小さくなったら終了
                if (label_area < AR_AREA_MIN)
                {
                    break;
                }
                // クリップ領域が画面の枠に接していれば除外
                if (label_pt.clip_l == 1 || label_pt.clip_r == xsize - 2)
                {// if(wclip[i*4+0] == 1 || wclip[i*4+1] ==xsize-2){
                    continue;
                }
                if (label_pt.clip_t == 1 || label_pt.clip_b == ysize - 2)
                {// if( wclip[i*4+2] == 1 || wclip[i*4+3] ==ysize-2){
                    continue;
                }
                // 既に検出された矩形との重なりを確認
                if (!overlap.check(label_pt))
                {
                    // 重なっているようだ。
                    continue;
                }
                // 輪郭を取得
                if (!this._cpickup.getContour(limage, limage.getTopClipTangentX(label_pt), label_pt.clip_t, coord))
                {
                    continue;
                }
                //輪郭線をチェックして、矩形かどうかを判定。矩形ならばmkvertexに取得
                if (!this._coord2vertex.getVertexIndexes(coord, label_area, mkvertex))
                {
                    // 頂点の取得が出来なかった
                    continue;
                }
                //矩形を発見したことをコールバック関数で通知
                i_cb.detectMarkerCallback(coord, mkvertex);

                // 検出済の矩形の属したラベルを重なりチェックに追加する。
                overlap.push(label_pt);

            }
            return;
        }

    }
}


