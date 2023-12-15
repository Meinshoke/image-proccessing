using System.Drawing;

namespace lab
{
    class Normalize
    {
        public static Bitmap Normalization(Bitmap _bitmap) 
        {
            var bitmap = new Bitmap(_bitmap);
            int[] brightness = new int[256];
            for (int i = 0; i < 256; i++)
                brightness[i] = 0;

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                    brightness[bitmap.GetPixel(i, j).R] += 1;

            int[] cdf = new int[256];
            cdf[0] = brightness[0];
            for (int i = 1; i < 256; i++)
                cdf[i] = cdf[i - 1] + brightness[i];

            int totalPixels = bitmap.Width * bitmap.Height;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixelColor = bitmap.GetPixel(x, y);
                    int newIntensity = (int)(((double)cdf[pixelColor.R] - cdf[0]) / (totalPixels - cdf[0]) * 255.0);
                    bitmap.SetPixel(x, y, Color.FromArgb(newIntensity, newIntensity, newIntensity));
                }
            }
            return bitmap;
        }
    }
}
