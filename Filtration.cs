using System;
using System.Drawing;

namespace lab
{
    class Filtration
    {
        public static Bitmap Grayscale(Bitmap _bitmap) 
        {
            var bitmap = new Bitmap(_bitmap);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i,j);
                    var grey = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
                    bitmap.SetPixel(i, j, Color.FromArgb(255, grey, grey, grey));
                }
            } 
            return bitmap;
        }
        public static Bitmap UseFiler(Bitmap _bitmap, int[][] kernel) 
        {
            int offset = (int)(kernel.Length / 2);
            var bitmap = new Bitmap((int)(_bitmap.Width - 2 * offset), (int)(_bitmap.Height - 2 * offset));
            for (int i = offset; i < _bitmap.Width - offset; i++)
            {
                for (int j = offset; j < _bitmap.Height - offset; j++)
                {
                    int counter = 0;
                    int[][] R = new int[kernel.Length][];
                    int[][] G = new int[kernel.Length][];
                    int[][] B = new int[kernel.Length][];
                    for (int k = -offset; k <= offset; k++)
                    {
                        int _counter = 0;
                        int[] _R = new int[kernel.Length];
                        int[] _G = new int[kernel.Length];
                        int[] _B = new int[kernel.Length];
                        for (int h = -offset; h <= offset; h++)
                        {
                            _R[_counter] = _bitmap.GetPixel(k + i, h + j).R;
                            _G[_counter] = _bitmap.GetPixel(k + i, h + j).G;
                            _B[_counter] = _bitmap.GetPixel(k + i, h + j).B;
                            _counter++;
                        }
                        R[counter] = _R;
                        G[counter] = _G;
                        B[counter] = _B;
                        counter++;
                    }
                    var r = Convolution(R, kernel);
                    var g = Convolution(G, kernel);
                    var b = Convolution(B, kernel);
                    bitmap.SetPixel(i - offset, j - offset, Color.FromArgb(255, r, g, b));
                }
            }
            return bitmap;
        }
        private static int Convolution(int[][] img, int[][] kernel)
        {
            int sum = 0;
            int result = 0;
            for (int i = 0; i < img.Length; i++)
            {
                for (int j = 0; j < img.Length; j++)
                {
                    sum += kernel[i][j];
                    result += img[i][j] * kernel[i][j];
                }
            }
            var retResult = Math.Abs(result / (sum != 0 ? sum : 2));
            return Math.Min(retResult, byte.MaxValue);
        }
    }
}
