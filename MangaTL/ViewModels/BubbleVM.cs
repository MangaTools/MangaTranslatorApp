using System.Windows;
using System.Windows.Media;

using MangaTL.Core;

using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class BubbleVM : BindableBase
    {
        private FontFamily _fontFamily;

        private SolidColorBrush _foregroundColor;
        private double _height;

        private double _size;

        private string _text;

        private TextAlignment _textAlignment;

        private double _width;
        private double _x;
        private double _y;
        private TextBubble bubbleModel;

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

        public BubbleVM(TextBubble bubbleModel, double scale, Point offset)
        {
            this.bubbleModel = bubbleModel;
            FontFamily = new FontFamily(bubbleModel.Style.FontName);
            ForegroundColor = new SolidColorBrush(Color.FromRgb(bubbleModel.Style.Color.R,
                                                                bubbleModel.Style.Color.G,
                                                                bubbleModel.Style.Color.B));
            Text = bubbleModel.TextContent;
            Alignment = TextAlignment.Center;
            UpdateVisual(scale, offset);
        }

        public void UpdateVisual(double scale, Point offset)
        {
            Size = bubbleModel.Style.FontSize / 0.75d * scale;
            Width = bubbleModel.Shape.Size.Width * scale;
            Height = bubbleModel.Shape.Size.Height * scale;
            X = offset.X + bubbleModel.Position.X * scale;
            Y = offset.Y + bubbleModel.Position.Y * scale;
        }
    }
}