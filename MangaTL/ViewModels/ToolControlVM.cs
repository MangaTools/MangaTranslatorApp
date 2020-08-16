using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Core;
using MangaTL.Managers;
using Prism.Commands;
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
            set
            {
                if (value == _isPressed)
                    return;
                SetProperty(ref _isPressed, value);

                if (value)
                    Activated();
                else
                    Deactivated();
            }
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        protected bool InAction { get; set; }

        public ICommand PressingCommand
        {
            get;
        }

        protected ToolControlVM(List<Key> hotkey, List<Key> fastKeys)
        {
            PressingCommand = new DelegateCommand(() =>
            {
                ToolManager.SetTool(this);
            });
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

        protected virtual void Activated()
        {
        }

        protected virtual void Deactivated()
        {
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