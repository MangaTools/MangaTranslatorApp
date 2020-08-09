﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class HandTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;

        public HandTool(ImageViewerVM viewer) : base(new List<Key> {Key.H}, new List<Key> {Key.Space})
        {
            _image = viewer;
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

        private void MoveImage(Point args)
        {
            _image.MoveImage(args);
            _image.MoveBubbles(args);
        }
    }
}