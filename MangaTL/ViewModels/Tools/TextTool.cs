using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Managers;
using Point = System.Windows.Point;

namespace MangaTL.ViewModels.Tools
{
    public class TextTool : ToolControlVM
    {
        private readonly ImageViewerVM image;
        private BubbleVM bubble;
        private Point startPoint;

        public TextTool(ImageViewerVM image) : base(new List<Key> {Key.T}, new List<Key>())
        {
            this.image = image;
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/TextIcon.png"));
        }

        protected override void DoAction()
        {
            base.DoAction();
            startPoint = image.GetRelativePoint(MouseManager.MousePosition);
            bubble = image.CreateBubble(new Rectangle((int) startPoint.X, (int) startPoint.Y, 0, 0));
            MouseManager.MouseMove += MouseMove;
        }

        protected override void Activated()
        {
            image.Cursor = "IBeam";
        }

        private void MouseMove(Point newPoint)
        {
            newPoint = image.GetRelativePoint(MouseManager.MousePosition);
            var x = (int) Math.Min(newPoint.X, startPoint.X);
            var y = (int) Math.Min(newPoint.Y, startPoint.Y);
            var size = newPoint - startPoint;
            bubble.SetNewRect(new Rectangle(x, y, (int) Math.Abs(size.X), (int) Math.Abs(size.Y)));
            UndoManager.RemoveLast(1);
        }

        protected override void StopAction()
        {
            base.StopAction();
            MouseManager.MouseMove -= MouseMove;
        }
    }
}