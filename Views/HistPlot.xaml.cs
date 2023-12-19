using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace lab
{
    /// <summary>
    /// Interaction logic for HistPlot.xaml
    /// </summary>
    public partial class HistPlot : Window
    {
        public Dictionary<double, double> ToHistogram(Bitmap _bitmap)
        {
            var bitmap = (Bitmap)_bitmap.Clone();
            var data = new Dictionary<double, double>();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var grey = (byte)(color.R / 3 +  color.G / 3 + color.B / 3);
                    if (data.ContainsKey(grey))
                        data[grey] += 1;
                    else
                        data.Add(grey, 1);
                }
            }
            return data;
        }
        public HistPlot(Bitmap bitmap)
        {
            InitializeComponent();
            var plotData = ToHistogram(bitmap);
            double[] positions = plotData.Keys.ToArray();
            double[] values = plotData.Values.ToArray();
            Histogram_Plot.Plot.AddBar(values,positions);
            Histogram_Plot.Refresh();
        }
    }
}
