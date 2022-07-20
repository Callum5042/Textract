using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var dialog = new OpenFileDialog
            {
                // FileName = "Image", // Default file name
                // DefaultExt = ".txt", // Default file extension
                // Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };

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

                // Extract text
                var text = TextExtractor.ExtractFromBytes(memory.ToArray());
                ProcessedTextbox.Text = text;

                // Update status
                CurrentStatus.Text = "Image Processed";
                StatusTextLength.Text = $"{text.Length} Characters";
                StatusWordCount.Text = $"{text.Split(' ').Length} Words";
            }
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
