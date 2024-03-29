﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class ResizeTool : ToolControlVM
    {
        private readonly ImageViewerVM imageVm;
        private bool first = true;
        private Point position;

        public ResizeTool(ImageViewerVM imageVm) : base(new List<Key> {Key.Z}, new List<Key> {Key.LeftCtrl, Key.Space})
        {
            this.imageVm = imageVm;
            ToolTip = "Change scale of image (S)";
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/ZoomIcon.png"));
        }

        protected override void Activated()
        {
            imageVm.Cursor = "SizeNESW";
        }

        protected override void Deactivated()
        {
            imageVm.Cursor = "Arrow";
        }

        protected override void DoAction(MouseButton pressedButton)
        {
            if (pressedButton != MouseButton.Left)
                return;
            base.DoAction(pressedButton);
            MouseManager.MouseMove += Zoom;
        }

        protected override void StopAction(MouseButton releasedButton)
        {
            base.StopAction(releasedButton);
            MouseManager.MouseMove -= Zoom;
            first = true;
        }

        private void Zoom(Point positionDelta)
        {
            if (first)
            {
                first = false;
                position = MouseManager.MousePosition;
            }

            imageVm.ScaleRegion(position, positionDelta.X / 500);
        }
    }
}