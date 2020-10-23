using System.IO;
using System.Reflection;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class AboutScreenVM : BindableBase
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string BuildDate => Assembly.GetExecutingAssembly().GetLinkerTime().ToString("dd MMMM yyyy HH:mm");

    }
}