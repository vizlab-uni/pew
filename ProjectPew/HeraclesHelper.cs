using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

public static class HeraclesHelper
{
    [DllImport("gdi32")]
    private static extern int DeleteObject(IntPtr o);

    #region Image Stuff

    public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        finally { DeleteObject(handle); }
    }

    public static BitmapSource ToBitmapSource(IImage image)
    {
        using (System.Drawing.Bitmap source = image.Bitmap)
        {
            IntPtr ptr = source.GetHbitmap();

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr);
            return bs;
        }
    }

    public static Mat RescaleImage(Mat image, float xFactor, float yFactor)
    {
        Mat tinyImage = new Mat();
        System.Drawing.Size s = new System.Drawing.Size();
        CvInvoke.Resize(image, tinyImage, s, xFactor, yFactor, Inter.Cubic);
        return tinyImage;
    }

    public static Mat RescaleImage(Mat image, int width = 0, int height = 0, Inter interpolation = Inter.Area)
    {
        if (image == null)
            return null;

        Mat tinyImage = new Mat();

        System.Drawing.Size dim = new System.Drawing.Size();
        float ratio;
        int w = image.Width;
        int h = image.Height;

        //If width and height equals zero ,return original image
        if (width <= 0 && height <= 0)
        {
            return image;
        }

        if (width <= 0)
        {
            ratio = height / (float)h;
            dim.Width = (int)(w * ratio);
            dim.Height = height;
        }
        else
        {
            ratio = width / (float)h;

            dim.Width = width;
            dim.Height = (int)(h * ratio);
        }

        CvInvoke.Resize(image, tinyImage, dim, interpolation: interpolation);
        return tinyImage;
    }

    public static Mat GetGrayscaleImage(Mat image)
    {
        if (image == null)
            return null;

        Mat grayImage = new Mat();
        if (image.NumberOfChannels == 3)
        {
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Rgb2Gray);
        }
        else if (image.NumberOfChannels == 4)
        {
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Rgba2Gray);
        }
        else
        {
            MessageBox.Show("Trying to convert image to gray scale but the image has unknown colors channel");
            return null;
        }
        return grayImage;
    }

    public static Mat ConvertToHSV(Mat image)
    {
        if (image == null)
            return null;

        Mat hsvImage = new Mat();
        if (image.NumberOfChannels == 3)
        {
            CvInvoke.CvtColor(image, hsvImage, ColorConversion.Rgb2Hsv);
        }
        else if (image.NumberOfChannels == 4)
        {
            //Convert from rgba to rgb
            CvInvoke.CvtColor(image, hsvImage, ColorConversion.Rgba2Rgb);
            //Convert from rgb to hsv
            CvInvoke.CvtColor(hsvImage, hsvImage, ColorConversion.Rgb2Hsv);
        }
        else
        {
            MessageBox.Show("Trying to convert image to HSV scale but the image has unknown colors channel");
            return null;
        }
        return hsvImage;
    }

    public static Mat ConvertToRGB(Mat image)
    {
        if (image == null)
            return null;

        Mat rgbImage = new Mat();
        if (image.NumberOfChannels == 1)
        {
            CvInvoke.CvtColor(image, rgbImage, ColorConversion.Gray2Rgb);
        }
        else if (image.NumberOfChannels == 3)
        {
            return rgbImage;
        }
        else if (image.NumberOfChannels == 4)
        {
            CvInvoke.CvtColor(image, rgbImage, ColorConversion.Rgba2Rgb);
        }
        else
        {
            MessageBox.Show("Trying to convert image to RGB scale but the image has unknown colors channel");
            return null;
        }
        return rgbImage;
    }

    public static Mat ConvertToRGBA(Mat image)
    {
        if (image == null)
            return null;

        Mat rgbaImage = new Mat();
        if (image.NumberOfChannels == 1)
        {
            CvInvoke.CvtColor(image, rgbaImage, ColorConversion.Gray2Rgba);
        }
        else if (image.NumberOfChannels == 4)
        {
            CvInvoke.CvtColor(image, rgbaImage, ColorConversion.Rgba2Rgb);
        }
        else if (image.NumberOfChannels == 4)
        {
            return rgbaImage;
        }
        else
        {
            MessageBox.Show("Trying to convert image to RGBA scale but the image has unknown colors channel");
            return null;
        }
        return rgbaImage;
    }

    public static Mat[] GetColorChannels(Mat src)
    {
        if (src == null)
            return null;

        //Create an empty channel Image with the size of the src mat
        Image<Gray, byte> emptyChannel = new Image<Gray, byte>(src.Width, src.Height);
        //Convert Mat to Image
        Image<Bgr, byte> img = src.ToImage<Bgr, byte>();

        Image<Bgr, byte> redImg = new Image<Bgr, byte>(new Image<Gray, byte>[]
        { emptyChannel, emptyChannel, img[2] });
        Image<Bgr, byte> greenImg = new Image<Bgr, byte>(new Image<Gray, byte>[]
        { emptyChannel, img[1], emptyChannel });
        Image<Bgr, byte> blueImg = new Image<Bgr, byte>(new Image<Gray, byte>[]
        { img[0], emptyChannel, emptyChannel });

        Mat[] matChannels = { redImg.Mat, greenImg.Mat, blueImg.Mat };
        return matChannels;
    }

    public static Mat SetImageChannels(Mat src, bool showR, bool showG, bool showB)
    {
        if (src == null)
            return null;

        //Create an empty channel Image with the size of the src mat
        Image<Gray, byte> emptyChannel = new Image<Gray, byte>(src.Width, src.Height);
        //Convert Mat to Image
        Image<Bgr, byte> img = src.ToImage<Bgr, byte>();

        //Set channels to empty if bool vars are false
        img[0] = showB ? img[0] : emptyChannel;
        img[1] = showG ? img[1] : emptyChannel;
        img[2] = showR ? img[2] : emptyChannel;

        return img.Mat;
    }

    public static Array GetHistogramOfImage(Mat image, char channel = 'b', int size = 256, float range = 256)
    {
        if (image == null)
            return null;

        Mat hist = new Mat();
        using (Emgu.CV.Util.VectorOfMat vm = new Emgu.CV.Util.VectorOfMat())
        {
            int[] histoChannel = { 0 };
            if (channel == 'b')
                histoChannel = new int[] { 0 };
            if (channel == 'g')
                histoChannel = new int[] { 1 };
            if (channel == 'r')
                histoChannel = new int[] { 2 };

            int[] histoSize = { size };
            float[] histoRange = { 0.0f, range };


            vm.Push(image);
            CvInvoke.CalcHist(vm, histoChannel, null, hist, histoSize, histoRange, false);

            //CvInvoke.Normalize(hist, hist, 0, image.Rows, NormType.MinMax);

        }

        return hist.GetData().Cast<float>().ToArray();
    }

    public static Array GetCDFOfHistogram(Array hist, int numberOfPixels)
    {
        float[] cdf = new float[256];

        float acm = 0;
        for (int i = 0; i < cdf.Length; i++)
        {
            acm += (float)hist.GetValue(i);
            cdf[i] = acm;
        }
        //Calcula o percentual após a computação dos valores acumulados para evitar
        //arredondamentos
        cdf = cdf.Select(x => x / numberOfPixels).ToArray<float>();
        return cdf;
    }

    //TODO: Ta meio problematico o save bmp e jpeg
    public static void SaveUsingEncoder(FrameworkElement visual, string fileName)
    {
        RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        Size visualSize = new Size(visual.ActualWidth, visual.ActualHeight);
        visual.Measure(visualSize);
        visual.Arrange(new Rect(visualSize));
        bitmap.Render(visual);
        BitmapFrame frame = BitmapFrame.Create(bitmap);

        SaveFileDialog dlg = new SaveFileDialog();
        dlg.FileName = fileName;
        //dlg.Filter = "JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
        dlg.Filter = "PNG Image|*.png";
        if (dlg.ShowDialog() == true)
        {
            var pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(frame);
                    using (var stream = File.Create(dlg.FileName))
                    {
                        pngEncoder.Save(stream);
                    }
            //switch (dlg.FilterIndex)
            //{
            //    case 1:
            //        var jpegEncoder = new JpegBitmapEncoder();
            //        jpegEncoder.Frames.Add(frame);
            //        using (var stream = File.Create(dlg.FileName))
            //        {
            //            jpegEncoder.Save(stream);
            //        }
            //        break;
            //    case 2:
            //        var pngEncoder = new PngBitmapEncoder();
            //        pngEncoder.Frames.Add(frame);
            //        using (var stream = File.Create(dlg.FileName))
            //        {
            //            pngEncoder.Save(stream);
            //        }
            //        break;
            //    case 3:
            //        var bmpEncoder = new BmpBitmapEncoder();
            //        bmpEncoder.Frames.Add(frame);
            //        using (var stream = File.Create(dlg.FileName))
            //        {
            //            bmpEncoder.Save(stream);
            //        }
            //        break;
            //}
        }
    }

    #endregion

    #region Process Stuff

    public static void RunPromptCommand(string args, bool cmdStayOpened = false, Action<string> redirectOutput = null, Action<string> redirectError = null)
    {
        //flag to define if closes prompt after execute command
        args = cmdStayOpened ? ("/K " + args) : ("/c " + args);

        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    //WorkingDirectory = "",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            if (redirectOutput != null)
            {
                process.OutputDataReceived += (s, data) => redirectOutput(data.Data);
            }
            if (redirectError != null)
            {
                process.ErrorDataReceived += (sErr, errData) => redirectError(errData.Data);
            }

            process.Start();
            if (redirectOutput != null)
            {
                process.BeginOutputReadLine();
            }
            if (redirectError != null)
            {
                process.BeginErrorReadLine();
            }
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
    }

    public static void RunPythonScript(string scriptPath, string args, Action<string> redirectOutput = null, Action<string> redirectError = null, Action redirectExit = null)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ProjectHeracles.Properties.Settings.Default.PythonPath,
                    //WorkingDirectory = "",
                    Arguments = "-u " + scriptPath + " " + args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            //Exited callback is called when this propertie is true
            process.EnableRaisingEvents = true;

            if (redirectOutput != null)
            {
                process.OutputDataReceived += (s, data) => redirectOutput(data.Data);
            }
            if (redirectError != null)
            {
                process.ErrorDataReceived += (sErr, errData) => redirectError(errData.Data);
            }
            if (redirectExit != null)
            {
                process.Exited += (sExit, data) => redirectExit();
            }

            process.Start();
            if (redirectOutput != null)
            {
                process.BeginOutputReadLine();
            }
            if (redirectError != null)
            {
                process.BeginErrorReadLine();
            }

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
    }

    #endregion
}