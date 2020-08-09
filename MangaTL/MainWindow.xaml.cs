using System.Windows;
using MangaTL.Managers;
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
            MouseManager.SetMainWindow(PageViewer);
            ToolManager.Start();
            DataContext = new MainWindowVM(GeneralWindow);
        }
    }
}