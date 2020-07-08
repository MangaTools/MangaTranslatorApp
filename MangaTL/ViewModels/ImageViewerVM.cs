using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using MangaTL.Core;
using MangaTL.Managers;

using Prism.Commands;
using Prism.Mvvm;

using FontFamily = System.Windows.Media.FontFamily;
using Page = MangaTL.Core.Page;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace MangaTL.ViewModels
{
    public class ImageViewerVM : BindableBase
    {
        private double _height;
        private Page currentPage;

        private BitmapImage _image;

        private double _width;
        private double _scale;

        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private double _x;
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
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

        public ICommand MouseWheelCommand { get; set; }

        public ICommand MouseEnterCommand { get; set; }

        public ICommand MouseLeaveCommand { get; set; }

        private ObservableCollection<BubbleVM> _bubbleCollection = new ObservableCollection<BubbleVM>();
        public ObservableCollection<BubbleVM> BubbleCollection
        {
            get => _bubbleCollection;
            set => SetProperty(ref _bubbleCollection, value);
        }

        public ImageViewerVM()
        {
            _scale = 1;
            Image = new BitmapImage(new Uri(@"d:\Manga\Clean\Done/SE/20/PNG/01.png"));
            Width = Image.Width;
            Height = Image.Height;
            MouseEnterCommand =
                new DelegateCommand<MouseEventArgs>(x => MouseManager.CurrentMouseOver = ControlType.ImageViewer);
            MouseLeaveCommand = new DelegateCommand<MouseEventArgs>(x =>
            {
                if (MouseManager.CurrentMouseOver == ControlType.ImageViewer)
                    MouseManager.CurrentMouseOver = ControlType.Null;
            });
        }

        public void MoveImage(Point delta)
        {
            X += delta.X;
            Y += delta.Y;
        }

        public void MoveBubbles(Point delta)
        {
            foreach (var bubbleVm in _bubbleCollection)
            {
                bubbleVm.X += delta.X;
                bubbleVm.Y += delta.Y;
            }
        }

        private System.Drawing.Point GetImagePoint()
        {
            var pos = MouseManager.MousePosition;
            pos.Y -= 26;
            var imagePos = new Point(X, Y);
            var newPos = (pos - imagePos) / _scale;
            return new System.Drawing.Point((int)Math.Floor(newPos.X), (int)Math.Floor(newPos.Y));
        }

        public void AddBubble()
        {
            var bubbleModel = currentPage.CreateBubble(TextStyle.StandartStyle, GetImagePoint(), 32);

            var bubble = new BubbleVM(bubbleModel);
            BubbleCollection.Add(bubble);
        }

        public void ScaleRegion(Point mousePosition, double deltaScale)
        {
            _scale += _scale * deltaScale;
            _scale = Math.Max(0.1f, Math.Min(_scale, 1000));

            ScaleImage(mousePosition);
            ScaleBubbles(mousePosition);
        }

        private void ScaleImage(Point mousePosition)
        {
            var oldSize = new Size(Width, Height);

            var newWidth = Image.Width * _scale;
            var newHeight = Image.Height * _scale;

            var centerPosition = new Point(X, Y);

            var deltaSize = new Point(newWidth - oldSize.Width, newHeight - oldSize.Height);
            var deltaMove = new Point(-(mousePosition.X - centerPosition.X) / oldSize.Width,
                                      -(mousePosition.Y - centerPosition.Y) / oldSize.Height);
            var deltaPosition = new Point(deltaMove.X * deltaSize.X, deltaMove.Y * deltaSize.Y);
            Width = newWidth;
            Height = newHeight;
            MoveImage(deltaPosition);
        }

        public void ScaleBubbles(Point mousePosition)
        {
            foreach (var bubble in _bubbleCollection)
            {
                var newTextSize = 10 * (_scale);

                var oldSize = new Size(bubble.Width, bubble.Height);
                var newSize = new Size(25 * _scale, 10 * _scale);

                var centerPosition = new Point(bubble.X, bubble.Y);

                var deltaSize = new Point(newSize.Width - oldSize.Width, newSize.Height - oldSize.Height);
                var deltaMove = new Point(-(mousePosition.X - centerPosition.X) / oldSize.Width,
                                          -(mousePosition.Y - centerPosition.Y) / oldSize.Height);
                var deltaPosition = new Point(deltaMove.X * deltaSize.X, deltaMove.Y * deltaSize.Y);

                bubble.Width = newSize.Width;
                bubble.Height = newSize.Height;
                bubble.X += deltaPosition.X;
                bubble.Y += deltaPosition.Y;
                bubble.Size = newTextSize;
            }
        }

        public void LoadPage(Page page)
        {

        }
    }
}