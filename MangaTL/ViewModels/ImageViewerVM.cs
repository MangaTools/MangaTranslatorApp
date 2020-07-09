using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using MangaTL.Core;
using MangaTL.Managers;

using Prism.Commands;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class ImageViewerVM : BindableBase
    {
        private ObservableCollection<BubbleVM> _bubbleCollection = new ObservableCollection<BubbleVM>();
        private double _height;

        private BitmapImage _image;
        private double _scale;

        private double _width;

        private double _x;

        private double _y;
        private Page currentPage;

        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
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

        public ObservableCollection<BubbleVM> BubbleCollection
        {
            get => _bubbleCollection;
            set => SetProperty(ref _bubbleCollection, value);
        }

        public ImageViewerVM()
        {
            _scale = 1;
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

        public async void AddBubble()
        {
            var bubbleModel = await currentPage.CreateBubble(TextStyle.StandartStyle, GetImagePoint(), 32);

            var bubble = new BubbleVM(bubbleModel, _scale, new Point(X, Y));
            BubbleCollection.Add(bubble);
        }

        public void ScaleRegion(Point mousePosition, double deltaScale)
        {
            _scale += _scale * deltaScale;
            _scale = Math.Max(0.1f, Math.Min(_scale, 1000));

            ScaleImage(mousePosition);
            ScaleBubbles();
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

        private void ScaleBubbles()
        {
            var p = new Point(X, Y);
            foreach (var bubble in _bubbleCollection)
            {
                bubble.UpdateVisual(_scale, p);
            }
        }

        public void LoadPage(Page page)
        {
            currentPage = page;
            _scale = 1;
            Image = page.CleanedImage.ConvertToBitmapImage();
            Width = Image.Width;
            Height = Image.Height;
            X = Y = 0;
            BubbleCollection.Clear();
            var p = new Point(X, Y);
            foreach (var pageBubble in page.Bubbles)
                BubbleCollection.Add(new BubbleVM(pageBubble, _scale, p));
        }
    }
}