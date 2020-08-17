using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class HandTool : ToolControlVM
    {
        private readonly ImageViewerVM image;

        public HandTool(ImageViewerVM viewer) : base(new List<Key> {Key.H}, new List<Key> {Key.Space})
        {
            image = viewer;
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/HandIcon.png"));
        }

        protected override void DoAction()
        {
            base.DoAction();
            MouseManager.MouseMove += MoveImage;
        }

        protected override void StopAction()
        {
            base.StopAction();
            MouseManager.MouseMove -= MoveImage;
        }

        protected override void Activated()
        {
            image.Cursor = "Hand";
        }

        private void MoveImage(Point args)
        {
            image.MoveImage(args);
            image.MoveBubbles(args);
        }
    }
}