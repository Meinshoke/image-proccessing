using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace lab
{
    /// <summary>
    /// Interaction logic for WatermarkWindow.xaml
    /// </summary>
    public partial class WatermarkWindow : Window
    {
        Bitmap img;
        Bitmap watermark;
        private Bitmap _saveMark;
        private Bitmap _saveImg;
        public WatermarkWindow()
        {
            InitializeComponent();
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        private void LoadImg(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files |*.png;*.jpg;*.jpeg;*.tif" + "|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var img = new Bitmap(openFileDialog.FileName);
                this.img = img;
                ImageView.Source = BitmapToImageSource(img);
                _saveImg = img;
            }
        }
        private void LoadMark(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files |*.png;*.jpg;*.jpeg;*.tif" + "|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var img = new Bitmap(openFileDialog.FileName);
                this.watermark = img;
                WatermarkView.Source = BitmapToImageSource(img);
                _saveMark = img;
            }
        }

        private void SaveImg(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files |*.png;*.tif|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapToImageSource(_saveImg)));
                using (var stream = saveFileDialog.OpenFile())
                {
                    encoder.Save(stream);
                }
                ImageView.Source = null;
            }
        }
        private void SaveMark(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files |*.png;*.tif|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapToImageSource(_saveMark)));
                using (var stream = saveFileDialog.OpenFile())
                {
                    encoder.Save(stream);
                }
                WatermarkView.Source = null;
            }
        }

        private void ApplyMark(object sender, RoutedEventArgs e)
        {
            _saveImg = Watermark.ApplyWatermark(img, watermark);
            ImageView.Source = BitmapToImageSource(_saveImg);
        }

        private void ReadMark(object sender, RoutedEventArgs e)
        {
            _saveMark = Watermark.ReedWatermark(img);
            WatermarkView.Source = BitmapToImageSource(_saveMark);
        }
    }
}
