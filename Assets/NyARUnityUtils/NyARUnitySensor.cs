using UnityEngine;
using System.Collections;
using NyARUnityUtils;
using jp.nyatla.nyartoolkit.cs.markersystem;
using jp.nyatla.nyartoolkit.cs.core;

namespace NyARUnityUtils
{
	public class NyARUnitySensor :NyARSensor
	{
	    private NyARUnityRaster _raster;	
		public NyARUnitySensor(int i_width,int i_height): base(new NyARIntSize(i_width,i_height))
		{
	        //RGBラスタの生成(Texture2Dは上下反転不要)
	        this._raster = new NyARUnityRaster(i_width,i_height,true);
	        //ラスタのセット
	        base.update(this._raster);
		}
		public void update(Texture2D i_input)
		{
			this._raster.updateByTexture2D(i_input);
		}
		public void dGetGsTex(Texture2D tx)
		{
			NyARIntSize sz=this._raster.getSize();
			int[] s=(int[])this._gs_raster.getBuffer();
			Debug.Log(s.Length);
			Color32[] c=new Color32[sz.w*sz.h];
			for(int i=0;i<sz.h;i++){
				for(int i2=0;i2<sz.w;i2++){
					c[i*sz.w+i2].r=c[i*sz.w+i2].g=c[i*sz.w+i2].b=(byte)s[i*sz.w+i2];
					c[i*sz.w+i2].a=0xff;
				}
			}
			tx.SetPixels32(c);
			tx.Apply( false );
		}		
	}

}

