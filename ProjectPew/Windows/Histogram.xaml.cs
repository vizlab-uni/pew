
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Emgu.CV;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace ProjectHeracles.Windows
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window
    {
        #region Constructors

        private Histogram()
        {
            InitializeComponent();
            srcImage = null;
        }

        public Histogram(Mat srcImage)
        {
            InitializeComponent();

            initializeToggles();

            this.srcImage = srcImage;
            imgSource.Source = HeraclesHelper.ToBitmapSource(srcImage);
            InitChart();
            CalculateHistograms();
            CalculateCdf();

            DataContext = this;
        }

        private void initializeToggles()
        {
            colorChannelElements.Clear();


            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Red", Image = null });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Green", Image = null });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Blue", Image = null });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Red CDF", Image = null });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Green CDF", Image = null });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Blue CDF", Image = null });

            icTodoList.ItemsSource = colorChannelElements;
        }

        #endregion

        #region Private Attributes

        private Array grayHistogramData;
        private Array redHistogramData;
        private Array greenHistogramData;
        private Array blueHistogramData;

        private Array redCDFData;
        private Array greenCDFData;
        private Array blueCDFData;

        private List<ColorChannelItem> colorChannelElements = new List<ColorChannelItem>();

        #endregion

        #region GET - SET

        public Mat srcImage { get; set; }
        public SeriesCollection histogramChart { get; set; }

        #endregion

        #region private Functions

        private void InitChart()
        {
            histogramChart = new SeriesCollection();
            chart.AxisY.Clear();
            chart.AxisY.Add(
                new Axis
                {
                    MinValue = 0
                });
            chart.AxisY.Add(
                new Axis
                {
                    MinValue = 0,
                    Position = AxisPosition.RightTop,
                    MaxValue = 1
                });
            chart.Series = histogramChart;
        }

        private void CalculateHistograms()
        {
            redHistogramData = HeraclesHelper.GetHistogramOfImage(srcImage, 'r');
            greenHistogramData = HeraclesHelper.GetHistogramOfImage(srcImage, 'g');
            blueHistogramData = HeraclesHelper.GetHistogramOfImage(srcImage, 'b');

            //ColumnSeries redCs = CreateColumnSerie(redHistogramData, "Red");
            //ColumnSeries greenCs = CreateColumnSerie(greenHistogramData, "Green");
            //ColumnSeries blueCs = CreateColumnSerie(blueHistogramData, "Blue");

            //histogramChart.Add(redCs);
            //histogramChart.Add(greenCs);
            //histogramChart.Add(blueCs);
            Brush rb = new SolidColorBrush(Brushes.Red.Color);
            rb.Opacity = 0.3;
            Brush gb = new SolidColorBrush(Brushes.Green.Color);
            gb.Opacity = 0.3;
            Brush bb = new SolidColorBrush(Brushes.Blue.Color);
            bb.Opacity = 0.3;
            LineSeries redLs = CreateLineSerie(redHistogramData, "Red", Brushes.Red, 1,0,null,rb);
            LineSeries greenLs = CreateLineSerie(greenHistogramData, "Green", Brushes.Green, 1, 0, null, gb);
            LineSeries blueLs = CreateLineSerie(blueHistogramData, "Blue", Brushes.Blue, 1, 0, null, bb);

            histogramChart.Add(redLs);
            histogramChart.Add(greenLs);
            histogramChart.Add(blueLs);

        }

        private ColumnSeries CreateColumnSerie(Array data, string title, int maxColumnWidth=100, int columnPadding=0)
        {
            ColumnSeries cs = new ColumnSeries
            {
                Title = title,
                MaxColumnWidth = maxColumnWidth,
                ColumnPadding = columnPadding,
                Values = new ChartValues<int> { }
            };
            
            int val;
            for (int i = 0; i < data.Length; i++)
            {
                val = Convert.ToInt32(data.GetValue(i));
                cs.Values.Add(val);
            }

            return cs;
        }

        private LineSeries CreateLineSerie(Array data, string title, Brush color, int pointSize = 5, int scalesYAt = 0, DoubleCollection strokeDashArray = null, Brush fill = null)
        {
            //Default value for fill must be a compile time constant, so we have to make
            //the default treatment with if command
            if (fill == null)
                fill = Brushes.Transparent;
            LineSeries ls = new LineSeries
            {
                Title = title,
                PointGeometrySize = pointSize,
                Stroke = color,
                Fill = fill,
                Values = new ChartValues<decimal> { },
                ScalesYAt = scalesYAt,
                PointGeometry = null,
                StrokeDashArray = strokeDashArray

            };

            decimal val;
            for (int i = 0; i < data.Length; i++)
            {
                val = Convert.ToDecimal(data.GetValue(i));
                ls.Values.Add(val);
            }
            
            return ls;
        }

        private void CalculateCdf()
        {
            int numberOfPixels = this.srcImage.Height * this.srcImage.Width;

            this.redCDFData = HeraclesHelper.GetCDFOfHistogram(this.redHistogramData, numberOfPixels);
            this.greenCDFData = HeraclesHelper.GetCDFOfHistogram(this.greenHistogramData, numberOfPixels);
            this.blueCDFData = HeraclesHelper.GetCDFOfHistogram(this.blueHistogramData, numberOfPixels);

            LineSeries redLs =  CreateLineSerie(redCDFData, "Red CDF", Brushes.Red, 1,1, new DoubleCollection { 2 });
            LineSeries greenLs = CreateLineSerie(greenCDFData, "Green CDF", Brushes.Green, 1,1, new DoubleCollection { 2 });
            LineSeries blueLs = CreateLineSerie(blueCDFData, "Blue CDF", Brushes.Blue, 1, 1, new DoubleCollection { 2 });

            histogramChart.Add(redLs);
            histogramChart.Add(greenLs);
            histogramChart.Add(blueLs);

        }

        private void BtnGrayScale_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRGB_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void HistogramChart_Click(object sender, ChartPoint chartPoint)
        {

        }

        private void TglColorElements_Click(object sender, RoutedEventArgs e)
        {
            string tglName = ((ToggleButton)sender).CommandParameter.ToString();

            bool[] tglStates = { true, true, true, true, true, true };

            //starts in 1 to ignore rgb element
            for (int i = 0; i < colorChannelElements.Count; i++)
            {
                tglStates[i] = colorChannelElements[i].IsSelected;
                LineSeries ls = ((LineSeries)chart.Series[i]);
                ls.Visibility = tglStates[i] ? Visibility.Visible:Visibility.Hidden;

            }
        }
        #endregion

    }
}
