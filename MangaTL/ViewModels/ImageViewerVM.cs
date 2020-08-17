using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Core;
using MangaTL.Managers;
using Prism.Commands;
using Prism.Mvvm;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

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

        private string cursor;
        public string Cursor
        {
            get => cursor;
            set => SetProperty(ref cursor, value);
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

        public Point GetRelativePoint(Point point)
        {
            return new Point((point.X - X) / _scale, (point.Y - Y) / _scale);
        }

        public void MoveBubbles(Point delta)
        {
            foreach (var bubbleVm in _bubbleCollection)
            {
                bubbleVm.X += delta.X;
                bubbleVm.Y += delta.Y;
            }
        }

        public BubbleVM CreateBubble(Rectangle rect)
        {
            var bubbleModel = currentPage.CreateBubble(rect);

            var bubble = new BubbleVM(bubbleModel, _scale, new Point(X, Y), this);
            BubbleCollection.Add(bubble);
            return bubble;
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
                bubble.UpdateVisual(_scale, p);
        }

        public void LoadPage(Page page)
        {
            currentPage = page;
            _scale = 1;
            Image = page.TranslateImage.ConvertToBitmapImage();
            Width = Image.Width;
            Height = Image.Height;
            X = Y = 0;
            BubbleCollection.Clear();
            var p = new Point(X, Y);
            foreach (var pageBubble in page.Bubbles)
                BubbleCollection.Add(new BubbleVM(pageBubble, _scale, p, this));
        }

        public (Point, double) GetData()
        {
            return (new Point(X, Y), _scale);
        }

        public BubbleVM GetBubbleFromPoint(Point mousePoint)
        {
            mousePoint = GetRelativePoint(mousePoint);
            return BubbleCollection.FirstOrDefault(bubbleVm => bubbleVm.GetBubble.Rect.Intersect(mousePoint));
        }

        public void RemoveBubble(BubbleVM bubble)
        {
            UndoManager.CountAction(() =>
            {
                currentPage.Bubbles.Add(bubble.GetBubble);
                BubbleCollection.Add(bubble);
            });
            BubbleCollection.Remove(bubble);
            currentPage.Bubbles.Remove(bubble.GetBubble);
        }
    }
}