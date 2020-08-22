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
        private readonly ImageViewerVM imageVm;
        private BubbleVM bubble;
        private Point startPoint;

        public TextTool(ImageViewerVM imageVm) : base(new List<Key> {Key.T}, new List<Key>())
        {
            this.imageVm = imageVm;
            ToolTip = "Create bubbles (T)";
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/TextIcon.png"));
        }

        protected override void DoAction(MouseButton pressedButton)
        {
            if(pressedButton != MouseButton.Left)
                return;
            base.DoAction(pressedButton);
            startPoint = imageVm.GetRelativePoint(MouseManager.MousePosition);
            bubble = imageVm.CreateBubble(new Rectangle((int) startPoint.X, (int) startPoint.Y, 0, 0));
            MouseManager.MouseMove += MouseMove;
        }

        protected override void Activated()
        {
            imageVm.Cursor = "IBeam";
        }

        protected override void Deactivated()
        {
            imageVm.Cursor = "Arrow";
        }

        private void MouseMove(Point newPoint)
        {
            newPoint = imageVm.GetRelativePoint(MouseManager.MousePosition);
            var x = (int) Math.Min(newPoint.X, startPoint.X);
            var y = (int) Math.Min(newPoint.Y, startPoint.Y);
            var size = newPoint - startPoint;
            bubble.SetNewRect(new Rectangle(x, y, (int) Math.Abs(size.X), (int) Math.Abs(size.Y)));
            UndoManager.RemoveLast(1);
        }

        protected override void StopAction(MouseButton releasedButton)
        {
            base.StopAction(releasedButton);
            MouseManager.MouseMove -= MouseMove;
            var rect = bubble.GetBubble.Rect;
            if (rect.Width * rect.Height < 1000)
            {
                imageVm.RemoveBubble(bubble);
            }
        }
    }
}