using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MangaTL.ViewModels;

namespace MangaTL.Controls
{
    /// <summary>
    /// Логика взаимодействия для AboutScreen.xaml
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
