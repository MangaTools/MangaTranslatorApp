using System.ComponentModel;
using System.Net;
using System.Windows;
using MangaTL.Managers;
using MangaTL.ViewModels;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using NetSparkleUpdater.UI.WPF;

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

        public MainWindow(string path)
        {
            InitializeComponent();
            MouseManager.SetMainWindow(PageViewer);
            ToolManager.Start();
            var vm = new MainWindowVM(GeneralWindow);
            DataContext = vm;
            vm.OpenFile(path);
        }

        private void StartSparkle()
        {
            var _sparkle =
                new SparkleUpdater("", new DSAChecker(SecurityMode.Strict))
                {
                    UIFactory = new UIFactory(),
                    ShowsUIOnMainThread = false,
                    SecurityProtocolType = SecurityProtocolType.Tls12
                };
            _sparkle.StartLoop(true, true);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ((MainWindowVM) DataContext).OnExitWindow(e);
        }
    }
}