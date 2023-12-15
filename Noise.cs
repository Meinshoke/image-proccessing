using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace lab
{
    class Noise
    {
        public static Bitmap ImpulseNoise(Bitmap _bitmap, int _chance)
        {
            var bitmap = new Bitmap(_bitmap);
            int totalPixels = bitmap.Width * bitmap.Height;
            int noisyPixels = totalPixels / _chance;
            var rand = new Random();
            for (int i = 0; i < noisyPixels; i++)
            {
                int x = rand.Next(0, bitmap.Width);
                int y = rand.Next(0, bitmap.Height);
                var noiseColor = (rand.NextDouble() < 0.5) ? Color.Black : Color.White;

                bitmap.SetPixel(x, y, noiseColor);
            }
            return bitmap;
        }
        public static Bitmap GaussianNoise(Bitmap _bitmap, double std)
        {
            var bitmap = new Bitmap(_bitmap);
            var rand = new Random();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    double u1 = 1.0 - rand.NextDouble();
                    double u2 = 1.0 - rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                    double noise = std * randStdNormal;
                    int red = (int)Math.Max(Math.Min(pixel.R + noise, 255), 0);
                    int green = (int)Math.Max(Math.Min(pixel.G + noise, 255), 0);
                    int blue = (int)Math.Max(Math.Min(pixel.B + noise, 255), 0);
                    bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return bitmap;
        }
        public static Bitmap RayleighNoise(Bitmap _bitmap, double a, double b)
        {
            var bitmap = new Bitmap(_bitmap);
            var rand = new Random();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    
                    var u = 1 - rand.NextDouble();
                    double noise = 0;
                    var mid = ((pixel.R + pixel.G + pixel.B) / 3) * 0.01;
                    if (mid >= a)
                        noise = b * Math.Sqrt(-2 * Math.Log(1 - u)) * 30; 
                    int red = (int)Math.Max(0.9 * Math.Min(pixel.R + noise, 255), 0);
                    int green = (int)Math.Max(0.9 * Math.Min(pixel.G + noise, 255), 0);
                    int blue = (int)Math.Max(0.9 * Math.Min(pixel.B + noise, 255), 0);

                    bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return bitmap;
        }
    }
}
