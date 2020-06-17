using System.Windows;
using MangaTL.ViewModels;

namespace MangaTL
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM(GeneralWindow);
        }
    }
}