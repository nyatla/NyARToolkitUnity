using System;
using System.Diagnostics;
using System.IO;

namespace jp.nyatla.nyartoolkit.cs.cs4
{

    public class BinaryWriter
    {
        public const int ENDIAN_LITTLE = 1;
        public const int ENDIAN_BIG = 2;
        private readonly MemoryStream _ms;
        private int _order;
        public BinaryWriter(int i_order, int i_initial_capacity)
        {
            this._ms = new MemoryStream(i_initial_capacity);
            this.order(i_order);
        }



        public void putInt(int v)
        {
            byte[] t = BitConverter.GetBytes(v);
            if (this._order == ENDIAN_BIG)
            {
                Array.Reverse(t);
            }
            this._ms.Write(t, 0, t.Length);
        }
        public void putFloat(float v)
        {
            byte[] t = BitConverter.GetBytes(v);
            if (this._order == ENDIAN_BIG)
            {
                Array.Reverse(t);
            }
            this._ms.Write(t, 0, t.Length);
        }
        public void putIntArray(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                this.putInt(v[i]);
            }
        }
        public void putFloatArray(float[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                this.putFloat(v[i]);
            }
        }
        public void putByteArray(byte[] v)
        {
            this._ms.Write(v, 0, v.Length);
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
         * 書き込み位置を設定する。
         * @param i_pos
         */
        public void position(int i_pos)
        {
            this._ms.Position = i_pos;
        }
        /**
         * 内容をbyte配列にして返します。
         * @return
         */
        public byte[] getBinary()
        {
            return this._ms.ToArray();
        }

    }
}
