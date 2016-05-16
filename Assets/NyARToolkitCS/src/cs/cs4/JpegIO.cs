#define NYARTKCS_DOTNET_FW
#if NYARTKCS_DOTNET_FW
using System.IO;
using System;
using System.Drawing;
using jp.nyatla.nyartoolkit.cs.core;
namespace jp.nyatla.nyartoolkit.cs.cs4
{

    /**
     * プラットフォーム依存のJpegデータIOを実装します。
     *
     */
    public class JpegIO
    {
        public class DecodeResult
        {
            readonly public int x_density;
            readonly public int y_density;
            readonly public int density_unit;
            readonly public byte[] img;
            readonly public int width;
            readonly public int height;
            public DecodeResult(int i_xd, int i_yd, byte[] i_img, int w, int h, int i_unit)
            {
                this.height = h;
                this.width = w;
                this.img = i_img;
                this.x_density = i_xd;
                this.y_density = i_yd;
                this.density_unit = i_unit;
            }
        }
        public static JpegIO.DecodeResult decode(byte[] i_src)
        {
            MemoryStream ms = new MemoryStream(i_src);
            Image im = System.Drawing.Image.FromStream(ms);
            int xd = (int)im.HorizontalResolution;
            int yd = (int)im.VerticalResolution;
            int unit = 1;//インチ
            byte[] buf=new byte[im.Width*im.Height];
            //Image ->byte[]変換
            {
                Bitmap b=new Bitmap(im);
                for (int y = 0; y < im.Height; y++)
                {
                    for (int x = 0; x < im.Width; x++)
                    {
                        Color c = b.GetPixel(x, y);
                        buf[y * im.Width + x] =(byte)((c.G + c.B + c.R) / 3);
                    }
                }
            }
            return new JpegIO.DecodeResult(xd, yd,buf , im.Width, im.Height, unit);
        }
        /**
         * http://stackoverflow.com/questions/233504/write-dpi-metadata-to-a-jpeg-image-in-java
         * @param w
         * @param h
         * @param i_x_dpi
         * @param i_y_dpi
         * @param i_dpi_unit
         * @param i_src
         * @param i_quority
         * @return
         * @throws IOException
         */
        public static byte[] encode(int w, int h, int i_x_dpi, int i_y_dpi, int i_dpi_unit, byte[] i_src, float i_quority)
        {
            throw new NyARRuntimeException();
            //BufferedImage img = new BufferedImage(w ,h ,BufferedImage.TYPE_BYTE_GRAY);
            //WritableRaster wr=img.getRaster();
            //DataBufferByte buf=(DataBufferByte)wr.getDataBuffer();
            //System.arraycopy(i_src, 0, buf.getData(),0,w*h);

            //ByteArrayOutputStream bout=new ByteArrayOutputStream();
            //JPEGImageWriter jw=(JPEGImageWriter)ImageIO.getImageWritersBySuffix("jpeg").next();
            //jw.setOutput(new MemoryCacheImageOutputStream(bout));

            //// Compression
            //JPEGImageWriteParam jpegParams = (JPEGImageWriteParam) jw.getDefaultWriteParam();
            //jpegParams.setCompressionMode(JPEGImageWriteParam.MODE_EXPLICIT);
            //jpegParams.setCompressionQuality(i_quority);

            //// Metadata (dpi)

            //IIOMetadata data = jw.getDefaultImageMetadata(new ImageTypeSpecifier(img), jpegParams);
            //Element tree = (Element)data.getAsTree("javax_imageio_jpeg_image_1.0");
            //Element jfif = (Element)tree.getElementsByTagName("app0JFIF").item(0);
            //jfif.setAttribute("Xdensity", Integer.toString(i_x_dpi));
            //jfif.setAttribute("Ydensity", Integer.toString(i_y_dpi));
            //jfif.setAttribute("resUnits", Integer.toString(i_dpi_unit)); // density is dots per inch
            //data.setFromTree("javax_imageio_jpeg_image_1.0",tree);

            //// Write and clean up
            //jw.write(null,  new IIOImage(img, null, data), jpegParams);
            //byte[] ret=bout.toByteArray();
            //bout.close();
            //jw.dispose();
            //return ret;
        }
    }
}

#endif
