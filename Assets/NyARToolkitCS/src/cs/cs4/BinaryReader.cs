using System.IO;
using System.Diagnostics;
using System;
namespace jp.nyatla.nyartoolkit.cs.cs4
{

    /**
     * バイナリデータの読出しクラス
     */
    public class BinaryReader
    {
        public const int ENDIAN_LITTLE = 1;
        public const int ENDIAN_BIG = 2;

        readonly private byte[] _data;
        private int _pos;
        private int _order;
        public static byte[] toArray(Stream i_stream)
        {
            using (System.IO.BinaryReader br = new System.IO.BinaryReader(i_stream))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] tmp = new byte[1024];
                    int p = 0;
                    int r;
                    do
                    {
                        r = br.Read(tmp, p, tmp.Length);
                        ms.Write(tmp, 0, r);
                    } while (r == tmp.Length);
                    ms.Flush();
                    return ms.ToArray();
                }
            }
        }
        public BinaryReader(Stream i_stream, int i_order)
            : this(toArray(i_stream), i_order)
        {
        }
        public BinaryReader(byte[] i_data, int i_order)
        {
            this._pos = 0;
            this._data = i_data;
            this.order(i_order);
        }
        /**
         * マルチバイト読み込み時のエンディアン.{@link #ENDIAN_BIG}か{@link #ENDIAN_LITTLE}を設定してください。
         * @param i_order
         */
        public void order(int i_order)
        {
            this._order = i_order;
        }
        /**
         * 読出し位置を設定する。
         * @param i_pos
         */
        public void position(int i_pos)
        {
            Debug.Assert(this._pos < this._data.Length);
            this._pos = i_pos;
        }
        /**
         * 読出し可能なサイズを返す。
         * @return
         */
        public int size()
        {
            return this._data.Length;
        }
        public int getInt()
        {
            Debug.Assert(this._pos < this._data.Length);
            int ret = BitConverter.ToInt32(this._data, this._pos);
            this._pos += 4;
            if (this._order == ENDIAN_LITTLE)
            {
                return ret;
            }
            //big endian
            byte[] ba = BitConverter.GetBytes(ret);
            Array.Reverse(ba);
            return BitConverter.ToInt32(ba, 0);
        }
        public int[] getIntArray(int[] it)
        {
            for (int i = 0; i < it.Length; i++)
            {
                it[i] = this.getInt();
            }
            return it;
        }
        public int[] getIntArray(int i_size)
        {
            return this.getIntArray(new int[i_size]);
        }
        public byte getByte()
        {
            Debug.Assert(this._pos < this._data.Length);
            byte ret = this._data[this._pos];
            this._pos += 1;
            return ret;
        }
        public byte[] getByteArray(byte[] buf)
        {
            Debug.Assert(this._pos < this._data.Length);
            Array.Copy(this._data,this._pos,buf,0,buf.Length);
            this._pos += buf.Length;
            return buf;
        }
        public byte[] getByteArray(int i_size)
        {
            return getByteArray(new byte[i_size]);
        }
        public float getFloat()
        {
            Debug.Assert(this._pos < this._data.Length);
            float ret = BitConverter.ToSingle(this._data, this._pos);
            this._pos += 4;
            if (this._order == ENDIAN_LITTLE)
            {
                return ret;
            }
            //big endian
            byte[] ba = BitConverter.GetBytes(ret);
            Array.Reverse(ba);
            return BitConverter.ToSingle(ba, 0);

        }
        public float[] getFloatArray(float[] ft)
        {
            for (int i = 0; i < ft.Length; i++)
            {
                ft[i] = this.getFloat();
            }
            return ft;
        }
        public float[] getFloatArray(int i_size)
        {
            return this.getFloatArray(new float[i_size]);
        }
        public double getDouble()
        {
            Debug.Assert(this._pos < this._data.Length);
            double ret = BitConverter.ToDouble(this._data, this._pos);
            this._pos += 8;
            if (this._order == ENDIAN_LITTLE)
            {
                return ret;
            }
            //big endian
            byte[] ba = BitConverter.GetBytes(ret);
            Array.Reverse(ba);
            return BitConverter.ToDouble(ba, 0);
        }
        /**
         * bにi_length個のdouble値を読み出します。
         * 読みだした値はbの先頭から格納されます。
         * @param b
         * @param i_lengrh
         * @return
         */
        public double[] getDoubleArray(double[] b, int i_length)
        {
            for (int i = 0; i < i_length; i++)
            {
                b[i] = this.getDouble();
            }
            return b;
        }
        /**
         * bにbの長さだけdouble値を読み出します。
         * @param b
         * @return
         */
        public double[] getDoubleArray(double[] b)
        {
            return this.getDoubleArray(b, b.Length);
        }
        /**
         * 新たにi_lengthサイズの配列を生成して、要素数と同じ個数のdouble値を読み出します。
         * @param i_length
         * @return
         */
        public double[] getDoubleArray(int i_length)
        {
            return this.getDoubleArray(new double[i_length]);
        }
    }
}
