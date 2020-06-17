using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using MangaTL.Core;
using MangaTL.Managers;

using Prism.Commands;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class ImageViewerVM : BindableBase
    {
        private double _height;


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


        public ImageViewerVM()
        {
            _scale = 1;
            Image = new BitmapImage(new Uri(@"d:\Manga\Clean\Done/SE/20/PNG/01.png"));
            Width = Image.Width;
            Height = Image.Height;
            //MouseWheelCommand = new DelegateCommand<MouseWheelEventArgs>(x => changeScale((float)x.Delta / 5000));
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

        public void ScaleImage(Point mousePosition, double deltaScale)
        {
            _scale += deltaScale;
            _scale = Math.Max(0.1f, Math.Min(_scale, 1000));

            var oldSize = new Size(Width, Height);

            var newWidth = Image.Width * _scale;
            var newHeight = Image.Height * _scale;

            var centerPosition = new Point(X, Y );

            var deltaSize = new Point(newWidth - oldSize.Width, newHeight - oldSize.Height);
            var deltaMove = new Point(-(mousePosition.X - centerPosition.X) / oldSize.Width,
                                      -(mousePosition.Y - centerPosition.Y) / oldSize.Height);
            var deltaPosition = new Point(deltaMove.X * deltaSize.X, deltaMove.Y * deltaSize.Y);
            Width = newWidth;
            Height = newHeight;
            MoveImage(deltaPosition);

            
        }
    }
}