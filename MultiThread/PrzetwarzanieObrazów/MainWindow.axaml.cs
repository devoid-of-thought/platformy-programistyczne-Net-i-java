using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using Avalonia.Threading;
using System.Threading;
using OpenCvSharp;
using Window = Avalonia.Controls.Window;

namespace PrzetwarzanieObrazów;

public partial class MainWindow : Window
{
    private string _sourceImagePath;
    private Bitmap _sourceBitmap;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnLoadImageClick(object sender, RoutedEventArgs e)
    {
        

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Wybierz obraz",
            AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
        });

        if (files.Count == 1)
        {
            _sourceImagePath = files[0].Path.LocalPath;
            
            using var stream = File.OpenRead(_sourceImagePath);
            _sourceBitmap = new Bitmap(stream);
            
            Original.Source = _sourceBitmap;
            ProcessImage.IsEnabled = true;

            Thresholded.Source = null;
            Negative.Source = null;
            Green.Source = null;
            GrayScale.Source = null;
        }
    }


    private void BtnProcessImageClick(object sender, RoutedEventArgs e)
    {
        Thread tresh = new Thread(() =>
        {
            Mat src = Cv2.ImRead(_sourceImagePath);
            Mat gray = src.CvtColor(ColorConversionCodes.BGR2GRAY);;
            Mat result = new Mat();
            Cv2.Threshold(gray, result, 0, 255, ThresholdTypes.Otsu);
            Cv2.ImEncode(".bmp", result, out byte[] buffer);
            
            Dispatcher.UIThread.Post(() =>
            {
                using var ms = new MemoryStream(buffer);
                Thresholded.Source = new Bitmap(ms);
            });
        });

        Thread negative = new Thread(() =>
        {
            Mat src = Cv2.ImRead(_sourceImagePath);
            Mat result = new Mat();
            Cv2.BitwiseNot(src, result);
            Cv2.ImEncode(".bmp", result, out byte[] buffer);
            
            Dispatcher.UIThread.Post(() =>
            {
                using var ms = new MemoryStream(buffer);
                Negative.Source = new Bitmap(ms);
            });
        });

        Thread green = new Thread(() =>
        {
            Mat src = Cv2.ImRead(_sourceImagePath);
            Mat result = new Mat();
            Mat[] color_channel = Cv2.Split(src);
            color_channel[0].SetTo(0);
            color_channel[2].SetTo(0);
            Cv2.Merge(color_channel,result);
            Cv2.ImEncode(".bmp", result, out byte[] buffer);
            Dispatcher.UIThread.Post(() =>
            {
                using var ms = new MemoryStream(buffer);
                Green.Source = new Bitmap(ms);
            });
        });

        Thread gray = new Thread(() =>
        {
            Mat src = Cv2.ImRead(_sourceImagePath);
            Mat gray = src.CvtColor(ColorConversionCodes.BGR2GRAY);;
            Cv2.ImEncode(".bmp", gray, out byte[] buffer);
            Dispatcher.UIThread.Post(() =>
            {
                using var ms = new MemoryStream(buffer);
                GrayScale.Source = new Bitmap(ms);
            });
        });

        tresh.Start();
        negative.Start();
        green.Start();
        gray.Start();
    }
    
}