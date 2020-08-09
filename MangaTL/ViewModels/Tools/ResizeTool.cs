using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class ResizeTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;
        private bool _first = true;
        private Point _position;

        public ResizeTool(ImageViewerVM image) : base(new List<Key> {Key.Z}, new List<Key> {Key.LeftCtrl, Key.Space})
        {
            _image = image;
        }

        protected override void DoAction()
        {
            base.DoAction();
            MouseManager.MouseMove += Zoom;
        }

        protected override void StopAction()
        {
            base.StopAction();
            MouseManager.MouseMove -= Zoom;
            _first = true;
        }

        private void Zoom(Point positionDelta)
        {
            if (_first)
            {
                _first = false;
                _position = MouseManager.MousePosition;
            }

            _image.ScaleRegion(_position, positionDelta.X / 500);
        }
    }
}