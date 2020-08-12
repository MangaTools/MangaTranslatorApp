using System.Linq;
using System.Windows;

namespace MangaTL
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            var fileName = e.Args.FirstOrDefault();
            var mainWindow = !string.IsNullOrWhiteSpace(fileName) ? new MainWindow(fileName) : new MainWindow();

            mainWindow.Show();
        }
    }
}