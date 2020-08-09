using System.Drawing;
using MangaTL.Core;
using Prism.Mvvm;
using Point = System.Windows.Point;

namespace MangaTL.ViewModels
{
    public class BubbleVM : BindableBase
    {
        private readonly TextBubble bubbleModel;
        private double _height;

        private string _text;

        private double _width;

        private double _x;
        private double _y;

        private ImageViewerVM vm;

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

        public BubbleVM(TextBubble bubbleModel, double scale, Point offset, ImageViewerVM vm)
        {
            this.bubbleModel = bubbleModel;
            this.vm = vm;
            Text = bubbleModel.TextContent;
            UpdateVisual(scale, offset);
        }

        public void UpdateVisual(double scale, Point offset)
        {
            Width = bubbleModel.Rect.Width * scale;
            Height = bubbleModel.Rect.Height * scale;
            X = offset.X + bubbleModel.Rect.X * scale;
            Y = offset.Y + bubbleModel.Rect.Y * scale;
        }

        public void SetNewRect(Rectangle newRect)
        {
            bubbleModel.Rect = newRect;
            var data = vm.GetData();
            UpdateVisual(data.Item2, data.Item1);
        }
    }
}