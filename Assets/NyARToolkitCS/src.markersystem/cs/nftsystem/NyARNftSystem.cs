using jp.nyatla.nyartoolkit.cs.core;
using System.Collections.Generic;
using System;
using System.Threading;
namespace jp.nyatla.nyartoolkit.cs.markersystem
{

    /**
     * NFTの自然特徴点マーカの管理クラスです。
     * ARToolKit version5互換のNFTマーカをトラッキングできます。
     */
    public class NyARNftSystem : NyARSingleCameraSystem
    {

        /**
         * コンストラクタです。
         * @param i_config
         * NFTのコンフィギュレーションオブジェクトを指定します。
         */
        public NyARNftSystem(INyARNftSystemConfig i_config)
            : base(i_config.getNyARSingleCameraView())
        {
            NyARParam cparam = this._view.getARParam();
            this._surface_tracker = new NyARSurfaceTracker(cparam, 16, 0.5);
            this._sftrackingutils = new NyARSurfaceTrackingTransmatUtils(cparam, 5.0);
            this._kpm_worker = new KpmWorker(cparam, this._nftdatalist);
            this._kpm_thread = new Thread(this._kpm_worker.run);
            this._kpm_thread.Start(this._kpm_thread);
        }
        private long _last_time_stamp = -1;
        private class NftTarget
        {
            /** KPMによる初期検出対象*/
            public const int ST_KPM_SEARCH = 1;
            public const int ST_KPM_FOUND = 2;
            /** AR2SURFACEトラッキングによる検出状態維持*/
            public const int ST_AR2_TRACKING = 3;
            public int stage;
            /** 参照定義セット*/
            public NyARNftDataSet dataset;
            /** KPMスレッドの出力結果*/
            public NyARDoubleMatrix44 back_transmat = new NyARDoubleMatrix44();
            /** transmat_backが更新されたかのフラグ*/
            public bool back_has_result;

            /** メインスレッド用の出力結果*/
            public NyARDoubleMatrix44 front_transmat = new NyARDoubleMatrix44();
            /** コンストラクタ*/
            public NftTarget(NyARNftDataSet i_dataset)
            {
                this.dataset = i_dataset;
                this.stage = ST_KPM_SEARCH;
                this.back_has_result = false;
            }
        }

        private class NyARNftTargetList : List<NftTarget>
        {
            private const long serialVersionUID = 2150347966734138642L;
        }
        readonly private Thread _kpm_thread;
        readonly private KpmWorker _kpm_worker;
        readonly private NyARNftTargetList _nftdatalist = new NyARNftTargetList();
        readonly private NyARSurfaceTrackingTransmatUtils _sftrackingutils;

        readonly private NyARSurfaceTracker _surface_tracker;

        //WORK AREA
        readonly private NyARDoublePoint2d[] _pos2d = NyARDoublePoint2d.createArray(16);
        readonly private NyARDoublePoint3d[] _pos3d = NyARDoublePoint3d.createArray(16);
        readonly private NyARTransMatResultParam result_param = new NyARTransMatResultParam();

        /**
         * この関数は、入力した画像でインスタンスの状態を更新します。
         * 関数は、入力画像を処理して検出、一致判定、トラッキング処理を実行します。
         * @param i_sensor
         * 画像を含むセンサオブジェクト
         */
        public void update(NyARSensor i_sensor)
        {
            long time_stamp = i_sensor.getTimeStamp();
            //センサのタイムスタンプが変化していなければ何もしない。
            if (this._last_time_stamp == time_stamp)
            {
                return;
            }
            //タイムスタンプの更新
            this._last_time_stamp = time_stamp;

            //ステータス遷移


            NyARDoublePoint2d[] pos2d = this._pos2d;
            NyARDoublePoint3d[] pos3d = this._pos3d;
            INyARGrayscaleRaster gs = i_sensor.getGsImage();

            //KPMスレッドによる更新()
            this._kpm_worker.updateInputImage(gs);

            //SurfaceTrackingによるfrontデータの更新

            foreach (NftTarget target in this._nftdatalist)
            {
                if (target.stage < NftTarget.ST_KPM_FOUND)
                {
                    //System.out.println("NOTHING");
                    //KPM検出前なら何もしない。
                    continue;
                }
                switch (target.stage)
                {
                    case NftTarget.ST_AR2_TRACKING:
                        //NftTarget.ST_AR2_TRACKING以降				
                        //front_transmatに作る。
                        NyARSurfaceTracker st = this._surface_tracker;
                        int nop = st.tracking(gs, target.dataset.surface_dataset, target.front_transmat, this._pos2d, pos3d, 16);
                        if (nop == 0)
                        {
                            //失敗
                            target.stage = NftTarget.ST_KPM_SEARCH;
                            //System.out.println("ST_KPM_SEARCH");
                            continue;
                        }
                        //Transmatの試験
                        NyARDoublePoint3d off = NyARSurfaceTrackingTransmatUtils.centerOffset(pos3d, nop, new NyARDoublePoint3d());
                        NyARSurfaceTrackingTransmatUtils.modifyInputOffset(target.front_transmat, pos3d, nop, off);//ARTK5の補正
                        if (!this._sftrackingutils.surfaceTrackingTransmat(target.front_transmat, pos2d, pos3d, nop, target.front_transmat, this.result_param))
                        {
                            //失敗
                            target.stage = NftTarget.ST_KPM_SEARCH;
                            //System.out.println("ST_KPM_SEARCH");
                            continue;
                        }
                        NyARSurfaceTrackingTransmatUtils.restoreOutputOffset(target.front_transmat, off);//ARTK5の補正
                        break;
                    case NftTarget.ST_KPM_FOUND:
                        target.stage = NftTarget.ST_AR2_TRACKING;
                        //System.out.println("ST_AR2_TRACKING");
                        break;
                }
            }
        }
        /**
         * NFTファイルセットのプレフィックスを指定して、NFTターゲットをインスタンスに登録します。
         * 登録される画像のサイズはNFTターゲットファイルの値です。
         * @param i_fileset_prefix
         * NFTターゲットのファイルパスのプレフィクス。ファイル名+.iset,.fset,.fset3をセットにして登録します。
         * @return
         * 特徴点セットのID値
         */
        public int addNftTarget(String i_fileset_prefix)
        {
            return this.addNftTarget(NyARNftDataSet.loadFromNftFiles(i_fileset_prefix));
        }
        /**
         * 画像のサイズを指定できる{@link #addNftTarget}です。
         * @param i_fileset_prefix
         * {@link #addNftTarget(String)}を参照してください。
         * @param i_width_in_msec
         * 画像サイズの横幅をmm単位で指定します。立幅は横幅に応じてスケーリングされます。
         * @return
         * 特徴点セットのID値
         */
        public int addNftTarget(String i_fileset_prefix, double i_width_in_msec)
        {
            return this.addNftTarget(NyARNftDataSet.loadFromNftFiles(i_fileset_prefix, i_width_in_msec));
        }
        /**
         * 生成済みのNFTの特徴点データセットをインスタンスに登録します。
         * @param i_dataset
         * 登録する特徴点データセット
         * @return
         * 特徴点セットのID値
         */
        public int addNftTarget(NyARNftDataSet i_dataset)
        {
            //KPMスレッドが待機中になるまで待つ
            while (this._kpm_thread.ThreadState == ThreadState.Running)
            {
                Thread.Yield();
            }
            //追加
            this._nftdatalist.Add(new NftTarget(i_dataset));
            return this._nftdatalist.Count - 1;
        }

        /**
         * NFTターゲットの変換行列を返します。
         * @para i_id
         * 特徴点セットのID値。
         * @return
         * [readonly]
         * 姿勢行列を格納したオブジェクト。座標系は、ARToolKit座標系です。
         */
        public NyARDoubleMatrix44 getTransformMatrix(int i_id)
        {
            NftTarget target = this._nftdatalist[i_id];
            return target.front_transmat;
        }
        /**
         * 特徴点セットのID値に対応したターゲットを検出しているかを返します。
         * @param i_id
         * 特徴点セットのID値
         * @return
         * 
         */
        public bool isExist(int i_id)
        {
            return this._nftdatalist[i_id].stage > NftTarget.ST_KPM_FOUND;
        }



        /**
         * Key point Matching Thread
         */
        private class KpmWorker
        {
            readonly private FreakKeypointMatching _attached_matcher;
            readonly private NyARNftTargetList _ref_nftdatalist;
            public KpmWorker(NyARParam i_ref_cparam, NyARNftTargetList i_attached_nftdatalist)
            {
                this._attached_matcher = new FreakKeypointMatching(i_ref_cparam);
                this._ref_nftdatalist = i_attached_nftdatalist;
            }
            private Thread _th=null;
            /** この関数はメインクラスのupdateから実行します。
             * 
             */
            public bool updateInputImage(INyARGrayscaleRaster i_input)
            {
                if (this._th == null)
                {
                    return false;
                }
                lock (this)
                {
                    ThreadState st = this._th.ThreadState;
                    //スレッドが無期限待機中でなければなにもしない。
                    if (st != ThreadState.WaitSleepJoin)
                    {
                        return false;
                    }
                    //計算結果のコピー
                    foreach (NftTarget target in this._ref_nftdatalist)
                    {
                        //検出フラグの更新
                        if (target.back_has_result && target.stage == NftTarget.ST_KPM_SEARCH)
                        {
                            //見つかった時だけ更新
                            //System.out.println("ST_KPM_FOUND");

                            target.stage = NftTarget.ST_KPM_FOUND;
                            target.front_transmat.setValue(target.back_transmat);
                            target.back_has_result = false;
                        }
                    }
                    //KPMスレッドの再開
                    this._attached_matcher.updateInputImage(i_input);
                    this._th.Interrupt();
                }
                return true;
            }
            public void run(object i_arg)
            {
                this._th = (Thread)i_arg;
                try
                {
                    for (; ; )
                    {
                        //開始待ち待機
                        try
                        {
                            Thread.Sleep(System.Threading.Timeout.Infinite);
                        }
                        catch (ThreadInterruptedException)
                        {
                            //Interrupted!
                        }
                        //全てのキーマップをチェック
                        this._attached_matcher.updateFeatureSet();
                        foreach (NftTarget nt in this._ref_nftdatalist)
                        {
                            //検出ステージチェック(別スレッドからの更新と衝突した場合は1フレーム無駄になる。)
                            if (nt.stage > NftTarget.ST_KPM_SEARCH)
                            {
                                continue;
                            }
                            //N番目のNFTターゲットのバックグラウンドに書き込み
                            nt.back_has_result = this._attached_matcher.kpmMatching(nt.dataset.freak_fset, nt.back_transmat);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    //Aborted!
                    return;
                }
            }
        }
        public void getScreenPos(int msid, double i_x, double i_y, double i_z,
                NyARDoublePoint2d pos)
        {
            // TODO Auto-generated method stub

        }
        /**
         * ワーカースレッドを終了します。
         * メインスレッド終了時に必ず実行してください。
         */
        public void shutdown()
        {
            this._kpm_thread.Abort();
            this._kpm_thread.Join();
        }
        public static void main(String[] args)
        {
            //String img_file="../Data/testcase/test.raw";
            //String cparam=	"../Data/testcase/camera_para5.dat";
            //String nftdataset="../Data/testcase/pinball";
            ////カメラパラメータ
            //INyARRgbRaster rgb=NyARRgbRaster.createInstance(640,480,NyARBufferType.BYTE1D_B8G8R8X8_32);
            //FileInputStream fs = new FileInputStream(img_file);
            //fs.read((byte[])rgb.getBuffer());

            //NyARParam param = NyARParam.loadFromARParamFile(new FileInputStream(cparam),640,480,NyARParam.DISTFACTOR_LT_ARTK5);
            //NyARSensor sensor=new NyARSensor(640,480);
            //NyARNftSystem ms=new NyARNftSystem(new NyARNftSystemConfig(param));
            //int id=ms.addNftTarget(NyARNftDataSet.loadFromNftFiles(nftdataset));
            //System.out.println(id);
            //sensor.update(rgb);
            //for(int i=0;i<10;i++){
            //    sensor.updateTimeStamp();
            //    ms.update(sensor);
            //    Thread.sleep(100);
            //}
            //ms.shutdown();
        }
        private NyARDoublePoint3d[] __pos3d = NyARDoublePoint3d.createArray(4);
        private NyARDoublePoint2d[] __pos2d = NyARDoublePoint2d.createArray(4);


        /**
         * この関数は、マーカ平面上の任意の４点で囲まれる領域から、画像を射影変換して返します。
         * {@link #isExistMarker(int)}がtrueの時にだけ使用できます。
         * @param i_id
         * マーカID（ハンドル）値。
         * @param i_sensor
         * 画像を取得するセンサオブジェクト。通常は{@link #update(NyARSensor)}関数に入力したものと同じものを指定します。
         * @param i_x1
         * 頂点1[mm]
         * @param i_y1
         * 頂点1[mm]
         * @param i_x2
         * 頂点2[mm]
         * @param i_y2
         * 頂点2[mm]
         * @param i_x3
         * 頂点3[mm]
         * @param i_y3
         * 頂点3[mm]
         * @param i_x4
         * 頂点4[mm]
         * @param i_y4
         * 頂点4[mm]
         * @param i_raster
         * 取得した画像を格納するオブジェクト
         * @return
         * 結果を格納したi_rasterオブジェクト
         * @throws NyARRuntimeException
         */
        public INyARRgbRaster getPlaneImage(
            int i_id,
            NyARSensor i_sensor,
            double i_x1, double i_y1,
            double i_x2, double i_y2,
            double i_x3, double i_y3,
            double i_x4, double i_y4,
            INyARRgbRaster i_raster)
        {
            NyARDoublePoint3d[] pos = this.__pos3d;
            NyARDoublePoint2d[] pos2 = this.__pos2d;
            NyARDoubleMatrix44 tmat = this.getTransformMatrix(i_id);
            tmat.transform3d(i_x1, i_y1, 0, pos[1]);
            tmat.transform3d(i_x2, i_y2, 0, pos[0]);
            tmat.transform3d(i_x3, i_y3, 0, pos[3]);
            tmat.transform3d(i_x4, i_y4, 0, pos[2]);
            for (int i = 3; i >= 0; i--)
            {
                this.getFrustum().project(pos[i], pos2[i]);
            }
            return i_sensor.getPerspectiveImage(pos2[0].x, pos2[0].y, pos2[1].x, pos2[1].y, pos2[2].x, pos2[2].y, pos2[3].x, pos2[3].y, i_raster);
        }
        /**
         * この関数は、マーカ平面上の任意の矩形で囲まれる領域から、画像を射影変換して返します。
         * {@link #isExistMarker(int)}がtrueの時にだけ使用できます。
         * @param i_id
         * マーカID（ハンドル）値。
         * @param i_sensor
         * 画像を取得するセンサオブジェクト。通常は{@link #update(NyARSensor)}関数に入力したものと同じものを指定します。
         * @param i_l
         * 矩形の左上点です。
         * @param i_t
         * 矩形の左上点です。
         * @param i_w
         * 矩形の幅です。
         * @param i_h
         * 矩形の幅です。
         * @param i_raster
         * 出力先のオブジェクト
         * @return
         * 結果を格納したi_rasterオブジェクト
         * @throws NyARRuntimeException
         */
        public INyARRgbRaster getPlaneImage(int i_id, NyARSensor i_sensor, double i_l, double i_t, double i_w, double i_h, INyARRgbRaster i_raster)
        {
            return this.getPlaneImage(i_id, i_sensor, i_l + i_w - 1, i_t + i_h - 1, i_l, i_t + i_h - 1, i_l, i_t, i_l + i_w - 1, i_t, i_raster);
        }
        /**
         * この関数は、スクリーン座標点をマーカ平面の点に変換します。
         * {@link #isExistMarker(int)}がtrueの時にだけ使用できます。
         * @param i_id
         * マーカID（ハンドル）値。
         * @param i_x
         * 変換元のスクリーン座標
         * @param i_y
         * 変換元のスクリーン座標
         * @param i_out
         * 結果を格納するオブジェクト
         * @return
         * 結果を格納したi_outに設定したオブジェクト
         */
        public NyARDoublePoint3d getPlanePos(int i_id, int i_x, int i_y, NyARDoublePoint3d i_out)
        {
            this.getFrustum().unProjectOnMatrix(i_x, i_y, this.getTransformMatrix(i_id), i_out);
            return i_out;
        }


        [System.Obsolete("use isTarget")]
        public bool isExistTarget(int i_id)
        {
            return this.isExist(i_id);
        }

        [System.Obsolete("use getPlanePos")]
        public NyARDoublePoint3d getMarkerPlanePos(int i_id, int i_x, int i_y, NyARDoublePoint3d i_out)
        {
            return this.getPlanePos(i_id, i_x, i_y, i_out);
        }


    }
}
