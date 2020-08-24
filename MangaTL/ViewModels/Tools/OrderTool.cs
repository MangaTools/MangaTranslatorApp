using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class OrderTool : ToolControlVM
    {
        private readonly ImageViewerVM imageVm;

        public OrderTool(ImageViewerVM vm) : base(new List<Key> {Key.O}, new List<Key>())
        {
            imageVm = vm;
            ToolTip = "Change bubble order(O)";

            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/OrderIcon.png"));
        }

        protected override void Activated()
        {
            imageVm.SetOrderText();
            imageVm.PageLoaded += SetText;
        }

        protected override void Deactivated()
        {
            imageVm.PageLoaded -= SetText;
            imageVm.SetContentText();
        }

        private void SetText()
        {
            imageVm.SetOrderText();
        }

        protected override void DoAction(MouseButton pressedButton)
        {
            if (pressedButton != MouseButton.Left && pressedButton != MouseButton.Right)
                return;
            base.DoAction(pressedButton);
            ChangeOrder(MouseManager.MousePosition, pressedButton != MouseButton.Left);
        }

        private void ChangeOrder(Point mousePosition, bool up)
        {
            var bubble = imageVm.GetBubbleFromPoint(mousePosition);
            if (bubble == null)
                return;
            imageVm.ChangeBubbleOrder(bubble, up);
        }
    }
}