using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Textract.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MenuItem_Click_Load(object sender, RoutedEventArgs e)
        {
            CurrentStatus.Text = "Processing Image";

            // Configure open file dialog box
            var dialog = new OpenFileDialog();

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                // Load image
                using var file = File.OpenRead(filename);
                using var memory = new MemoryStream();
                await file.CopyToAsync(memory);

                // Load bitmap
                // var imageSourceConverter = new ImageSourceConverter();
                // Image.Source = (BitmapSource?)imageSourceConverter.ConvertFrom(memory.ToArray());

                ExtractTextFromBytesAndUpdateWindow(memory.ToArray());
            }
        }

        private void ExtractTextFromBytesAndUpdateWindow(byte[] memory)
        {
            // Extract text
            var text = TextExtractor.ExtractFromBytes(memory);
            ProcessedTextbox.Text = text;
            ProcessedTextbox.CaretIndex = text.Length;

            // Update status
            CurrentStatus.Text = "Image Processed";
            StatusTextLength.Text = $"{text.Length} Characters";
            StatusWordCount.Text = $"{text.Split(' ').Length} Words";
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Clipboard.ContainsImage())
                {
                    var bitmap = Clipboard.GetImage();

                    using var memory = new MemoryStream();

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(memory);
                    memory.Seek(0, SeekOrigin.Begin);

                    ExtractTextFromBytesAndUpdateWindow(memory.ToArray());
                }
            }
        }
    }
}
