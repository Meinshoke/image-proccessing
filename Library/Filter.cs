using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace lab.Library
{
    class Filter
    {
        public static byte GetMedian(byte[] arrSource)
        {
            byte[] arrSorted = (byte[])arrSource.Clone();
            Array.Sort(arrSorted);
            int size = arrSorted.Length;
            int mid = size / 2;

            if (size % 2 != 0)
                return arrSorted[mid];

            dynamic value1 = arrSorted[mid];
            dynamic value2 = arrSorted[mid - 1];
            return (value1 + value2) / 2;
        }
        public static Bitmap MedianFilter(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
            bitmap.UnlockBits(bitmapData);
            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;

            var resultBitmap = new Bitmap(wres, hres);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int resBytes = resultData.Stride * resultData.Height;
            byte[] result = new byte[resBytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixelLocation = x * 3 + y * bitmapData.Stride;
                    int resPixelLoc = (x - r) * 3 + (y - r) * resultData.Stride;
                    byte[][] neighborhood = new byte[3][];

                    for (int c = 0; c < 3; c++)
                    {
                        neighborhood[c] = new byte[(int)Math.Pow(2 * r + 1, 2)];
                        int added = 0;
                        for (int kx = -r; kx <= r; kx++)
                        {
                            for (int ky = -r; ky <= r; ky++)
                            {
                                int kernelPixel = pixelLocation + kx * 3 + ky * bitmapData.Stride;
                                neighborhood[c][added] = buffer[kernelPixel + c];
                                added++;
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[resPixelLoc + c] = GetMedian(neighborhood[c]);
                    }
                }
            }

            Marshal.Copy(result, 0, resultData.Scan0, resBytes);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public static Bitmap ArithmeticFilter(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
            bitmap.UnlockBits(bitmapData);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap resultBitmap = new Bitmap(wres, hres);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int resBytes = resultData.Stride * resultData.Height;
            byte[] result = new byte[resBytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixelLocation = x * 3 + y * bitmapData.Stride;
                    int resPixelLoc = (x - r) * 3 + (y - r) * resultData.Stride;
                    double[] mean = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernelPixel = pixelLocation + kx * 3 + ky * bitmapData.Stride;

                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] += buffer[kernelPixel + c] / Math.Pow(2 * r + 1, 2);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[resPixelLoc + c] = (byte)mean[c];
                    }
                }
            }

            Marshal.Copy(result, 0, resultData.Scan0, resBytes);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public static Bitmap GeometricFilter(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
            bitmap.UnlockBits(bitmapData);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;

            Bitmap resultBitmap = new Bitmap(wres, hres);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int resBytes = resultData.Stride * resultData.Height;
            byte[] result = new byte[resBytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixelLocation = x * 3 + y * bitmapData.Stride;
                    int resPixelLoc = (x - r) * 3 + (y - r) * resultData.Stride;
                    double[] mean = new double[3];

                    for (int i = 0; i < mean.Length; i++)
                    {
                        mean[i] = 1;
                    }

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernelPixel = pixelLocation + kx * 3 + ky * bitmapData.Stride;

                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] *= buffer[kernelPixel + c];
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[resPixelLoc + c] = (byte)Math.Pow(mean[c], 1 / Math.Pow(2 * r + 1, 2));
                    }
                }
            }

            Marshal.Copy(result, 0, resultData.Scan0, resBytes);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public static Bitmap HarmonicFilter(Bitmap _bitmap)
        {
            var bitmap = new Bitmap(_bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
            bitmap.UnlockBits(bitmapData);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap resultBitmap = new Bitmap(wres, hres);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int resBytes = resultData.Stride * resultData.Height;
            byte[] result = new byte[resBytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixelLocation = x * 3 + y * bitmapData.Stride;
                    int resPixelLoc = (x - r) * 3 + (y - r) * resultData.Stride;
                    double[] mean = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernelPixel = pixelLocation + kx * 3 + ky * bitmapData.Stride;
                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] += 1d / buffer[kernelPixel + c];
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[resPixelLoc + c] = (byte)(Math.Pow(2 * r + 1, 2) / mean[c]);
                    }
                }
            }

            Marshal.Copy(result, 0, resultData.Scan0, resBytes);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public static Bitmap ContraharmonicFilter(Bitmap _bitmap, double order)
        {
            var bitmap = new Bitmap(_bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = bitmapData.Stride * bitmapData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);
            bitmap.UnlockBits(bitmapData);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap resultBitmap = new Bitmap(wres, hres);
            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int resBytes = resultData.Stride * resultData.Height;
            byte[] result = new byte[resBytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixelLocation = x * 3 + y * bitmapData.Stride;
                    int resPixelLoc = (x - r) * 3 + (y - r) * resultData.Stride;
                    double[] sum1 = new double[3];
                    double[] sum2 = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernelPixel = pixelLocation + kx * 3 + ky * bitmapData.Stride;

                            for (int c = 0; c < 3; c++)
                            {
                                sum1[c] += Math.Pow(buffer[kernelPixel + c], order + 1);
                                sum2[c] += Math.Pow(buffer[kernelPixel + c], order);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[resPixelLoc + c] = (byte)(sum1[c] / sum2[c]);
                    }
                }
            }

            Marshal.Copy(result, 0, resultData.Scan0, resBytes);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}
