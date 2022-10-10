using ImageChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace ImageChecker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            image.MouseDown += MouseDownOnImage;
            image.MouseMove += MouseMoveOnImage;
        }


        private void MouseDownOnImage(object sender, MouseEventArgs e)
        {

            Point point = e.GetPosition(image);
            double x = point.X;
            double y = point.Y;
            BitmapSource b = (BitmapSource)image.Source;

            Color mediacolor = GetPixelColor((int)(x / image.ActualWidth * b.PixelWidth), (int)(y / image.ActualHeight * b.PixelHeight), b);
            var hsv = HSVFeature.RGBtoHSV(System.Drawing.Color.FromArgb(
    mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B));
            clickhsv.Text = hsv.ToString();
            clickcolor.Background = new SolidColorBrush(mediacolor);

        }
        private void MouseMoveOnImage(object sender, MouseEventArgs e)
        {

            Point point = e.GetPosition(image);
            double x = point.X;
            double y = point.Y;
            BitmapSource b = (BitmapSource)image.Source;

            Color mediacolor = GetPixelColor((int)(x / image.ActualWidth * b.PixelWidth), (int)(y / image.ActualHeight * b.PixelHeight), b);
            var hsv = HSVFeature.RGBtoHSV(System.Drawing.Color.FromArgb(
     mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B));
            currenthsv.Text = hsv.ToString();
            currentcolor.Background = new SolidColorBrush(mediacolor);

        }

        private Color GetPixelColor(int x, int y, BitmapSource bmp)
        {
            var cb = new CroppedBitmap(bmp, new Int32Rect(x, y, 1, 1));
            var fcb = new FormatConvertedBitmap(cb, PixelFormats.Bgra32, null, 0);
            byte[] pixels = new byte[4];
            fcb.CopyPixels(pixels, 4, 0);
            var c = Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            return c;
        }
    }
}
