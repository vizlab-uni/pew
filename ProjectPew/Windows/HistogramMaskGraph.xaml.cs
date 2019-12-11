using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace ProjectHeracles.Windows
{
    /// <summary>
    /// Interaction logic for HistogramMaskGraph.xaml
    /// </summary>
    public partial class HistogramMaskGraph : Window
    {
        public HistogramMaskGraph()
        {
            InitializeComponent();

            HistogramCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "hist",
                    Values = new ChartValues<int> { },
                    MaxColumnWidth = 100,
                    ColumnPadding = 0
                },

                new LineSeries
                {
                    Title = "cdf",
                    Values = new ChartValues<int> { },
                    PointGeometrySize = 5,
                }
        };
            DataContext = this;
        }

        public SeriesCollection HistogramCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Mat srcImage { get; set; }
        public Mat srcImage_Gray { get; set; }
        public Mat previewImage { get; set; }

        Dictionary<int, List<int[]>> histoPixels = new Dictionary<int, List<int[]>>();

        private void CartesianChart_DataClick(object sender, ChartPoint chartPoint)
        {
            int index = chartPoint.Key;
            chartPoint.SeriesView.Values[index] = (double)chartPoint.SeriesView.Values[index] + 0.5;
        }

        void OpenImage(string path)
        {
            srcImage_Gray = new Mat();
            srcImage = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            CvInvoke.CvtColor(srcImage, srcImage_Gray, ColorConversion.Bgra2Gray);

            previewImage = new Mat(srcImage.Rows, srcImage.Cols, srcImage_Gray.Depth, srcImage_Gray.NumberOfChannels);

            imgSource.Source = HeraclesHelper.ToBitmapSource(srcImage);
        }

        Array GetHistogramValues()
        {
            Mat hist = new Mat();
            using (Emgu.CV.Util.VectorOfMat vm = new Emgu.CV.Util.VectorOfMat())
            {
                int[] channels = { 0, 1, 2 };
                int[] histSize = { 32, 32, 32 };
                float[] ranges = { 0.0f, 256.0f, 0.0f, 256.0f, 0.0f, 256.0f };


                vm.Push(srcImage_Gray);
                CvInvoke.CalcHist(vm, new int[] { 0 }, null, hist, new int[] { 256 }, new float[] { 0.0f, 256.0f }, false);

                CvInvoke.Normalize(hist, hist, 0, srcImage_Gray.Rows, NormType.MinMax);
            }
            return hist.GetData();
        }

        private void SetPreviewPixel(Dictionary<int, List<int[]>> histoPixels, int pixelColor)
        {
            for (int i = 0; i < histoPixels.Count; i++)
            {
                if (i == pixelColor)
                {
                    for (int j = 0; j < histoPixels[i].Count; j++)
                    {
                        previewImage.GetData().SetValue((byte)pixelColor, histoPixels[i][j]);
                    }

                    imgPreview.Source = HeraclesHelper.ToBitmapSource(previewImage);
                    return;
                }
            }
        }

        private Dictionary<int, List<int[]>> GetHistogramPixels(Array histogram)
        {
            Dictionary<int, List<int[]>> histoPixels = new Dictionary<int, List<int[]>>();

            for (int i = 0; i < srcImage_Gray.Rows; i++)
            {
                for (int j = 0; j < srcImage_Gray.Cols; j++)
                {
                    //Get pixel color
                    int pixelVal = Convert.ToInt32(srcImage_Gray.GetData().GetValue(i, j));
                    //If pixel isn`t registred on dictonary, add as a key
                    if (!histoPixels.ContainsKey(pixelVal))
                    {
                        histoPixels.Add(pixelVal, new List<int[]>());
                    }
                    //add coordinates of pixel Color (key) on value
                    histoPixels[pixelVal].Add(new int[] { i, j });
                }
            }

            return histoPixels;
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Load image to srcImage
                OpenImage(openFileDialog.FileName);

                if (!srcImage.IsEmpty)
                {
                    Array vals = GetHistogramValues();
                    
                    histoPixels = GetHistogramPixels(vals);

                    int cdfSum = 0;
                    int val;
                    for (int i = 0; i < 256; i++)
                    {
                        val = Convert.ToInt32(vals.GetValue(i, 0));
                        HistogramCollection[0].Values.Add(val);
                        cdfSum += val;
                        HistogramCollection[1].Values.Add(cdfSum);
                    }
                }
            }
        }

        private void HistogramChart_Click(object sender, ChartPoint chartPoint)
        {
            if (!srcImage_Gray.IsEmpty)
            {
                int index = chartPoint.Key;
                SetPreviewPixel(histoPixels, index);
                //for (int i = 0; i < srcImage_Gray.Rows; i++)
                //{
                //    for (int j = 0; j < srcImage_Gray.Cols; j++)
                //    {
                //        if(Convert.ToInt32(srcImage_Gray.GetData().GetValue(i,j)) == index)
                //        {
                //            previewImage.GetData().SetValue(srcImage_Gray.GetData().GetValue(i, j), new int[] { i, j }) ;
                //        }
                //    }
                //}
                //imgPreview.Source = BitmapSourceConvert.ToBitmapSource(previewImage);
            }
        }
    }
}
