using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

using MangaTL.ViewModels.Tools;

using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class ToolsMenuVM : BindableBase
    {
        private ObservableCollection<ToolControlVM> _tools;

        public ObservableCollection<ToolControlVM> Tools
        {
            get => _tools;
            set => SetProperty(ref _tools, value);
        }

        public ToolsMenuVM(ImageViewerVM imageVM)
        {
            _tools = new ObservableCollection<ToolControlVM> { new HandTool(imageVM), new ResizeTool(imageVM) };
        }
    }
}