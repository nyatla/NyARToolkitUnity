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
using System.Collections.Generic;
using System.Text;

namespace jp.nyatla.nyartoolkit.cs.cs4
{
    public class ArrayUtils
    {
	    public static int[] toIntArray_impl(List<int> i_list,int i_offset,int i_size,int[] i_dest){
		    for(int i=0;i<i_size;i++){
			    i_dest[i]=i_list[i_offset+i];
		    }
		    return i_dest;
	    }
	    public static int[] toIntArray_impl(List<int> i_list,int i_offset,int i_size){
		    return toIntArray_impl(i_list,i_offset,i_size,new int[i_size]);
	    }
	    public static int[] toIntArray_impl(byte[] i_byte)
	    {
		    int[] a=new int[i_byte.Length];
		    for(int i=0;i<a.Length;i++){
			    a[i]=i_byte[i] &0xff;
		    }
		    return a;
	    }
	    public static byte[] toByteArray_impl(int[] i_int)
	    {
		    byte[] a=new byte[i_int.Length];
		    for(int i=0;i<a.Length;i++){
			    a[i]=(byte)(i_int[i] &0xff);
		    }
		    return a;
	    }


        public static double[][] newDouble2dArray(int i_r, int i_c)
        {
            double[][] d = new double[i_r][];
            for (int i = 0; i < i_r; i++)
            {
                d[i] = new double[i_c];
            }
            return d;
        }
        public static int[][] newInt2dArray(int i_r, int i_c)
        {
            int[][] d = new int[i_r][];
            for (int i = 0; i < i_r; i++)
            {
                d[i] = new int[i_c];
            }
            return d;
        }
        public static long[][] newLong2dArray(int i_r, int i_c)
        {
            long[][] d = new long[i_r][];
            for (int i = 0; i < i_r; i++)
            {
                d[i] = new long[i_c];
            }
            return d;
        }
    }
}
