#define NYARTKCS_DOTNET_FW
#if NYARTKCS_DOTNET_FW
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using jp.nyatla.nyartoolkit.cs.core;

namespace jp.nyatla.nyartoolkit.cs.cs4
{


    /**
     * bitmapと互換性のあるラスタです。
     */
    public class NyARBitmapRaster : NyARRgbRaster,IDisposable
    {
        #region APIs
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion
        private bool _is_disposed=false;
        readonly private Bitmap _buf;
        /// <summary>
        /// Bitmapを参照するインスタンスを生成する。
        /// </summary>
        /// <param name="i_img">
        /// PixelFormat.Format32bppRgb形式のビットマップである必要がある。
        /// </param>
        public NyARBitmapRaster(Bitmap i_img)
            : base(i_img.Width, i_img.Height, false)
	    {
            if (i_img.PixelFormat != PixelFormat.Format32bppRgb)
            {
                throw new NyARRuntimeException();
            }
            this._buf = i_img;
            this._is_disposed = true;
	    }
        /**
         * インスタンスを生成します。インスタンスは、PixelFormat.Format32bppRgb形式のビットマップをバッファに持ちます。
         */
        public NyARBitmapRaster(int i_width, int i_heigth)
            : base(i_width, i_heigth,true)
        {
            this._buf = new Bitmap(i_width, i_heigth, PixelFormat.Format32bppRgb);
        }


        ///**
        // * Readerとbufferを初期化する関数です。コンストラクタから呼び出します。
        // * 継承クラスでこの関数を拡張することで、対応するバッファタイプの種類を増やせます。
        // * @param i_size
        // * ラスタのサイズ
        // * @param i_raster_type
        // * バッファタイプ
        // * @param i_is_alloc
        // * 外部参照/内部バッファのフラグ
        // * @return
        // * 初期化が成功すると、trueです。
        // * @ 
        // */
        //protected override void initInstance(NyARIntSize i_size, int i_raster_type, bool i_is_alloc)
        //{
        //    //バッファの構築
        //    switch (i_raster_type)
        //    {
        //        case NyARBufferType.OBJECT_CS_Bitmap:
        //            this._rgb_pixel_driver = new NyARRgbPixelDriver_CsBitmap();
        //            if (i_is_alloc)
        //            {
        //                this._buf = new Bitmap(i_size.w, i_size.h, PixelFormat.Format32bppRgb);
        //                this._rgb_pixel_driver.switchRaster(this);
        //            }
        //            else
        //            {
        //                this._buf = null;
        //            }
        //            this._is_attached_buffer = i_is_alloc;
        //            break;
        //        default:
        //            base.initInstance(i_size,i_raster_type,i_is_alloc);
        //            break;
        //    }
        //    //readerの構築
        //    return;
        //}
        public void Dispose()
        {
            //ロックを解除
            while (this.number_of_lock > 0)
            {
                this.unlockBitmap();
            }
            //Unmanaged Resourceの開放
            if (!this._is_disposed)
            {
                ((Bitmap)this._buf).Dispose();
            }
            //Disposed
            this._is_disposed = true;
            //GCからデストラクタを除外
            GC.SuppressFinalize(this);
        }
        ~NyARBitmapRaster()
        {
            this.Dispose();
        }
        /**
         * この関数は、ラスタに外部参照バッファをセットします。
         * 外部参照バッファの時にだけ使えます。
         */
        public override void wrapBuffer(object i_ref_buf)
        {
            throw new NyARRuntimeException();
        }

        public override object createInterface(Type iIid)
        {
            if (iIid == typeof(INyARPerspectiveCopy))
            {
                return new PerspectiveCopy_CSBitmap(this);
            }
            if (iIid == typeof(NyARMatchPattDeviationColorData.IRasterDriver))
            {
                return NyARMatchPattDeviationColorData.RasterDriverFactory.createDriver(this);
            }
            if (iIid == typeof(INyARRgb2GsFilter))
            {
                return new NyARRgb2GsFilterRgbAve_CsBitmap(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterRgbAve))
            {
                return new NyARRgb2GsFilterRgbAve_CsBitmap(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterRgbCube))
            {
                return NyARRgb2GsFilterFactory.createRgbCubeDriver(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterYCbCr))
            {
                return NyARRgb2GsFilterFactory.createYCbCrDriver(this);
            }
            if (iIid == typeof(INyARRgb2GsFilterArtkTh))
            {
                return new NyARRgb2GsFilterArtkTh_CsBitmap(this);
            }
            throw new NyARRuntimeException();
        }
        /**
          * この関数は、指定した座標の1ピクセル分のRGBデータを、配列に格納して返します。
          */
        override public int[] getPixel(int i_x, int i_y, int[] o_rgb)
        {
            BitmapData bm = this.lockBitmap();
            //byte(BGRX)=int(XRGB)
            int pix = Marshal.ReadInt32(bm.Scan0, i_x * 4 + i_y * bm.Stride);
            o_rgb[0] = (pix >> 16) & 0xff;// R
            o_rgb[1] = (pix >> 8) & 0xff; // G
            o_rgb[2] = (pix) & 0xff;    // B
            this.unlockBitmap();
            return o_rgb;
        }

        /**
         * この関数は、座標群から、ピクセルごとのRGBデータを、配列に格納して返します。
         */
        override public int[] getPixelSet(int[] i_x, int[] i_y, int i_num, int[] o_rgb)
        {
            BitmapData bm = this.lockBitmap();
            for (int i = i_num - 1; i >= 0; i--)
            {
                int pix = Marshal.ReadInt32(bm.Scan0, i_x[i] * 4 + i_y[i] * bm.Stride);
                o_rgb[i * 3 + 0] = (pix >> 16) & 0xff;// R
                o_rgb[i * 3 + 1] = (pix >> 8) & 0xff; // G
                o_rgb[i * 3 + 2] = (pix) & 0xff;      // B
            }
            this.unlockBitmap();
            return o_rgb;
        }

        /**
         * この関数は、RGBデータを指定した座標のピクセルにセットします。
         */
        override public void setPixel(int i_x, int i_y, int[] i_rgb)
        {
            BitmapData bm = this.lockBitmap();
            int pix = (0x00ff0000 & (i_rgb[0] << 16)) | (0x0000ff00 & (i_rgb[1] << 8)) | (0x0000ff & (i_rgb[2]));
            Marshal.WriteInt32(bm.Scan0, i_x * 4 + i_y * bm.Stride, pix);
            this.unlockBitmap();
            return;
        }

        /**
         * この関数は、RGBデータを指定した座標のピクセルにセットします。
         */
        override public void setPixel(int i_x, int i_y, int i_r, int i_g, int i_b)
        {
            BitmapData bm = this.lockBitmap();
            int pix = (0x00ff0000 & (i_r << 16)) | (0x0000ff00 & (i_g << 8)) | (0x0000ff & (i_b));
            Marshal.WriteInt32(bm.Scan0, i_x * 4 + i_y * bm.Stride, pix);
            this.unlockBitmap();
            return;
        }

        /**
         * この関数は、機能しません。
         */
        override public void setPixels(int[] i_x, int[] i_y, int i_num, int[] i_intrgb)
        {
            throw new NyARRuntimeException();
        }
        #region extension functions
        private int number_of_lock=0;
        private BitmapData _bm_cache;
        public BitmapData lockBitmap()
        {

            if(this.number_of_lock==0){
                Bitmap bm=(Bitmap)this._buf;
                this.number_of_lock++;
                this._bm_cache = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),ImageLockMode.ReadWrite,PixelFormat.Format32bppRgb);
            }else{
                this.number_of_lock++;
            }
            return this._bm_cache;
        }
        public void unlockBitmap()
        {

            if (this.number_of_lock>1)
            {
                this.number_of_lock--;
            }
            else if (this.number_of_lock == 0)
            {
                throw new NyARRuntimeException();
            }
            else
            {
                this.number_of_lock--;
                Bitmap bm = (Bitmap)this._buf;
                bm.UnlockBits(this._bm_cache);
            }
        }
        public Bitmap getBitmap()
        {
            Debug.Assert(!this._is_disposed);

            return (Bitmap)this._buf;
        }
        override public Object getBuffer()
        {
            return this._buf;
        }
        override public int getBufferType()
        {
            return NyARBufferType.OBJECT_CS_Bitmap;
        }

        #endregion
    }

    #region pixel drivers
    class NyARRgb2GsFilterRgbAve_CsBitmap : INyARRgb2GsFilterRgbAve
    {
        private NyARBitmapRaster _ref_raster;
        public NyARRgb2GsFilterRgbAve_CsBitmap(NyARBitmapRaster i_ref_raster)
        {
            Debug.Assert(i_ref_raster.isEqualBufferType(NyARBufferType.OBJECT_CS_Bitmap));
            this._ref_raster = i_ref_raster;
        }
        public void convert(INyARGrayscaleRaster i_raster)
        {
            NyARIntSize s = this._ref_raster.getSize();
            this.convertRect(0, 0, s.w, s.h, i_raster);
        }
        private byte[] _work = new byte[4 * 8];
        public void convertRect(int l, int t, int w, int h, INyARGrayscaleRaster o_raster)
        {
            byte[] work = this._work;
            BitmapData bm = this._ref_raster.lockBitmap();
            NyARIntSize size = this._ref_raster.getSize();
            int bp = (l + t * size.w) * 4 + (int)bm.Scan0;
            int b = t + h;
            int row_padding_dst = (size.w - w);
            int row_padding_src = row_padding_dst * 4;
            int pix_count = w;
            int pix_mod_part = pix_count - (pix_count % 8);
            int dst_ptr = t * size.w + l;
            // in_buf = (byte[])this._ref_raster.getBuffer();
            switch (o_raster.getBufferType())
            {
                case NyARBufferType.INT1D_GRAY_8:
                    int[] out_buf = (int[])o_raster.getBuffer();
                    for (int y = t; y < b; y++)
                    {

                        int x = 0;
                        for (x = pix_count - 1; x >= pix_mod_part; x--)
                        {
                            int p = Marshal.ReadInt32((IntPtr)bp);
                            out_buf[dst_ptr++] = (((p >> 16) & 0xff) + ((p >> 8) & 0xff) + (p & 0xff)) / 3;
                            bp += 4;
                        }
                        for (; x >= 0; x -= 8)
                        {
                            Marshal.Copy((IntPtr)bp, work, 0, 32);
                            out_buf[dst_ptr++] = (work[0] + work[1] + work[2]) / 3;
                            out_buf[dst_ptr++] = (work[4] + work[5] + work[6]) / 3;
                            out_buf[dst_ptr++] = (work[8] + work[9] + work[10]) / 3;
                            out_buf[dst_ptr++] = (work[12] + work[13] + work[14]) / 3;
                            out_buf[dst_ptr++] = (work[16] + work[17] + work[18]) / 3;
                            out_buf[dst_ptr++] = (work[20] + work[21] + work[22]) / 3;
                            out_buf[dst_ptr++] = (work[24] + work[25] + work[26]) / 3;
                            out_buf[dst_ptr++] = (work[28] + work[29] + work[30]) / 3;
                            bp += 32;
                        }
                        bp += row_padding_src;
                        dst_ptr += row_padding_dst;
                    }
                    this._ref_raster.unlockBitmap();
                    return;
                default:
                    for (int y = t; y < b; y++)
                    {
                        for (int x = 0; x < pix_count; x++)
                        {
                            int p = Marshal.ReadInt32((IntPtr)bp);
                            o_raster.setPixel(x, y, (((p >> 16) & 0xff) + ((p >> 8) & 0xff) + (p & 0xff)) / 3);
                            bp += 4;
                        }
                        bp += row_padding_src;
                    }
                    this._ref_raster.unlockBitmap();
                    return;
            }

        }
    }


    sealed class NyARRgb2GsFilterArtkTh_CsBitmap : INyARRgb2GsFilterArtkTh
    {
        private NyARBitmapRaster _raster;
        public void doFilter(int i_h, INyARGrayscaleRaster i_gsraster)
        {
            NyARIntSize s = this._raster.getSize();
            this.doFilter(0, 0, s.w, s.h, i_h, i_gsraster);
        }

        public NyARRgb2GsFilterArtkTh_CsBitmap(NyARBitmapRaster i_raster)
        {
            Debug.Assert(i_raster.isEqualBufferType(NyARBufferType.OBJECT_CS_Bitmap));
            this._raster = i_raster;
        }
        private byte[] _work=new byte[4*8];
        public void doFilter(int i_l, int i_t, int i_w, int i_h, int i_th, INyARGrayscaleRaster i_gsraster)
        {
            Debug.Assert(i_gsraster.isEqualBufferType(NyARBufferType.INT1D_BIN_8));
            BitmapData bm = this._raster.lockBitmap();
            byte[] work = this._work;
            int[] output = (int[])i_gsraster.getBuffer();
            NyARIntSize s = this._raster.getSize();
            int th = i_th * 3;
            int skip_dst = (s.w - i_w);
            int skip_src = skip_dst * 4;
            int pix_count = i_w;
            int pix_mod_part = pix_count - (pix_count % 8);
            //左上から1行づつ走査していく
            long pt_dst = (i_t * s.w + i_l);
            long pt_src = pt_dst * 4 + (long)bm.Scan0;
            for (int y = i_h - 1; y >= 0; y -= 1)
            {
                int x;
                int p;
                for (x = pix_count - 1; x >= pix_mod_part; x--)
                {
                    p = Marshal.ReadInt32((IntPtr)pt_src);
                    output[pt_dst++] = (((p >> 16) & 0xff) + ((p >> 8) & 0xff) + (p & 0xff)) <= th ? 0 : 1;
                    pt_src += 4;
                }
                for (; x >= 0; x -= 8)
                {
                    Marshal.Copy((IntPtr)pt_src, work, 0, 32);
                    output[pt_dst  ] = (work[0]+work[1]+work[2]) <= th ? 0 : 1;
                    output[pt_dst+1] = (work[4] + work[5] + work[6]) <= th ? 0 : 1;
                    output[pt_dst+2] = (work[8] + work[9] + work[10]) <= th ? 0 : 1;
                    output[pt_dst+3] = (work[12] + work[13] + work[14]) <= th ? 0 : 1;
                    output[pt_dst+4] = (work[16] + work[17] + work[18]) <= th ? 0 : 1;
                    output[pt_dst+5] = (work[20] + work[21] + work[22]) <= th ? 0 : 1;
                    output[pt_dst+6] = (work[24] + work[25] + work[26]) <= th ? 0 : 1;
                    output[pt_dst+7] = (work[28] + work[29] + work[30]) <= th ? 0 : 1;
                    pt_src += 32;
                    pt_dst += 8;
                }
                //スキップ
                pt_src += skip_src;
                pt_dst += skip_dst;
            }
            this._raster.unlockBitmap();
            return;
        }

    }



    sealed class PerspectiveCopy_CSBitmap : NyARPerspectiveCopy_Base
    {
        private NyARBitmapRaster _ref_raster;
        public PerspectiveCopy_CSBitmap(NyARBitmapRaster i_ref_raster)
        {
            Debug.Assert(i_ref_raster.isEqualBufferType(NyARBufferType.OBJECT_CS_Bitmap));
            this._ref_raster = i_ref_raster;
        }
        protected override bool onePixel(int pk_l, int pk_t, double[] cpara, INyARRaster o_out)
        {
            BitmapData in_bmp = this._ref_raster.lockBitmap();
            int in_w = this._ref_raster.getWidth();
            int in_h = this._ref_raster.getHeight();

             //ピクセルリーダーを取得
            double cp0 = cpara[0];
            double cp3 = cpara[3];
            double cp6 = cpara[6];
            double cp1 = cpara[1];
            double cp4 = cpara[4];
            double cp7 = cpara[7];

            int out_w = o_out.getWidth();
            int out_h = o_out.getHeight();
            double cp7_cy_1 = cp7 * pk_t + 1.0 + cp6 * pk_l;
            double cp1_cy_cp2 = cp1 * pk_t + cpara[2] + cp0 * pk_l;
            double cp4_cy_cp5 = cp4 * pk_t + cpara[5] + cp3 * pk_l;
            int r, g, b, p;
            switch (o_out.getBufferType())
            {
                case NyARBufferType.INT1D_X8R8G8B8_32:
                    int[] pat_data = (int[])o_out.getBuffer();
                    p = 0;
                    for (int iy = 0; iy < out_h; iy++)
                    {
                        //解像度分の点を取る。
                        double cp7_cy_1_cp6_cx = cp7_cy_1;
                        double cp1_cy_cp2_cp0_cx = cp1_cy_cp2;
                        double cp4_cy_cp5_cp3_cx = cp4_cy_cp5;

                        for (int ix = 0; ix < out_w; ix++)
                        {
                            //1ピクセルを作成
                            double d = 1 / (cp7_cy_1_cp6_cx);
                            int x = (int)((cp1_cy_cp2_cp0_cx) * d);
                            int y = (int)((cp4_cy_cp5_cp3_cx) * d);
                            if (x < 0) { x = 0; } else if (x >= in_w) { x = in_w - 1; }
                            if (y < 0) { y = 0; } else if (y >= in_h) { y = in_h - 1; }

                            //
                            pat_data[p] = Marshal.ReadInt32(in_bmp.Scan0, (x * 4 + y * in_bmp.Stride));
                            //r = (px >> 16) & 0xff;// R
                            //g = (px >> 8) & 0xff; // G
                            //b = (px) & 0xff;    // B
                            cp7_cy_1_cp6_cx += cp6;
                            cp1_cy_cp2_cp0_cx += cp0;
                            cp4_cy_cp5_cp3_cx += cp3;
                            //pat_data[p] = (r << 16) | (g << 8) | ((b & 0xff));
                            //pat_data[p] = px;

                            p++;
                        }
                        cp7_cy_1 += cp7;
                        cp1_cy_cp2 += cp1;
                        cp4_cy_cp5 += cp4;
                    }
                    this._ref_raster.unlockBitmap();
                    return true;
                default:
                    if (o_out is NyARBitmapRaster)
                    {
                        NyARBitmapRaster bmr = (NyARBitmapRaster)o_out;
                        BitmapData bm = bmr.lockBitmap();
                        p = 0;
                        for (int iy = 0; iy < out_h; iy++)
                        {
                            //解像度分の点を取る。
                            double cp7_cy_1_cp6_cx = cp7_cy_1;
                            double cp1_cy_cp2_cp0_cx = cp1_cy_cp2;
                            double cp4_cy_cp5_cp3_cx = cp4_cy_cp5;

                            for (int ix = 0; ix < out_w; ix++)
                            {
                                //1ピクセルを作成
                                double d = 1 / (cp7_cy_1_cp6_cx);
                                int x = (int)((cp1_cy_cp2_cp0_cx) * d);
                                int y = (int)((cp4_cy_cp5_cp3_cx) * d);
                                if (x < 0) { x = 0; } else if (x >= in_w) { x = in_w - 1; }
                                if (y < 0) { y = 0; } else if (y >= in_h) { y = in_h - 1; }
                                int pix = Marshal.ReadInt32(in_bmp.Scan0, (x * 4 + y * in_bmp.Stride));
                                Marshal.WriteInt32(bm.Scan0, ix * 4 + iy * bm.Stride, pix);
                                cp7_cy_1_cp6_cx += cp6;
                                cp1_cy_cp2_cp0_cx += cp0;
                                cp4_cy_cp5_cp3_cx += cp3;
                                p++;
                            }
                            cp7_cy_1 += cp7;
                            cp1_cy_cp2 += cp1;
                            cp4_cy_cp5 += cp4;
                        }
                        bmr.unlockBitmap();
                        this._ref_raster.unlockBitmap();
                        return true;
                    }
                    else if (o_out is INyARRgbRaster)                    
                    {
                        //ANY to RGBx
                        INyARRgbRaster out_raster = ((INyARRgbRaster)o_out);
                        for (int iy = 0; iy < out_h; iy++)
                        {
                            //解像度分の点を取る。
                            double cp7_cy_1_cp6_cx = cp7_cy_1;
                            double cp1_cy_cp2_cp0_cx = cp1_cy_cp2;
                            double cp4_cy_cp5_cp3_cx = cp4_cy_cp5;

                            for (int ix = 0; ix < out_w; ix++)
                            {
                                //1ピクセルを作成
                                double d = 1 / (cp7_cy_1_cp6_cx);
                                int x = (int)((cp1_cy_cp2_cp0_cx) * d);
                                int y = (int)((cp4_cy_cp5_cp3_cx) * d);
                                if (x < 0) { x = 0; } else if (x >= in_w) { x = in_w - 1; }
                                if (y < 0) { y = 0; } else if (y >= in_h) { y = in_h - 1; }

                                int px = Marshal.ReadInt32(in_bmp.Scan0, (x*4 + y * in_bmp.Stride));
                                r = (px >> 16) & 0xff;// R
                                g = (px >> 8) & 0xff; // G
                                b = (px) & 0xff;    // B
                                cp7_cy_1_cp6_cx += cp6;
                                cp1_cy_cp2_cp0_cx += cp0;
                                cp4_cy_cp5_cp3_cx += cp3;
                                out_raster.setPixel(ix, iy, r, g, b);
                            }
                            cp7_cy_1 += cp7;
                            cp1_cy_cp2 += cp1;
                            cp4_cy_cp5 += cp4;
                        }
                        this._ref_raster.unlockBitmap();
                        return true;
                    }
                    break;
            }
            this._ref_raster.unlockBitmap();
            return false;
        }
        protected override bool multiPixel(int pk_l, int pk_t, double[] cpara, int i_resolution, INyARRaster o_out)
        {
            BitmapData in_bmp = this._ref_raster.lockBitmap();
            int in_w = this._ref_raster.getWidth();
            int in_h = this._ref_raster.getHeight();
            int res_pix = i_resolution * i_resolution;

            //ピクセルリーダーを取得
            double cp0 = cpara[0];
            double cp3 = cpara[3];
            double cp6 = cpara[6];
            double cp1 = cpara[1];
            double cp4 = cpara[4];
            double cp7 = cpara[7];
            double cp2 = cpara[2];
            double cp5 = cpara[5];

            int out_w = o_out.getWidth();
            int out_h = o_out.getHeight();
            if (o_out is NyARBitmapRaster)
            {
                NyARBitmapRaster bmr=((NyARBitmapRaster)o_out);
                BitmapData bm=bmr.lockBitmap();
                for (int iy = out_h - 1; iy >= 0; iy--)
                {
                    //解像度分の点を取る。
                    for (int ix = out_w - 1; ix >= 0; ix--)
                    {
                        int r, g, b;
                        r = g = b = 0;
                        int cy = pk_t + iy * i_resolution;
                        int cx = pk_l + ix * i_resolution;
                        double cp7_cy_1_cp6_cx_b = cp7 * cy + 1.0 + cp6 * cx;
                        double cp1_cy_cp2_cp0_cx_b = cp1 * cy + cp2 + cp0 * cx;
                        double cp4_cy_cp5_cp3_cx_b = cp4 * cy + cp5 + cp3 * cx;
                        for (int i2y = i_resolution - 1; i2y >= 0; i2y--)
                        {
                            double cp7_cy_1_cp6_cx = cp7_cy_1_cp6_cx_b;
                            double cp1_cy_cp2_cp0_cx = cp1_cy_cp2_cp0_cx_b;
                            double cp4_cy_cp5_cp3_cx = cp4_cy_cp5_cp3_cx_b;
                            for (int i2x = i_resolution - 1; i2x >= 0; i2x--)
                            {
                                //1ピクセルを作成
                                double d = 1 / (cp7_cy_1_cp6_cx);
                                int x = (int)((cp1_cy_cp2_cp0_cx) * d);
                                int y = (int)((cp4_cy_cp5_cp3_cx) * d);
                                if (x < 0) { x = 0; } else if (x >= in_w) { x = in_w - 1; }
                                if (y < 0) { y = 0; } else if (y >= in_h) { y = in_h - 1; }

                                int px = Marshal.ReadInt32(in_bmp.Scan0, (x * 4 + y * in_bmp.Stride));
                                r += (px >> 16) & 0xff;// R
                                g += (px >> 8) & 0xff; // G
                                b += (px) & 0xff;    // B
                                cp7_cy_1_cp6_cx += cp6;
                                cp1_cy_cp2_cp0_cx += cp0;
                                cp4_cy_cp5_cp3_cx += cp3;
                            }
                            cp7_cy_1_cp6_cx_b += cp7;
                            cp1_cy_cp2_cp0_cx_b += cp1;
                            cp4_cy_cp5_cp3_cx_b += cp4;
                        }
                        Marshal.WriteInt32(bm.Scan0, ix * 4 + iy * bm.Stride,
                            (0x00ff0000 & ((r / res_pix) << 16)) | (0x0000ff00 & ((g / res_pix) << 8)) | (0x0000ff & (b / res_pix)));
                    }
                }
                bmr.unlockBitmap();
                this._ref_raster.unlockBitmap();
                return true;
            }
            else if (o_out is INyARRgbRaster)
            {
                INyARRgbRaster out_raster = ((INyARRgbRaster)o_out);
                for (int iy = out_h - 1; iy >= 0; iy--)
                {
                    //解像度分の点を取る。
                    for (int ix = out_w - 1; ix >= 0; ix--)
                    {
                        int r, g, b;
                        r = g = b = 0;
                        int cy = pk_t + iy * i_resolution;
                        int cx = pk_l + ix * i_resolution;
                        double cp7_cy_1_cp6_cx_b = cp7 * cy + 1.0 + cp6 * cx;
                        double cp1_cy_cp2_cp0_cx_b = cp1 * cy + cp2 + cp0 * cx;
                        double cp4_cy_cp5_cp3_cx_b = cp4 * cy + cp5 + cp3 * cx;
                        for (int i2y = i_resolution - 1; i2y >= 0; i2y--)
                        {
                            double cp7_cy_1_cp6_cx = cp7_cy_1_cp6_cx_b;
                            double cp1_cy_cp2_cp0_cx = cp1_cy_cp2_cp0_cx_b;
                            double cp4_cy_cp5_cp3_cx = cp4_cy_cp5_cp3_cx_b;
                            for (int i2x = i_resolution - 1; i2x >= 0; i2x--)
                            {
                                //1ピクセルを作成
                                double d = 1 / (cp7_cy_1_cp6_cx);
                                int x = (int)((cp1_cy_cp2_cp0_cx) * d);
                                int y = (int)((cp4_cy_cp5_cp3_cx) * d);
                                if (x < 0) { x = 0; } else if (x >= in_w) { x = in_w - 1; }
                                if (y < 0) { y = 0; } else if (y >= in_h) { y = in_h - 1; }

                                int px = Marshal.ReadInt32(in_bmp.Scan0, (x * 4 + y * in_bmp.Stride));
                                r += (px >> 16) & 0xff;// R
                                g += (px >> 8) & 0xff; // G
                                b += (px) & 0xff;    // B
                                cp7_cy_1_cp6_cx += cp6;
                                cp1_cy_cp2_cp0_cx += cp0;
                                cp4_cy_cp5_cp3_cx += cp3;
                            }
                            cp7_cy_1_cp6_cx_b += cp7;
                            cp1_cy_cp2_cp0_cx_b += cp1;
                            cp4_cy_cp5_cp3_cx_b += cp4;
                        }
                        out_raster.setPixel(ix, iy, r / res_pix, g / res_pix, b / res_pix);
                    }
                }
                this._ref_raster.unlockBitmap();
                return true;
            }
            else
            {
                throw new NyARRuntimeException();
            }
        }
    }
    #endregion
}
#endif
