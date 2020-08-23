using System.Reflection;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class AboutScreenVM : BindableBase
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}