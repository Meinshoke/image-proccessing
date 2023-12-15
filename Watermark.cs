using System;
using System.Drawing;
using AForge.Imaging.Filters;
using System.Collections;
using System.Linq;
using System.Diagnostics;

namespace lab
{
    class Watermark
    {
        private static byte GetByte(byte[] bits)
        {
            var bitString = "";
            for (int i = 0; i < 8; i++)
                bitString += bits[i];
            byte newpix = Convert.ToByte(bitString, 2);
            int dePix = newpix ^ 2;
            return (byte)dePix;
        }
        private static byte[] GetBits(byte simplepixel)
        {
            int pixel = 0;
            pixel = simplepixel ^ 2;
            var bits = new BitArray(new byte[] { (byte)pixel });
            bool[] boolarray = new bool[bits.Count];
            bits.CopyTo(boolarray, 0);
            byte[] bitsArray = boolarray.Select(bit => (byte)(bit ? 1 : 0)).ToArray();
            Array.Reverse(bitsArray);
            return bitsArray;
        }
        public static Bitmap ApplyWatermark(Bitmap _bitmap, Bitmap _watermark)
        {
            var bitmap = new Bitmap(_bitmap);
            Bitmap secretGreyScale = (Bitmap)_watermark.Clone();
            if (secretGreyScale.Height != bitmap.Height || secretGreyScale.Width != bitmap.Width)
            {
                ResizeBilinear resizeFilter = new ResizeBilinear(bitmap.Width, bitmap.Height);
                secretGreyScale = resizeFilter.Apply(secretGreyScale);
            }

            byte[] MsgBits;
            byte[] AlphaBits;
            byte[] RedBits;
            byte[] GreenBits;
            byte[] BlueBits;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var pixelMsgImage = secretGreyScale.GetPixel(j, i);
                    MsgBits = GetBits(pixelMsgImage.R);
                    var pixelContainerImage = bitmap.GetPixel(j, i);
                    AlphaBits = GetBits(pixelContainerImage.A);
                    RedBits = GetBits(pixelContainerImage.R);
                    GreenBits = GetBits(pixelContainerImage.G);
                    BlueBits = GetBits(pixelContainerImage.B);
                    AlphaBits[6] = MsgBits[0];
                    AlphaBits[7] = MsgBits[1];
                    RedBits[6] = MsgBits[2];
                    RedBits[7] = MsgBits[3];
                    GreenBits[6] = MsgBits[4];
                    GreenBits[7] = MsgBits[5];
                    BlueBits[6] = MsgBits[6];
                    BlueBits[7] = MsgBits[7];
                    byte newAlpha = GetByte(AlphaBits);
                    byte newRed = GetByte(RedBits);
                    byte newGreen = GetByte(GreenBits);
                    byte newBlue = GetByte(BlueBits);
                    bitmap.SetPixel(j, i, Color.FromArgb(newAlpha, newRed, newGreen, newBlue));
                }
            }
            return bitmap;
        }
        public static Bitmap ReedWatermark(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            var watermark = new Bitmap(_bitmap);

            byte[] BitsToDecrypt = new byte[8];
            byte[] AlphaBits;
            byte[] RedBits;
            byte[] GreenBits;
            byte[] BlueBits;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var pixelToDecrypt = bitmap.GetPixel(j, i);
                    AlphaBits = GetBits(pixelToDecrypt.A);
                    RedBits = GetBits(pixelToDecrypt.R);
                    GreenBits = GetBits(pixelToDecrypt.G);
                    BlueBits = GetBits(pixelToDecrypt.B);
                    BitsToDecrypt[0] = AlphaBits[6];
                    BitsToDecrypt[1] = AlphaBits[7];
                    BitsToDecrypt[2] = RedBits[6];
                    BitsToDecrypt[3] = RedBits[7];
                    BitsToDecrypt[4] = GreenBits[6];
                    BitsToDecrypt[5] = GreenBits[7];
                    BitsToDecrypt[6] = BlueBits[6];
                    BitsToDecrypt[7] = BlueBits[7];
                    byte newGrey = GetByte(BitsToDecrypt);
                    watermark.SetPixel(j, i, Color.FromArgb(newGrey, newGrey, newGrey));
                }
            }
            return watermark;
        }
    }
}
