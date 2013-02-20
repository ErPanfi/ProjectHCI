using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace ProjectHCI.Utility
{

    /// <summary>
    /// Represent a Bitmap Image, the name "RgbData" can deceive the reader, this struct can represent Rgba too.
    /// </summary>
    public struct RgbData
    {
        public byte[] rawRgbByteArray;
        public int stride;
        public int dataLength;
        public int pixelWidth;
        public int pixelHeight;
        public double dpiX;
        public double dpiY;
        public PixelFormat pixelFormat;
    }

    

    public static class BitmapUtility
    {

        /// <summary>
        /// This method returns the RgbData struct representing the given BitmapSource
        /// </summary>
        /// <param name="bitmapSource">the target BitmapSource</param>
        /// <returns>RgbData representing the given BitmapSource</returns>
        static public RgbData getRgbData(BitmapSource bitmapSource)
        {

            RgbData rgbData = new RgbData();
            rgbData.stride = bitmapSource.PixelWidth * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            rgbData.dataLength = rgbData.stride * bitmapSource.PixelHeight;
            rgbData.pixelWidth = bitmapSource.PixelWidth;
            rgbData.pixelHeight = bitmapSource.PixelHeight;
            rgbData.dpiX = bitmapSource.DpiX;
            rgbData.dpiY = bitmapSource.DpiY;
            rgbData.pixelFormat = bitmapSource.Format;
            rgbData.rawRgbByteArray = new byte[rgbData.dataLength];

            bitmapSource.CopyPixels(rgbData.rawRgbByteArray, rgbData.stride, 0);

            return rgbData;
        }

        /// <summary>
        /// Create a new BitmapSource from the given RgbData
        /// </summary>
        /// <param name="rgbData">the source RgbData</param>
        /// <returns>a BitmapSource</returns>
        static public BitmapSource createBitmapSource(RgbData rgbData)
        {
            BitmapSource bitmapSource = BitmapSource.Create(rgbData.pixelWidth,
                                                        rgbData.pixelHeight, 
                                                        rgbData.dpiX, 
                                                        rgbData.dpiY, 
                                                        rgbData.pixelFormat, 
                                                        null, 
                                                        rgbData.rawRgbByteArray, 
                                                        rgbData.stride);
            return bitmapSource;
        }

       

        //private static void copyPixels(this BitmapSource source, PixelColor[,] pixels, int stride, int offset)
        //{
        //    var height = source.PixelHeight;
        //    var width = source.PixelWidth;
        //    var pixelBytes = new byte[height * width * 4];
        //    source.CopyPixels(pixelBytes, stride, 0);
        //    int y0 = offset / width;
        //    int x0 = offset - width * y0;
        //    for (int y = 0; y < height; y++)
        //        for (int x = 0; x < width; x++)
        //            pixels[x + x0, y + y0] = new PixelColor
        //            {
        //                Blue = pixelBytes[(y * width + x) * 4 + 0],
        //                Green = pixelBytes[(y * width + x) * 4 + 1],
        //                Red = pixelBytes[(y * width + x) * 4 + 2],
        //                Alpha = pixelBytes[(y * width + x) * 4 + 3],
        //            };
        //}


        //public static PixelColor[,] getPixels(BitmapSource source)
        //{
        //    if (source.Format != PixelFormats.Bgra32)
        //        source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

        //    int width = source.PixelWidth;
        //    int height = source.PixelHeight;
        //    PixelColor[,] result = new PixelColor[width, height];

        //    BitmapUtility.copyPixels(source, result, width * 4, 0);
        //    return result;
        //}

    }
}
