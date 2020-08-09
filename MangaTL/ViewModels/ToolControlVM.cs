using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Core;
using MangaTL.Managers;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public abstract class ToolControlVM : BindableBase
    {
        private BitmapImage _imageSource;
        private bool _isPressed;

        private ICommand _pressCommand;

        private string _toolTip;

        public ICommand PressCommand
        {
            get => _pressCommand;
            set => SetProperty(ref _pressCommand, value);
        }

        public string ToolTip
        {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value);
        }

        public bool Pressed
        {
            get => _isPressed;
            set => SetProperty(ref _isPressed, value);
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        protected bool InAction { get; set; }

        protected ToolControlVM(List<Key> hotkey, List<Key> fastKeys)
        {
            ToolManager.AddTool(this, hotkey, fastKeys);

            MouseManager.MousePressed += args =>
            {
                if (!Pressed || args.ChangedButton != MouseButton.Left)
                    return;
                if (MouseManager.CurrentMouseOver == ControlType.ImageViewer)
                    DoAction();
            };

            MouseManager.MouseReleased += args =>
            {
                if (!InAction || args.ChangedButton != MouseButton.Left)
                    return;
                if (args.ButtonState == MouseButtonState.Released)
                    StopAction();
            };
        }

        protected virtual void DoAction()
        {
            InAction = true;
        }

        protected virtual void StopAction()
        {
            InAction = false;
        }
    }
}