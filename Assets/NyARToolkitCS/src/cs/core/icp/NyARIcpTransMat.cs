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

    /**
     * This class provides ARToolkitV4 style fitting algorism.
     * 
     */
    public class NyARIcpTransMat : INyARTransMat
    {
        /** This value indicates POINT algorism. */
        public const int AL_POINT = 0;
        /** This value indicates POINT_ROBUST algorism. */
        public const int AL_POINT_ROBUST = 1;

        private NyARIcpPoint _icpp;
        private NyARIcpPlane _icpc;
        /**
         * The constructor.
         * @param i_param
         * ARToolkit parameter object that finished setup.
         * @param i_al_mode
         * fitting algorism type.
         * @throws NyARException
         */
        public NyARIcpTransMat(NyARParam i_param, int i_al_mode)
        {
            this._icpc = new NyARIcpPlane(i_param.getPerspectiveProjectionMatrix());
            switch (i_al_mode)
            {
                case AL_POINT:
                    this._icpp = new NyARIcpPoint(i_param.getPerspectiveProjectionMatrix());
                    break;
                case AL_POINT_ROBUST:
                    this._icpp = new NyARIcpPointRobust(i_param.getPerspectiveProjectionMatrix());
                    break;
                default:
                    throw new System.ArgumentException();
            }
            return;
        }
        /**
         * Make tansform matrix by ICP algorism.
         */
	    public bool transMat(NyARSquare i_square, NyARRectOffset i_offset,NyARDoubleMatrix44 i_result,NyARTransMatResultParam o_param)
	    {
		    if(this._icpc.icpGetInitXw2Xc_from_PlanarData(i_square.sqvertex,i_offset.vertex,4,i_result)){
			    if(this._icpp.icpPoint(i_square.sqvertex,i_offset.vertex,4,i_result,i_result,o_param)){
				    return true;
			    }
		    }
		    return false;
	    }
	    /**
	     * Make tansform matrix by ICP algorism.
	     * i_prev_result parameter is not effective. should set 0.
	     */
	    public bool transMatContinue(NyARSquare i_square, NyARRectOffset i_offset, NyARDoubleMatrix44 i_prev_result, double i_prev_err,
			    NyARDoubleMatrix44 o_result, NyARTransMatResultParam o_param)
	    {
		    if(this._icpp.icpPoint(i_square.sqvertex,i_offset.vertex,4,o_result,o_result,o_param)){
			    return true;
		    }
		    return false;
	    }
    }

}
