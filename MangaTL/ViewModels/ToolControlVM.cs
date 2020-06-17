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

        private readonly HashSet<Key> activeKeys = new HashSet<Key>();
        protected HashSet<Key> FastKeys = new HashSet<Key>();

        protected Key HotKey;

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

        public bool InAction { get; protected set; }

        protected ToolControlVM(Key hotKey, IEnumerable<Key> fastKeys)
        {
            HotKey = hotKey;
            foreach (var fastKey in fastKeys)
                FastKeys.Add(fastKey);

            MouseManager.MousePressed += args =>
            {
                if (!Pressed || args.ChangedButton != MouseButton.Left)
                    return;
                if (MouseManager.CurrentMouseOver == ControlType.ImageViewer)
                    DoAction();
            };

            MouseManager.MouseReleased += args =>
            {
                if (!Pressed || args.ChangedButton != MouseButton.Left)
                    return;

                if (args.ButtonState == MouseButtonState.Released)
                    StopAction();
            };

            PressCommand = new DelegateCommand(() =>
            {
                if (!Pressed)
                    ToolManager.CurrentTool = this;
            });

            KeyManager.KeyDown += KeyPressed;
            KeyManager.KeyUp += KeyReleased;
        }

        public abstract void DoAction();
        public abstract void StopAction();


        private void KeyPressed(Key key)
        {
            if (HotKey == key && !Pressed)
                ToolManager.CurrentTool = this;

            if (FastKeys == null || !FastKeys.Contains(key))
                return;

            activeKeys.Add(key);
            if (FastKeys.Count != 0 && activeKeys.Count == FastKeys.Count)
                ToolManager.FastTool = this;
        }

        private void KeyReleased(Key key)
        {
            if (FastKeys == null || !activeKeys.Contains(key))
                return;

            activeKeys.Remove(key);
            if (ToolManager.FastTool == this)
                ToolManager.FastTool = null;
        }
    }
}