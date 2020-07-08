using System.Windows;
using System.Windows.Media;
using MangaTL.Core;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class BubbleVM : BindableBase
    {
        private FontFamily _fontFamily;
        private TextBubble bubbleModel;

        private SolidColorBrush _foregroundColor;
        private double _height;

        private double _size;

        private string _text;

        private TextAlignment _textAlignment;

        private double _width;
        private double _x;
        private double _y;

        public BubbleVM(TextBubble bubbleModel)
        {
            this.bubbleModel = bubbleModel;
        }

        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public FontFamily FontFamily
        {
            get => _fontFamily;
            set => SetProperty(ref _fontFamily, value);
        }

        public TextAlignment Alignment
        {
            get => _textAlignment;
            set => SetProperty(ref _textAlignment, value);
        }

        public double Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public SolidColorBrush ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }

        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }
    }
}