using System;
using System.IO;
using jp.nyatla.nyartoolkit.cs.cs4;
namespace jp.nyatla.nyartoolkit.cs.core
{

    /**
     * DataSetを構成するファイルパッケージのIOクラスです。
     * <pre>
     * File structure
     *	struct{
     * 	char[32] HEADERSTR
     *	int NUM_ENTRY
     *	struct [NUM_ENTRY]{
     * 		int ENTRY_TYPE
     * 		int ENTRY_SIZE
     *	}
     *	byte[] ENTRY_BYTES
     *	:
     *	}
     *</pre>
     *<pre>
     *　HS_NFTPACK_V1 parameter
     *	HEADERSTR:=	"NYARNFTPACK/1"
     *	NUM_ENTRY:= 3
     *	ENTRY[0]:=	{ET_ISET5_RAW,s1}
     *	ENTRY[1]:=	{ET_FSET,s2}
     *	ENTRY[2]:=	{ET_FREAKFSET,s3}	//ページ番号は0のみであること
     *	ENTRY_BYTES:= byte[s1+s2+s3]
     *</pre>
     */
    public class NyARNftDataSetFile
    {

        public const int ET_ISET4 = 0x0101;
        public const int ET_ISET5 = 0x0102;
        public const int ET_ISET5_RAW = 0x0103;
        public const int ET_FSET = 0x0201;
        public const int ET_FREAKFSET = 0x0301;
        public const int ET_FREAKBH = 0x0401;

        readonly public NyARNftIsetFile iset;
        readonly public NyARNftFsetFile fset;
        readonly public NyARNftFreakFsetFile fset3;
        private static String byteArray2NullTerminateStr(byte[] i_bytes)
        {

            for (int i = 0; i < i_bytes.Length; i++)
            {
                if (i_bytes[i] == 0)
                {
                    byte[] d = new byte[i];
                    System.Array.Copy(i_bytes, d, i);
                    return System.Text.Encoding.UTF8.GetString(d);
                }
            }
            return "";
        }
        public static NyARNftDataSetFile loadFromNftFilePack(Stream i_stream)
        {
            return loadFromNftFilePack(jp.nyatla.nyartoolkit.cs.cs4.BinaryReader.toArray(i_stream));
        }
        public static NyARNftDataSetFile loadFromNftFilePack(byte[] i_byte)
        {
            jp.nyatla.nyartoolkit.cs.cs4.BinaryReader br = new jp.nyatla.nyartoolkit.cs.cs4.BinaryReader(i_byte, jp.nyatla.nyartoolkit.cs.cs4.BinaryReader.ENDIAN_LITTLE);
            //タイプチェック
            String header = byteArray2NullTerminateStr(br.getByteArray(32));
            if (header.CompareTo(HS_NFTPACK_V1) != 0)
            {
                throw new NyARRuntimeException("Invalid header");
            }

            //Entryの取得
            int num_of_entry = br.getInt();
            if (num_of_entry != 3)
            {
                throw new NyARRuntimeException("Invalid entry size");
            }
            int[] entry_info = br.getIntArray(num_of_entry * 2);
            return new NyARNftDataSetFile(
                NyARNftIsetFile.loadFromIsetFile(br.getByteArray(entry_info[2 * 0 + 1]), NyARNftIsetFile.FILE_FORMAT_ARTK_V5RAW),
                NyARNftFsetFile.loadFromFsetFile(br.getByteArray(entry_info[2 * 1 + 1])),
                NyARNftFreakFsetFile.loadFromfset3File(br.getByteArray(entry_info[2 * 2 + 1])));
        }

        public NyARNftDataSetFile(NyARNftIsetFile i_iset, NyARNftFsetFile i_fset, NyARNftFreakFsetFile i_freak_fset)
        {
            this.iset = i_iset;
            this.fset = i_fset;
            this.fset3 = i_freak_fset;
            //iset,fset,fset3から初期化済インスタンスを生成
        }



        private const String HS_NFTPACK_V1 = "NYARNFTPACK/1";
        public byte[] makeBinary()
        {
            jp.nyatla.nyartoolkit.cs.cs4.BinaryWriter bw = new jp.nyatla.nyartoolkit.cs.cs4.BinaryWriter(jp.nyatla.nyartoolkit.cs.cs4.BinaryWriter.ENDIAN_LITTLE, 2 * 1024 * 1024);
            {   //HEADERSTR
                byte[] hs = new byte[32];
                byte[] s = System.Text.Encoding.UTF8.GetBytes(HS_NFTPACK_V1);
                System.Array.Copy(s,hs,s.Length);
                bw.putByteArray(hs);
            }
            byte[] iset_bin = this.iset.makeIsetBinary(NyARNftIsetFile.FILE_FORMAT_ARTK_V5RAW);
            byte[] fset_bin = this.fset.makeFsetBinary();
            byte[] fset3_bin = this.fset3.makeFset3Binary();

            //NUM_ENTRY
            bw.putInt(3);
            //ENTRY_INFO
            bw.putInt(ET_ISET5_RAW);
            bw.putInt(iset_bin.Length);
            bw.putInt(ET_FSET);
            bw.putInt(fset_bin.Length);
            bw.putInt(ET_FREAKFSET);
            bw.putInt(fset3_bin.Length);
            //BYTE_INFO
            bw.putByteArray(iset_bin);
            bw.putByteArray(fset_bin);
            bw.putByteArray(fset3_bin);
            //バイナリを返す
            return bw.getBinary();
        }
        public static void main(String[] args)
        {
//            NyARNftIsetFile f1 = NyARNftIsetFile.loadFromIsetFile(new File("../Data/nft/infinitycat.iset"));
//            NyARNftFsetFile f2 = NyARNftFsetFile.loadFromFsetFile(new File("../Data/nft/infinitycat.fset"));
//            NyARNftFreakFsetFile f3 = NyARNftFreakFsetFile.loadFromfset3File(new File("../Data/nft/infinitycat.fset3"));
//            NyARNftDataSetFile fo1 = new NyARNftDataSetFile(f1, f2, f3);
//            NyARNftDataSetFile fo2 = NyARNftDataSetFile.loadFromNftFilePack(fo1.makeBinary());
            return;
        }


    }
}
