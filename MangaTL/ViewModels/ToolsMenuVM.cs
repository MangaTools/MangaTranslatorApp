using System.Collections.ObjectModel;
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

        public ToolsMenuVM(ImageViewerVM imageVM, StyleControlVM styeVM)
        {
            _tools = new ObservableCollection<ToolControlVM>
                {new HandTool(imageVM), new ResizeTool(imageVM), new TextTool(imageVM), new PointTool(imageVM, styeVM)};
        }
    }
}