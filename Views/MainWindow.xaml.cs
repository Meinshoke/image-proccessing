using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using lab.Library;

namespace lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap img;
        private Bitmap _forSave;
        public int[][] customKernel;
        public int size = 3;

        public MainWindow()
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
                _forSave = img;
            }
        }

        private void SaveImg(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files |*.png;*.tif|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapToImageSource(_forSave)));
                using (var stream = saveFileDialog.OpenFile())
                {
                    encoder.Save(stream);
                }
                ImageView.Source = null;
            }
        }

        private void UseMask(object sender, RoutedEventArgs e)
        {
            if (size == 3)
            {
                customKernel = new int[3][]
                {
                        new int[3] { Convert.ToInt32(Text00.Text), Convert.ToInt32(Text01.Text), Convert.ToInt32(Text02.Text) },
                        new int[3] { Convert.ToInt32(Text10.Text), Convert.ToInt32(Text11.Text), Convert.ToInt32(Text12.Text) },
                        new int[3] { Convert.ToInt32(Text20.Text), Convert.ToInt32(Text21.Text), Convert.ToInt32(Text22.Text) }
                };
            }
            else 
            {
                customKernel = new int[5][]
                {
                        new int[5] { Convert.ToInt32(Text00.Text), Convert.ToInt32(Text01.Text), Convert.ToInt32(Text02.Text), Convert.ToInt32(Text03.Text), Convert.ToInt32(Text04.Text) },
                        new int[5] { Convert.ToInt32(Text10.Text), Convert.ToInt32(Text11.Text), Convert.ToInt32(Text12.Text), Convert.ToInt32(Text13.Text), Convert.ToInt32(Text14.Text) },
                        new int[5] { Convert.ToInt32(Text20.Text), Convert.ToInt32(Text21.Text), Convert.ToInt32(Text22.Text), Convert.ToInt32(Text23.Text), Convert.ToInt32(Text24.Text) },
                        new int[5] { Convert.ToInt32(Text30.Text), Convert.ToInt32(Text31.Text), Convert.ToInt32(Text32.Text), Convert.ToInt32(Text33.Text), Convert.ToInt32(Text34.Text) },
                        new int[5] { Convert.ToInt32(Text40.Text), Convert.ToInt32(Text41.Text), Convert.ToInt32(Text42.Text), Convert.ToInt32(Text43.Text), Convert.ToInt32(Text44.Text) }
                };
            }
            int[][] kernel = Masks_Comb.SelectedIndex switch
            {
                0 => new int[3][]
                {
                    new int[3] { 2, 2, 2 },
                    new int[3] { 2, 0, 2 },
                    new int[3] { 2, 2, 2 }
                },
                1 => new int[3][]
                {
                    new int[3] { -1, -1, -1 },
                    new int[3] { -1, 16, -1 },
                    new int[3] { -1, -1, -1 }
                },
                2 => new int[3][]
                {
                    new int[3] { -1, -1, -1 },
                    new int[3] { -1, 9, -1 },
                    new int[3] { -1, -1, -1 }
                },
                3 => new int[3][]
                {
                    new int[3] { 1, 1, 1 },
                    new int[3] { 1, -2, 1 },
                    new int[3] { -1, -1, -1 }
                },
                4 => new int[3][]
                {
                    new int[3] { -1, -1, -1 },
                    new int[3] { -1, 8, -1 },
                    new int[3] { -1, -1, -1 }
                },
                5 => new int[3][]
                {
                    new int[3] { 1, 2, 1 },
                    new int[3] { 2, 4, 2 },
                    new int[3] { 1, 2, 1 }
                },
                6 => new int[3][]
                {
                    new int[3] { -1, 0, 1 },
                    new int[3] { -1, 0, 1 },
                    new int[3] { -1, 0, 1 }
                },
                7 => new int[3][]
                {
                    new int[3] { 1, 1, 1 },
                    new int[3] { 0, 0, 0 },
                    new int[3] { -1, -1, -1 }
                },
                8 => new int[3][]
                {
                    new int[3] { -1, 0, 1 },
                    new int[3] { -2, 0, 2 },
                    new int[3] { -1, 0, 1 }
                },
                9 => new int[3][]
                {
                    new int[3] { 1, 2, 1 },
                    new int[3] { 0, 0, 0 },
                    new int[3] { -1, -2, -1 }
                },
                10 => new int[5][]
                {
                    new int[5] { -1, -3, -4, -3, -1 },
                    new int[5] { -3, 0, 6, 0, -3 },
                    new int[5] { -4, 6, 20, 6, -4 },
                    new int[5] { -3, 0, 6, 0, -3 },
                    new int[5] { -1, -3, -4, -3, -1 }
                },
                11 => new int[5][]
                {
                    new int[5] { 2, 7, 12, 7, 2 },
                    new int[5] { 7, 31, 52, 31, 7 },
                    new int[5] { 12, 52, 127, 52, 12 },
                    new int[5] { 7, 31, 52, 31, 7 },
                    new int[5] { 2, 7, 12, 7, 2 }
                },
                12 => customKernel,
                _ => new int[3][]
                {
                    new int[3] { 0, 0, 0 },
                    new int[3] { 0, 1, 0 },
                    new int[3] { 0, 0, 0 }
                }
            };
            _forSave = Filtration.UseFiler(img, kernel);
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void ToGrey(object sender, RoutedEventArgs e)
        {
            _forSave = Filtration.Grayscale(img);
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void UseGradient(object sender, RoutedEventArgs e)
        {
            switch (Gradients_Comb.SelectedIndex) 
            { 
                case 0:
                    _forSave = Gradient.Negative(img); break;

                case 1:
                    _forSave = Gradient.LogTransform(img, Convert.ToDouble(DataInput1.Text)); break;
                
                case 2:
                    _forSave = Gradient.ExpTransform(img, Convert.ToDouble(DataInput1.Text)); break;
            }
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void GetHistogram(object sender, RoutedEventArgs e)
        {
            var plot = new HistPlot(_forSave);
            plot.Show();
        }

        private void UseNorm(object sender, RoutedEventArgs e)
        {
            _forSave = Normalize.Normalization(img);
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void AddNoise(object sender, RoutedEventArgs e)
        {
            _forSave = Noise_Comb.SelectedIndex switch
            {
                0 => _forSave = Noise.RayleighNoise(img, Convert.ToDouble(DataInput1.Text), Convert.ToDouble(DataInput2.Text)),
                1 => _forSave = Noise.GaussianNoise(img, Convert.ToInt32(DataInput1.Text)),
                2 => _forSave = Noise.ImpulseNoise(img, Convert.ToInt32(DataInput1.Text)),
                _ => img,
            };
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void UseFiter(object sender, RoutedEventArgs e)
        {
            _forSave = Filter_Comb.SelectedIndex switch
            {
                0 => Filter.MedianFilter(img),
                1 => Filter.ArithmeticFilter(img),
                2 => Filter.GeometricFilter(img),
                3 => Filter.HarmonicFilter(img),
                4 => Filter.ContraharmonicFilter(img, Convert.ToDouble(DataInput1.Text)),
                _ => img,
            }; 
            ImageView.Source = BitmapToImageSource(_forSave);
        }

        private void Noise_Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Stats is null)
                return;
            Stats.Visibility = Visibility.Visible;
            DataInput2.Visibility = Visibility.Visible;
            switch (Noise_Comb.SelectedIndex)
            {
                case 0:
                    DataLabel1.Content = "a = ";
                    DataLabel2.Content = "b = ";
                    DataInput1.Text = "0";
                    DataInput2.Text = "0,4";
                    break;
                case 1:
                    DataLabel1.Content = "StD = ";
                    DataLabel2.Content = "";
                    DataInput1.Text = "15";
                    DataInput2.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    DataLabel1.Content = "Chnc = ";
                    DataLabel2.Content = "";
                    DataInput1.Text = "15";
                    DataInput2.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Filter_Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Filter_Comb.SelectedIndex == 4)
            {
                Stats.Visibility = Visibility.Visible;
                DataLabel1.Content = "Q = ";
                DataLabel2.Content = "";
                DataInput1.Text = "0,1";
                DataInput2.Visibility = Visibility.Collapsed;
            }
        }

        private void Matrix_Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Matrix_Size.SelectedIndex)
            {
                case 0:
                    Text03.Visibility = Visibility.Collapsed;
                    Text04.Visibility = Visibility.Collapsed;
                    Text13.Visibility = Visibility.Collapsed;
                    Text14.Visibility = Visibility.Collapsed;
                    Text23.Visibility = Visibility.Collapsed;
                    Text24.Visibility = Visibility.Collapsed;
                    Text33.Visibility = Visibility.Collapsed;
                    Text34.Visibility = Visibility.Collapsed;
                    Text43.Visibility = Visibility.Collapsed;
                    Text44.Visibility = Visibility.Collapsed;
                    Text30.Visibility = Visibility.Collapsed;
                    Text31.Visibility = Visibility.Collapsed;
                    Text32.Visibility = Visibility.Collapsed;
                    Text40.Visibility = Visibility.Collapsed;
                    Text41.Visibility = Visibility.Collapsed;
                    Text42.Visibility = Visibility.Collapsed;
                    size = 3;
                    break;
                case 1:
                    Text03.Visibility = Visibility.Visible;
                    Text04.Visibility = Visibility.Visible;
                    Text13.Visibility = Visibility.Visible;
                    Text14.Visibility = Visibility.Visible;
                    Text23.Visibility = Visibility.Visible;
                    Text24.Visibility = Visibility.Visible;
                    Text33.Visibility = Visibility.Visible;
                    Text34.Visibility = Visibility.Visible;
                    Text43.Visibility = Visibility.Visible;
                    Text44.Visibility = Visibility.Visible;
                    Text30.Visibility = Visibility.Visible;
                    Text31.Visibility = Visibility.Visible;
                    Text32.Visibility = Visibility.Visible;
                    Text40.Visibility = Visibility.Visible;
                    Text41.Visibility = Visibility.Visible;
                    Text42.Visibility = Visibility.Visible;
                    size = 5;
                    break;
            }
        }

        private void Masks_Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MatrixInput is null)
                return; 
            if (Masks_Comb.SelectedIndex == 12)
                MatrixInput.Visibility = Visibility.Visible;
            else
                MatrixInput.Visibility = Visibility.Collapsed;
        }
        private void Gradients_Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Stats is null)
                return;
            Stats.Visibility = Visibility.Visible;
            DataInput2.Visibility = Visibility.Visible;
            switch (Gradients_Comb.SelectedIndex)
            {
                case 0:
                    Stats.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    DataLabel1.Content = "c = ";
                    DataLabel2.Content = "";
                    DataInput1.Text = "0,1";
                    DataInput2.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    DataLabel1.Content = "c = ";
                    DataLabel2.Content = "";
                    DataInput1.Text = "1,6";
                    DataInput2.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ToWatermark(object sender, RoutedEventArgs e)
        {
            var markWindow = new WatermarkWindow();
            markWindow.Show();
        }
    }
}
