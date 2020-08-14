using System.Drawing;
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
                    return;
                SetProperty(ref _x, value);
                var rect = _bubble.GetBubble.Rect;
                _bubble.SetNewRect(new Rectangle(X, rect.Y, rect.Width, rect.Height));
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (_bubble == null)
                    return;
                SetProperty(ref _y, value);
                var rect = _bubble.GetBubble.Rect;
                _bubble.SetNewRect(new Rectangle(rect.X, Y, rect.Width, rect.Height));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_bubble == null)
                    return;
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
                    return;
                SetProperty(ref _width, value);
                var rect = _bubble.GetBubble.Rect;
                _bubble.SetNewRect(new Rectangle(rect.X, rect.Y, Width, rect.Height));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (_bubble == null)
                    return;
                SetProperty(ref _height, value);
                var rect = _bubble.GetBubble.Rect;
                _bubble.SetNewRect(new Rectangle(rect.X, rect.Y, rect.Width, Height));
            }
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