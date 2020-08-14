using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using MangaTL.Managers;
using Point = System.Windows.Point;

namespace MangaTL.ViewModels.Tools
{
    public class TextTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;
        private BubbleVM _bubble;
        private Point _startPoint;

        public TextTool(ImageViewerVM image) : base(new List<Key> {Key.T}, new List<Key>())
        {
            _image = image;
        }

        protected override void DoAction()
        {
            base.DoAction();
            _startPoint = _image.GetRelativePoint(MouseManager.MousePosition);
            _bubble = _image.CreateBubble(new Rectangle((int) _startPoint.X, (int) _startPoint.Y, 0, 0));
            MouseManager.MouseMove += MouseMove;
        }

        private void MouseMove(Point newPoint)
        {
            newPoint = _image.GetRelativePoint(MouseManager.MousePosition);
            var x = (int) Math.Min(newPoint.X, _startPoint.X);
            var y = (int) Math.Min(newPoint.Y, _startPoint.Y);
            var size = newPoint - _startPoint;
            _bubble.SetNewRect(new Rectangle(x, y, (int) Math.Abs(size.X), (int) Math.Abs(size.Y)));
        }

        protected override void StopAction()
        {
            base.StopAction();
            MouseManager.MouseMove -= MouseMove;
        }
    }
}