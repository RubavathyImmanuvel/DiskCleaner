using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DiskCleaner
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileItem> Files { get; set; } = new ObservableCollection<FileItem>();

        public MainWindow()
        {
            InitializeComponent();
            FilesDataGrid.ItemsSource = Files;
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            Files.Clear();
            StatusTextBlock.Text = "Scanning...";

            var tempPath = Path.GetTempPath();
            var tempFiles = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in tempFiles)
            {
                var fileInfo = new FileInfo(file);
                Files.Add(new FileItem
                {
                    FilePath = file,
                    Size = fileInfo.Length / 1024,
                    IsSelected = false
                });
            }

            StatusTextBlock.Text = "Scan completed.";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filesToDelete = Files.Where(f => f.IsSelected).ToList();
            foreach (var fileItem in filesToDelete)
            {
                try
                {
                    File.Delete(fileItem.FilePath);
                    Files.Remove(fileItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file {fileItem.FilePath}: {ex.Message}");
                }
            }

            StatusTextBlock.Text = $"{filesToDelete.Count} files deleted.";
        }
    }

    public class FileItem
    {
        public bool IsSelected { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
    }
}
