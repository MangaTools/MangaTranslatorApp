using System.Windows;
using System.Windows.Input;

using MangaTL.Managers;

using Prism.Commands;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class MainWindowVM : BindableBase
    {
        private ICommand _keyDownCommand;

        private ICommand _keyUpCommand;
        public ImageViewerVM Image { get; set; }
        public ToolsMenuVM Tools { get; set; }

        public ICommand KeyDownCommand
        {
            get => _keyDownCommand;
            set => SetProperty(ref _keyDownCommand, value);
        }

        public ICommand KeyUpCommand
        {
            get => _keyUpCommand;
            set => SetProperty(ref _keyUpCommand, value);
        }

        public ICommand MouseDownCommand { get; }

        public ICommand MouseUpCommand { get; }

        public object MouseMoveCommand { get; }

        private string _test;
        public string Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        private Window _window;

        public MainWindowVM(Window window)
        {
            _window = window;
            MouseManager.SetMainWindow(_window);
            Image = new ImageViewerVM();
            Tools = new ToolsMenuVM(Image);

            KeyUpCommand = new DelegateCommand<KeyEventArgs>(x => KeyManager.KeyReleased(x.Key));
            KeyDownCommand = new DelegateCommand<KeyEventArgs>(x => KeyManager.KeyPressed(x.Key));
            MouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(MouseManager.SetMousePressed);
            MouseUpCommand = new DelegateCommand<MouseButtonEventArgs>(MouseManager.SetMouseReleased);
            MouseMoveCommand = new DelegateCommand<MouseEventArgs>(MouseManager.MoveMouse);
            MouseManager.MouseMove += x => Test = $"{MouseManager.MousePosition.X} {MouseManager.MousePosition.Y}";
        }

    }
}