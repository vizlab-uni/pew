using Emgu.CV;
using Emgu.CV.Structure;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectHeracles
{
    /// <summary>
    /// Interaction logic for LucasTest.xaml
    /// </summary>
    public partial class LucasTest : Window
    {

        #region Constructors

        public LucasTest()
        {
            InitializeComponent();
            srcImage = new Mat();
        }

        #endregion

        #region Attributes

        Mat srcImage, previewImage;
        List<ColorChannelItem> colorChannelElements = new List<ColorChannelItem>();

        #endregion

        #region Private Methods

        private void CreateRGBTinyImages(Mat src)
        {
            Mat coloredTinyImage = HeraclesHelper.RescaleImage(src, 32);
            Mat[] rgbImages = HeraclesHelper.GetColorChannels(coloredTinyImage);

            colorChannelElements.Clear();

            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "RGB", Image = HeraclesHelper.ToBitmapSource(coloredTinyImage) });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Red", Image = HeraclesHelper.ToBitmapSource(rgbImages[0]) });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Green", Image = HeraclesHelper.ToBitmapSource(rgbImages[1]) });
            colorChannelElements.Add(new ColorChannelItem
            { IsSelected = true, Name = "Blue", Image = HeraclesHelper.ToBitmapSource(rgbImages[2]) });

            icTodoList.ItemsSource = colorChannelElements;
        }

        #region Callback Methods

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Load image to srcImage
                srcImage = CvInvoke.Imread(openFileDialog.FileName, Emgu.CV.CvEnum.ImreadModes.AnyColor);
                previewImage = srcImage;
                CreateRGBTinyImages(srcImage);

                imgSource.Source = HeraclesHelper.ToBitmapSource(srcImage);
            }
        }

        private void BtnShowHistogramWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.Histogram histo = new Windows.Histogram(srcImage);
            histo.ShowDialog();
        }

        private void ImgSource_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //TODO: Create scale system
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            //Just enable this menu items if has an image loaded
            mainMenu_windows_histogram.IsEnabled = !srcImage.IsEmpty;
        }

        private void TglColorElements_Click(object sender, RoutedEventArgs e)
        {
            string tglName = ((ToggleButton)sender).CommandParameter.ToString();

            //TODO:Solve this, actually the RGB element don`t work properly
            //Deal when click specifically on RGB Toggle
            if (tglName.Equals("RGB"))
            {
                if ((bool)((ToggleButton)sender).IsChecked)
                {
                    previewImage = HeraclesHelper.SetImageChannels(srcImage, true, true, true);
                    imgSource.Source = HeraclesHelper.ToBitmapSource(previewImage);
                    return;
                }
                else
                {
                    //if try to pass to unchecked, force to Checked
                    colorChannelElements[0].IsSelected = true;
                    ((ToggleButton)sender).IsChecked = true;
                    return;
                }
            }

            bool[] tglStates = { true, true, true };

            //starts in 1 to ignore rgb element
            for (int i = 1; i < colorChannelElements.Count; i++)
            {
                tglStates[i - 1] = colorChannelElements[i].IsSelected;

                //If some channel is disabled, disable RGB too
                if (!colorChannelElements[i].IsSelected)
                {
                    colorChannelElements[0].IsSelected = false;
                }
            }
            //Set image source with the preview image with selected channels
            previewImage = HeraclesHelper.SetImageChannels(srcImage, tglStates[0], tglStates[1], tglStates[2]);
            imgSource.Source = HeraclesHelper.ToBitmapSource(previewImage);
        }

        #endregion

        #endregion

    }

    public class ColorChannelItem
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public BitmapSource Image { get; set; }
    }
}