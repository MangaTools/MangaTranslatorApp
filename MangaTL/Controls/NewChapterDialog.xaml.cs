using System.Windows;
using Microsoft.Win32;

namespace MangaTL.Controls
{
    /// <summary>
    ///     Логика взаимодействия для NewChapterDialog.xaml
    /// </summary>
    public partial class NewChapterDialog : Window
    {
        public string[] cleanedPath;
        public string[] tlPath;

        public NewChapterDialog()
        {
            InitializeComponent();
            CreateBT.IsEnabled = false;
        }

        private void ChooseCleanedFiles(object sender, RoutedEventArgs e)
        {
            var images = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                Title = "Select Cleaned images"
            };
            if (images.ShowDialog() != true)
                return;

            cleanedPath = images.FileNames;
            if (cleanedPath.Length > 0 && tlPath != null && tlPath.Length == cleanedPath.Length)
                CreateBT.IsEnabled = true;
            else
                CreateBT.IsEnabled = false;
        }

        private void ChooseTLFiles(object sender, RoutedEventArgs e)
        {
            var images = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                Title = "Select Original images"
            };
            if (images.ShowDialog() != true)
                return;

            tlPath = images.FileNames;
            if (tlPath.Length > 0 && cleanedPath != null && tlPath.Length == cleanedPath.Length)
                CreateBT.IsEnabled = true;
            else
                CreateBT.IsEnabled = false;
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}