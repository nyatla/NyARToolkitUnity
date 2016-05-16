/* 
 * PROJECT: NyARToolkit Professional Addon
 * --------------------------------------------------------------------------------
 * The NyARToolkit is Java edition ARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 * wm@nyatla.jp
 * http://nyatla.jp
 * 
 * This work is based on the ARToolKit4.
 * Copyright 2010-2011 ARToolworks, Inc. All rights reserved.
 *
 */
namespace jp.nyatla.nyartoolkit.cs.core
{

    public class NyARIcpPoint : NyARIcp
    {

        public NyARIcpPoint(NyARDoubleMatrix44 i_projection_matrix)
            : base(i_projection_matrix)
        {
        }
        private NyARIcpUtils.JusStack __jus = new NyARIcpUtils.JusStack(16);
        private NyARDoublePoint2d[] __du = NyARDoublePoint2d.createArray(16);
        private NyARIcpUtils.U __u = new NyARIcpUtils.U();
        private NyARDoubleMatrix44 __matXw2U = new NyARDoubleMatrix44();
        private NyARIcpUtils.DeltaS __dS = new NyARIcpUtils.DeltaS();
        public override bool icpPoint(NyARDoublePoint2d[] screenCoord,
                NyARDoublePoint3d[] worldCoord, int num,
                NyARDoubleMatrix44 initMatXw2Xc, NyARDoubleMatrix44 o_matxw2xc, NyARTransMatResultParam o_result_param)
        {
            double err0 = 0, err1;
            if (num < 4)
            {
                return false;
            }

            NyARIcpUtils.DeltaS dS = this.__dS;
            NyARIcpUtils.U u = this.__u;

            //ワークオブジェクトのリセット		
            if (this.__jus.getArraySize() < num)
            {
                this.__jus = new NyARIcpUtils.JusStack(num);
                this.__du = NyARDoublePoint2d.createArray(num);
            }
            NyARIcpUtils.JusStack jus = this.__jus;
            NyARDoublePoint2d[] du = this.__du;

            o_matxw2xc.setValue(initMatXw2Xc);

            double breakLoopErrorThresh = this.getBreakLoopErrorThresh();
            double breakLoopErrorThresh2 = this.getBreakLoopErrorThresh2();
            double breakLoopErrorRatioThresh = this.getBreakLoopErrorRatioThresh();
            double maxLoop = this.getMaxLoop();

            NyARDoubleMatrix44 matXw2U = this.__matXw2U;
            for (int i = 0; ; i++)
            {
                matXw2U.mul(this._ref_matXc2U, o_matxw2xc);
                err1 = 0.0;
                for (int j = 0; j < num; j++)
                {
                    if (!u.setXbyMatX2U(matXw2U, worldCoord[j]))
                    {
                        return false;
                    }
                    double dx = screenCoord[j].x - u.x;
                    double dy = screenCoord[j].y - u.y;
                    err1 += dx * dx + dy * dy;
                    du[j].x = dx;
                    du[j].y = dy;
                }
                err1 /= num;
                if (err1 < breakLoopErrorThresh)
                {
                    break;
                }
                if ((i > 0) && (err1 < breakLoopErrorThresh2) && (err1 / err0 > breakLoopErrorRatioThresh))
                {
                    break;
                }
                if (i == maxLoop)
                {
                    break;
                }
                err0 = err1;
                jus.clear();
                for (int j = 0; j < num; j++)
                {
	    			if(!jus.push(this._ref_matXc2U,o_matxw2xc, worldCoord[j],du[j],1.0))
                    {
                        return false;
                    }
                }
                if (!dS.setJusArray(jus))
                {
                    return false;
                }
                dS.makeMat(o_matxw2xc);
                }
            if (o_result_param != null)
            {
                o_result_param.last_error = err1;
            }
            // *err = err1;
            return true;
        }

    }
}
