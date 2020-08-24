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
        private readonly ImageViewerVM imageVm;

        public HandTool(ImageViewerVM viewer) : base(new List<Key> {Key.H}, new List<Key> {Key.Space})
        {
            imageVm = viewer;
            ToolTip = "Move image (H)";
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/HandIcon.png"));
        }

        protected override void DoAction(MouseButton pressedButton)
        {
            if (pressedButton != MouseButton.Left)
                return;
            base.DoAction(pressedButton);
            MouseManager.MouseMove += MoveImage;
        }

        protected override void StopAction(MouseButton releasedButton)
        {
            base.StopAction(releasedButton);
            MouseManager.MouseMove -= MoveImage;
        }

        protected override void Activated()
        {
            imageVm.Cursor = "Hand";
        }

        protected override void Deactivated()
        {
            imageVm.Cursor = "Arrow";
        }

        private void MoveImage(Point args)
        {
            imageVm.MoveImage(args);
            imageVm.MoveBubbles(args);
        }
    }
}