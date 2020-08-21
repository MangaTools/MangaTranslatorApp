using System.Drawing;
using System.Windows.Input;
using MangaTL.Managers;
using Prism.Commands;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class StyleControlVM : BindableBase
    {
        private BubbleVM _bubble;

        private int _height;

        private string _text;

        private int _width;
        private int _x;

        private int _y;

        public int X
        {
            get => _x;
            set
            {
                if (_bubble == null)
                {
                    SetProperty(ref _x, value);
                    return;
                }

                var rect = _bubble.GetBubble.Rect;
                SetProperty(ref _x, value);
                _bubble.SetNewRect(new Rectangle(X, rect.Y, rect.Width, rect.Height));
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (_bubble == null)
                {
                    SetProperty(ref _y, value);
                    return;
                }

                var rect = _bubble.GetBubble.Rect;
                SetProperty(ref _y, value);
                _bubble.SetNewRect(new Rectangle(rect.X, Y, rect.Width, rect.Height));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_bubble == null)
                {
                    SetProperty(ref _text, value);
                    return;
                }

                SetProperty(ref _text, value);
                _bubble.SetNewText(Text);
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (_bubble == null)
                {
                    SetProperty(ref _width, value);
                    return;
                }

                var rect = _bubble.GetBubble.Rect;

                SetProperty(ref _width, value);
                _bubble.SetNewRect(new Rectangle(rect.X, rect.Y, Width, rect.Height));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (_bubble == null)
                {
                    SetProperty(ref _height, value);
                    return;
                }

                var rect = _bubble.GetBubble.Rect;

                SetProperty(ref _height, value);
                _bubble.SetNewRect(new Rectangle(rect.X, rect.Y, rect.Width, Height));
            }
        }

        public ICommand GetFocus { get; }

        public ICommand LostFocus { get; }

        public StyleControlVM()
        {
            GetFocus = new DelegateCommand(KeyManager.StopCatchingKeys);
            LostFocus = new DelegateCommand(KeyManager.ResumeCatchingKeys);
        }

        public void Clear()
        {
            _bubble = null;
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Text = "";
        }

        public void SetBubble(BubbleVM vm)
        {
            _bubble = vm;
            if (_bubble == null)
                return;

            var data = _bubble.GetBubble;
            X = data.Rect.X;
            Y = data.Rect.Y;
            Width = data.Rect.Width;
            Height = data.Rect.Height;
            Text = data.TextContent;
        }
    }
}