using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class HandTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;

        public override void DoAction()
        {
            InAction = true;
            MouseManager.MouseMove += MoveImage;
        }

        public override void StopAction()
        {
            InAction = false;
            MouseManager.MouseMove -= MoveImage;
        }

        private void MoveImage(Point args)
        {
            _image.MoveImage(args);
        }

        public HandTool(ImageViewerVM viewer) : base(Key.H, new List<Key> { Key.Space })
        {
            _image = viewer;
        }
    }
}