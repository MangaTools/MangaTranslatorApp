using System.Drawing;
using System.Windows;
using MangaTL.Core;
using MangaTL.Managers;
using Prism.Mvvm;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace MangaTL.ViewModels
{
    public class BubbleVM : BindableBase
    {
        private const double FontSizes = 16;
        private const double BorderSize = 5;

        private readonly ImageViewerVM vm;

        private double _backgroundOpacity;

        private Color _borderColor;

        private double _borderThickness;

        private double _fontSize;

        private double _foregroundOpacity;
        private double _height;

        private string _text;

        private double _width;

        private double _x;
        private double _y;

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


        public Color BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        public double BorderThickness
        {
            get => _borderThickness;
            set => SetProperty(ref _borderThickness, value);
        }

        public double BackgroundOpacity
        {
            get => _backgroundOpacity;
            set => SetProperty(ref _backgroundOpacity, value);
        }

        public double ForegroundOpacity
        {
            get => _foregroundOpacity;
            set => SetProperty(ref _foregroundOpacity, value);
        }

        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        public TextBubble GetBubble { get; }

        public BubbleVM(TextBubble bubbleModel, double scale, Point offset, ImageViewerVM vm)
        {
            GetBubble = bubbleModel;
            this.vm = vm;
            BorderThickness = BorderSize;
            ForegroundOpacity = 0;
            BackgroundOpacity = 0;
            FontSize = FontSizes;
            BorderColor = (Color) Application.Current.Resources["AlternativeBrightColor"];

            Text = bubbleModel.TextContent;
            UpdateVisual(scale, offset);
        }

        public void Select()
        {
            BorderColor = (Color) Application.Current.Resources["DarkBackgroundColor"];
            ForegroundOpacity = 1;
            BackgroundOpacity = 0.75;
        }

        public void Deselect()
        {
            BorderColor = (Color) Application.Current.Resources["AlternativeBrightColor"];
            ForegroundOpacity = 0;
            BackgroundOpacity = 0;
        }

        public void UpdateVisual(double scale, Point offset)
        {
            FontSize = FontSizes * scale;
            BorderThickness = BorderSize * scale;
            Width = GetBubble.Rect.Width * scale;
            Height = GetBubble.Rect.Height * scale;
            X = offset.X + GetBubble.Rect.X * scale;
            Y = offset.Y + GetBubble.Rect.Y * scale;
        }

        private void UpdateVisual()
        {
            var (offset, scale) = vm.GetData();
            UpdateVisual(scale, offset);
        }

        public void SetNewRect(Rectangle newRect)
        {
            var oldRect = GetBubble.Rect;
            if (newRect == oldRect)
                return;
            UndoManager.CountAction(() =>
            {
                SetNewRect(oldRect);
                UndoManager.RemoveLast(1);
            });
            GetBubble.Rect = newRect;
            UpdateVisual();
        }

        public void SetVisibleText(string newText)
        {
            Text = newText;
        }

        public void SetNewText(string newText)
        {
            var oldText = GetBubble.TextContent;
            if (oldText == newText)
                return;
            UndoManager.CountAction(() =>
            {
                SetNewText(oldText);
                UndoManager.RemoveLast(1);
            });
            GetBubble.TextContent = newText;
            Text = newText;
        }
    }
}