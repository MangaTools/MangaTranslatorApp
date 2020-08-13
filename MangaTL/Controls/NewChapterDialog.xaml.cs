using System.Windows;
using Microsoft.Win32;

namespace MangaTL.Controls
{
    /// <summary>
    ///     Логика взаимодействия для NewChapterDialog.xaml
    /// </summary>
    public partial class NewChapterDialog : Window
    {
        public string[] tlPath;

        public NewChapterDialog()
        {
            InitializeComponent();
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
            CreateBT.IsEnabled = tlPath.Length != 0;
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}