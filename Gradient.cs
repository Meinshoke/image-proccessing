using System;
using System.Drawing;

namespace lab
{
    class Gradient
    {
        public static Bitmap Negative(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var r = byte.MaxValue - color.R;
                    var g = byte.MaxValue - color.G;
                    var b = byte.MaxValue - color.B;
                    bitmap.SetPixel(i, j, Color.FromArgb(255, r, g, b));
                }
            }
            return bitmap;
        }
        public static Bitmap LogTransform(Bitmap _bitmap,double n)
        {
            var bitmap = new Bitmap(_bitmap);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    double r = 255 / Math.Log(1 + Math.Pow(10, n) * 255) * Math.Log(1 + Math.Pow(10, n) * color.R);
                    double g = 255 / Math.Log(1 + Math.Pow(10, n) * 255) * Math.Log(1 + Math.Pow(10, n) * color.G);
                    double b = 255 / Math.Log(1 + Math.Pow(10, n) * 255) * Math.Log(1 + Math.Pow(10, n) * color.B);
                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));
                    bitmap.SetPixel(i, j, Color.FromArgb(255, (int)r, (int)g, (int)b));
                }
            }
            return bitmap;
        }
        public static Bitmap ExpTransform(Bitmap _bitmap, double c)
        {
            var bitmap = new Bitmap(_bitmap);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    double r = color.R / 255.0;
                    double g = color.G / 255.0;
                    double b = color.B / 255.0;
                    double sR = 255 * Math.Pow(r, c);
                    double sG = 255 * Math.Pow(g, c);
                    double sB = 255 * Math.Pow(b, c);
                    sR = Math.Max(0, Math.Min(255, sR));
                    sG = Math.Max(0, Math.Min(255, sG));
                    sB = Math.Max(0, Math.Min(255, sB));
                    bitmap.SetPixel(i, j, Color.FromArgb(color.A, (int)sR, (int)sG, (int)sB));
                }
            }
            return bitmap;
        }
    }
}
