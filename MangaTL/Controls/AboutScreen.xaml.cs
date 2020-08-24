using System.Windows;
using MangaTL.ViewModels;

namespace MangaTL.Controls
{
    /// <summary>
    ///     Логика взаимодействия для AboutScreen.xaml
    /// </summary>
    public partial class AboutScreen : Window
    {
        public AboutScreen()
        {
            InitializeComponent();
            DataContext = new AboutScreenVM();
        }
    }
}